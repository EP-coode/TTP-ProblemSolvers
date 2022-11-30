// See https://aka.ms/new-console-template for more information


using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.ProblemLoader;
using GeneticAlghoritm.Logger;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Selection;
using GneticAlghoritm.GA.Crossing;
using GneticAlghoritm.Experiment;
using GneticAlghoritm.GA;
using GneticAlghoritm.TS;
using System.Reflection.Emit;
using System.Data;
using GneticAlghoritm.SA;
using System.Runtime.Intrinsics.X86;
using System.Collections.Concurrent;
using GneticAlghoritm.GA.Mutation;
using System;
using GneticAlghoritm.hybrid;

Console.WriteLine("HELLO");

Random r = new Random();

var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

GreedyItemsSelector greedyItemsSelector = new GreedyItemsSelector();
var desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

IMutationStrategy inverseMutator = new InverseMutation();
ICrossingStrategy orderedCrossover = new OrderedCrossover();

IMutationStrategy swapMutation = new SwapMutation(0.15);
ICrossingStrategy cycleCrossover = new CycleCrossover();

ISelector tournamentSelector = new Tournament(3);
ISelector rouletteSelector = new Roulette(7);


// ============================ EA ==============================

IEnumerable<EaParameters> GetEaParams(EaParamsSpace parametersSpace)
{
    foreach (var popSize in parametersSpace.PopSize)
    {
        foreach (var MutationStrategy in parametersSpace.MutationStrategy)
        {
            foreach (var MutationTreshold in parametersSpace.MutationTreshold)
            {
                foreach (var crossingStrategy in parametersSpace.CrossingStrategy)
                {
                    foreach (var SelctionStrategy in parametersSpace.SelctionStrategy)
                    {
                        foreach (var CrossingTreshold in parametersSpace.CrossingTreshold)
                        {
                            yield return new EaParameters()
                            {
                                PopSize = popSize,
                                MutationStrategy = MutationStrategy,
                                CrossingStrategy = crossingStrategy,
                                SelctionStrategy = SelctionStrategy,
                                CrossingTreshold = CrossingTreshold,
                                MutationTreshold = MutationTreshold,
                            };
                        }
                    }
                }
            }
        }
    }
}

void RunExperiment(Experiment e)
{
    Console.WriteLine("ThreadID: " + Thread.CurrentThread.ManagedThreadId);
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{e.FileName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    Individual[] best = new Individual[e.Repeats];
    List<int> experimentIds = Enumerable.Range(0, e.Repeats).ToList();
    CsvFileLogger logger2 = new CsvFileLogger();
    logger2.SetFileDest(Path.Combine(desktopLocation, e.Name, $"best_of_best.csv"), new string[] { "value" });
    int[] avaligbleGens = problem.GetGens();

    Parallel.ForEach(experimentIds, experimentId =>
    {
        CsvFileLogger logger = new CsvFileLogger();
        logger.SetFileDest(Path.Combine(desktopLocation, e.Name, $"{experimentId}_pop-{e.PopSize}_mut-{e.MutationStrategy}_{e.MutationTreshold}_cross-{e.CrossingStrategy}_{e.CrossingTreshold}_selection-{e.SelctionStrategy}.csv"), new string[] { "min", "max", "avg" });


        ProblemSlover gaSolver = new GeneticAlghoritm.GA.GeneticAlghoritm(avaligbleGens,
           e.PopSize, e.MutationTreshold, e.CrossingTreshold, evaluator, e.MutationStrategy, e.CrossingStrategy, e.SelctionStrategy)
        {
            logger = logger,
        };
        gaSolver.Run(2000);
        logger.Flush();

        logger2.Log(new string[] { $"{gaSolver.BestIndividual.Value}" });
    });

    logger2.Flush();
}

void RunEaParameterSpaceOverwiew(EaParamsSpace parametersSpace, string problemName, string outputName, int experimentRepeats, int generationsCount)
{
    var experimentNumbers = Enumerable.Range(0, experimentRepeats);
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{problemName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();
    CsvFileLogger logger = new CsvFileLogger();

    logger.SetFileDest(Path.Combine(desktopLocation, $"{outputName}.csv"), new string[] {
        "PopSize", "MutationStrategy", "MutationTreshold","CrossingStrategy", "SelctionStrategy", "CrossingTreshold",
        "Avg", "Min", "Max", "Std" });

    IEnumerable<EaParameters> gaParamsList = GetEaParams(parametersSpace);

    Parallel.ForEach(gaParamsList, parallelOptions, gaParams =>
    {
        ConcurrentBag<double> experimentScores = new ConcurrentBag<double>();

        Parallel.ForEach(experimentNumbers, parallelOptions, experimentNumber =>
        {
            ProblemSlover gaSolver = new GeneticAlghoritm.GA.GeneticAlghoritm(avalibleGens,
                gaParams.PopSize, gaParams.MutationTreshold, gaParams.CrossingTreshold, evaluator, gaParams.MutationStrategy, gaParams.CrossingStrategy, gaParams.SelctionStrategy);
            gaSolver.Run(generationsCount);
            experimentScores.Add(gaSolver.BestIndividual.Value);
        });

        var avgScore = experimentScores.Average();
        logger.Log(new string[] { $"{gaParams.PopSize}", $"{gaParams.MutationStrategy}", $"{gaParams.MutationTreshold}", $"{gaParams.CrossingStrategy}", $"{gaParams.SelctionStrategy}", $"{gaParams.CrossingTreshold}",
            $"{avgScore}", $"{experimentScores.Min()}", $"{experimentScores.Max()}", $"{Math.Sqrt(experimentScores.Average(v=>Math.Pow(v-avgScore,2)))}" });
    });

    logger.Flush();
}



// ============================ TS ==============================


void RunTsExperiment(TS_Experiment e)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{e.dataSet}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();
    CsvFileLogger logger2 = new CsvFileLogger();
    logger2.SetFileDest(Path.Combine(desktopLocation, e.experimentName, $"summary_ts_tabu{e.tabuSize}_s{e.neightbourhoodSize}_{e.generator}.csv"), new string[] { "best_value_in_run" });

    Individual[] best = new Individual[e.repeats];
    List<int> experimentIds = Enumerable.Range(0, e.repeats).ToList();

    Parallel.ForEach(experimentIds, experimentId =>
    {
        CsvFileLogger logger = new CsvFileLogger();
        logger.SetFileDest(Path.Combine(desktopLocation, e.experimentName, $"{experimentId}_ts_tabu{e.tabuSize}_s{e.neightbourhoodSize}_{e.generator}.csv"), new string[] { "current", "best", "max", "min", "avg" });


        var tsSolver = new TabuSearch(avalibleGens, evaluator, e.generator, e.neightbourhoodSize, e.tabuSize)
        {
            logger = logger
        };

        tsSolver.Run(6_000);
        logger.Flush();
        logger2.Log(new string[] { $"{tsSolver.BestIndividual.Value}" });
    });

    logger2.Flush();
}


IEnumerable<TsParameters> GetSaParamListTs(TsParametersSpace parametersSpace)
{
    foreach (var neightbourhoodSize in parametersSpace.neightbourhoodSize)
    {
        foreach (var tabuSize in parametersSpace.tabuSize)
        {
            yield return new TsParameters()
            {
                neightbourhoodSize = neightbourhoodSize,
                tabuSize = tabuSize,
            };
        }
    }
}

void RunTsParameterSpaceOverwiew(TsParametersSpace parametersSpace, string problemName, string outputName, int experimentRepeats, int maxIterations)
{
    var neighborhoodGenerator = new InverseGenerator();
    var experimentNumbers = Enumerable.Range(0, experimentRepeats);
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{problemName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();
    CsvFileLogger logger = new CsvFileLogger();

    logger.SetFileDest(Path.Combine(desktopLocation, $"{outputName}.csv"), new string[] { "neightbourhoodSize", "tabuSize", "Avg", "Min", "Max", "Std" });

    IEnumerable<TsParameters> tsParameters = GetSaParamListTs(parametersSpace);

    Parallel.ForEach(tsParameters, parallelOptions, tsParams =>
    {
        ConcurrentBag<double> experimentScores = new ConcurrentBag<double>();

        Parallel.ForEach(experimentNumbers, parallelOptions, experimentNumber =>
        {
            var tsSolver = new TabuSearch(avalibleGens, evaluator, neighborhoodGenerator, tsParams.neightbourhoodSize, tsParams.tabuSize);
            tsSolver.Run(maxIterations);
            experimentScores.Add(tsSolver.BestIndividual.Value);
        });

        var avgScore = experimentScores.Average();
        logger.Log(new string[] { $"{tsParams.neightbourhoodSize}", $"{tsParams.tabuSize}",
            $"{avgScore}", $"{experimentScores.Min()}", $"{experimentScores.Max()}", $"{Math.Sqrt(experimentScores.Average(v=>Math.Pow(v-avgScore,2)))}" });
    });

    logger.Flush();
}


// ============================ SA ==============================

void RunSaExperiment(SA_Experiment e)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{e.dataSet}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();
    CsvFileLogger logger2 = new CsvFileLogger();
    logger2.SetFileDest(Path.Combine(desktopLocation, e.ExperimentName, $"summary.csv"), new string[] { "best_value_in_run" });

    Individual[] best = new Individual[e.repeats];
    List<int> experimentIds = Enumerable.Range(0, e.repeats).ToList();

    Parallel.ForEach(experimentIds, parallelOptions, experimentId =>
    {
        CsvFileLogger logger = new CsvFileLogger();
        //$"{experimentId}_n-size_{e.NeightbourhoodSize}_start-t_{e.InitialTemperature}_min-t_{e.MinTemperature}_{e.CoolingStrategy}.csv"
        logger.SetFileDest(Path.Combine(desktopLocation, e.ExperimentName, $"{experimentId}_n-size_{e.NeightbourhoodSize}_start-t_{e.InitialTemperature}_min-t_{e.MinTemperature}_{e.CoolingStrategy}.csv"), new string[] { "current", "best", "initialTemperature" });


        var tsSolver = new SimulatedAnnealing(avalibleGens, evaluator, e.NeighbourGenerator, e.NeightbourhoodSize, e.InitialTemperature, e.MinTemperature, e.CoolingStrategy)
        {
            logger = logger
        };

        tsSolver.Run(40_000);
        logger.Flush();

        logger2.Log(new string[] { $"{tsSolver.BestIndividual.Value}" });
    });

    logger2.Flush();
}

IEnumerable<SaParams> GetSaParamListSa(SaParameterSpace parametersSpace)
{
    foreach (var minTemp in parametersSpace.MinTemperature)
    {
        foreach (var initTemp in parametersSpace.InitialTemperature)
        {
            foreach (var cooling in parametersSpace.CoolingStrategy)
            {
                yield return new SaParams()
                {
                    CoolingStrategy = cooling,
                    MinTemperature = minTemp,
                    InitialTemperature = initTemp,
                };
            }
        }
    }
}

void RunSaParameterSpaceOverwiew(SaParameterSpace parametersSpace, string problemName, string outputName, int experimentRepeats, int maxIterations)
{
    var neighborhoodGenerator = new SwapNeighbourGenerator(1);
    var experimentNumbers = Enumerable.Range(0, experimentRepeats);
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{problemName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();
    CsvFileLogger logger = new CsvFileLogger();

    logger.SetFileDest(Path.Combine(desktopLocation, $"{outputName}.csv"), new string[] { "InitialTemperature", "MinTemperature", "CoolingFactor", "Avg", "Min", "Max", "Std" });

    IEnumerable<SaParams> saParamList = GetSaParamListSa(parametersSpace);

    Parallel.ForEach(saParamList, parallelOptions, saParams =>
    {
        ConcurrentBag<double> experimentScores = new ConcurrentBag<double>();

        Parallel.ForEach(experimentNumbers, parallelOptions, experimentNumber =>
        {
            var saSolver = new SimulatedAnnealing(avalibleGens, evaluator, neighborhoodGenerator, 1, saParams.InitialTemperature, saParams.MinTemperature, saParams.CoolingStrategy);
            saSolver.Run(maxIterations);
            experimentScores.Add(saSolver.BestIndividual.Value);
        });

        var avgScore = experimentScores.Average();
        logger.Log(new string[] { $"{saParams.InitialTemperature}", $"{saParams.MinTemperature}", $"{saParams.CoolingStrategy}",
            $"{avgScore}", $"{experimentScores.Min()}", $"{experimentScores.Max()}", $"{Math.Sqrt(experimentScores.Average(v=>Math.Pow(v-avgScore,2)))}" });
    });

    logger.Flush();
}

var saParametersSpace = new SaParameterSpace()
{
    InitialTemperature = Enumerable.Range(1, 100).Select(x => x * 10d), // od 100 do 50_000; krok 200
    MinTemperature = new List<double>() { 0 },
    CoolingStrategy = Enumerable.Range(0, 1000).Select(x => new ExponentialCooling(x * 0.0001)), // 0.0001 do 0.1
};

var tsParametersSpace = new TsParametersSpace()
{
    neightbourhoodSize = Enumerable.Range(1, 10).Select(x => 2 * x).Union(Enumerable.Range(5, 5).Select(x => x * 10)).Union(Enumerable.Range(5, 5).Select(x => x * 20)),
    tabuSize = Enumerable.Range(1, 50).Select(x => x * 200), // od 50 do 10_000; krok 50
};


var gaParametersSpace = new EaParamsSpace()
{
    PopSize = new List<int>() { 10, 30, 50, 100 }, // od 10 do 1000; krok 20
    CrossingTreshold = Enumerable.Range(0, 20).Select(x => x * 0.05), // 0 do 1; krok 0,05
    MutationTreshold = Enumerable.Range(1, 20).Select(x => x * 0.05), // 0 do 1; krok 0,05
    SelctionStrategy = Enumerable.Range(0, 10).Select(x => (ISelector)new Tournament(x * 0.03)),
    //.Union(Enumerable.Range(2, 7).Select(x => (ISelector)new Roulette(x))),
    CrossingStrategy = new List<ICrossingStrategy>() { new OrderedCrossover() },
    MutationStrategy = new List<IMutationStrategy>() { new SwapCountMutation(1) },
};

void RunRandomSolver(string problemName)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{problemName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();

    CsvFileLogger logger = new CsvFileLogger();
    logger.SetFileDest(Path.Combine(desktopLocation, $"random_{problemName}.csv"), new string[] { "value" });


    for (int i = 0; i < 10_000; i++)
    {
        var shuffledCities = problem.Cities.OrderBy(x => r.Next()).Select(c => c.Index).ToArray();
        Individual individual = new Individual(shuffledCities, evaluator, false);
        logger.Log(new string[] { individual.Value.ToString() });

    }


    logger.Flush();
}

void RunGreedySolver(string problemName)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{problemName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();

    CsvFileLogger logger = new CsvFileLogger();
    logger.SetFileDest(Path.Combine(desktopLocation, $"greedy_{problemName}.csv"), new string[] { "value" });

    Parallel.ForEach(Enumerable.Range(0, 10_000), i =>
    {
        var shuffledCities = problem.Cities.OrderBy(x => r.Next()).ToArray();
        var bst = GreedyCityPathPicker.GetBestIndividual(null, evaluator, shuffledCities);
        logger.Log(new string[] { bst.Value.ToString() });
    });

    logger.Flush();
}

void RunGaSa(GaSaParams gaSaParams)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{gaSaParams.FileName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();

    CsvFileLogger logger = new CsvFileLogger();
    logger.SetFileDest(Path.Combine(desktopLocation, gaSaParams.Name, $"GaSaParams_summary.csv"), new string[] { "value" });

    Parallel.ForEach(Enumerable.Range(0, gaSaParams.Repeats), i =>
    {
        CsvFileLogger logger2 = new CsvFileLogger();
        logger2.SetFileDest(Path.Combine(desktopLocation, gaSaParams.Name, $"GaSaParams_{i}.csv"), new string[] { "min", "max", "avg" });

        var gaSaSolver = new GaSa(avalibleGens, evaluator, gaSaParams)
        {
            logger = logger2,
        };

        gaSaSolver.Run(2000);
        logger.Log(new string[] { $"{gaSaSolver.BestIndividual.Value}" });
        logger2.Flush();
    });

    logger.Flush();
}

void RunGaTe(GaTempParams gaTeParams)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{gaTeParams.FileName}");
    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
    int[] avalibleGens = problem.GetGens();

    CsvFileLogger logger = new CsvFileLogger();
    logger.SetFileDest(Path.Combine(desktopLocation, gaTeParams.Name, $"GaTe_summary.csv"), new string[] { "value" });

    Parallel.ForEach(Enumerable.Range(0, gaTeParams.Repeats), i =>
    {
        CsvFileLogger logger2 = new CsvFileLogger();
        logger2.SetFileDest(Path.Combine(desktopLocation, gaTeParams.Name, $"GaTe_{i}.csv"), new string[] { "min", "max", "avg" });

        var gaSaSolver = new GaTe(avalibleGens,
           gaTeParams.PopSize, gaTeParams.MutationTreshold, gaTeParams.Cooling, gaTeParams.CrossingTreshold, evaluator, gaTeParams.MutationStrategy,
           gaTeParams.CrossingStrategy, gaTeParams.SelctionStrategy)
        {
            logger = logger2,
        };

        gaSaSolver.Run(2000);
        logger.Log(new string[] { $"{gaSaSolver.BestIndividual.Value}" });
        logger2.Flush();
    });

    logger.Flush();
}

//RunGaSa(new GaSaParams()
//{
//    FileName = "medium_0.ttp",
//    Name = "GaSaParams_med_0",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    saGroupSize = 25,
//    saIterationsLimit = 2000,
//    saRunFrequency = 20,
//});

RunGaSa(new GaSaParams()
{
    FileName = "medium_1.ttp",
    Name = "GaSaParams_med_1",
    CrossingStrategy = new OrderedCrossover(),
    CrossingTreshold = 0.8,
    MutationStrategy = new SwapCountMutation(1),
    MutationTreshold = 0.65,
    PopSize = 50,
    Repeats = 10,
    SelctionStrategy = new Tournament(0.05),
    saGroupSize = 25,
    saIterationsLimit = 2500,
    saRunFrequency = 30,
});

//RunGaSa(new GaSaParams()
//{
//    FileName = "medium_2.ttp",
//    Name = "GaSaParams_med_2",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    saGroupSize = 25,
//    saIterationsLimit = 2000,
//    saRunFrequency = 20,
//});
//Console.WriteLine("MED2");

//RunGaSa(new GaSaParams()
//{
//    FileName = "easy_0.ttp",
//    Name = "GaSaParams_easy_0",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    saGroupSize = 5,
//    saIterationsLimit = 2000,
//    saRunFrequency = 10,
//});


//RunGaSa(new GaSaParams()
//{
//    FileName = "hard_3.ttp",
//    Name = "GaSaParams_hard_3",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    saGroupSize = 25,
//    saIterationsLimit = 2000,
//    saRunFrequency = 20,
//});

//Console.WriteLine("HARD3");

//RunGaSa(new GaSaParams()
//{
//    FileName = "hard_4.ttp",
//    Name = "GaSaParams_hard_4",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    saGroupSize = 25,
//    saIterationsLimit = 2000,
//    saRunFrequency = 50,
//});

//Console.WriteLine("HARD4");

//===========================

//RunGaTe(new GaTempParams()
//{
//    FileName = "easy_0.ttp",
//    Name = "GaTeParams_easy_0",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//}); ;

//RunGaTe(new GaTempParams()
//{
//    FileName = "easy_0.ttp",
//    Name = "GaTeParams_easy_0_cooling",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(1d / 2000d)
//});

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_1.ttp",
//    Name = "GaTeParams_med_1",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//}); ;

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_1.ttp",
//    Name = "GaTeParams_med_1_cooling",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(1d / 2000d)
//});

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_1.ttp",
//    Name = "GaTeParams_med_1",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//}); ;
//Console.WriteLine("AA");

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_0.ttp",
//    Name = "GaTeParams_med_0_cooling",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(1d / 2000d)
//});
//Console.WriteLine("AA");

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_0.ttp",
//    Name = "GaTeParams_med_0",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//});
//Console.WriteLine("AA");

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_2.ttp",
//    Name = "GaTeParams_med_2",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//}); ;

//RunGaTe(new GaTempParams()
//{
//    FileName = "medium_2.ttp",
//    Name = "GaTeParams_med_2_cooling",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(1d / 2000d)
//});

//RunGaTe(new GaTempParams()
//{
//    FileName = "hard_3.ttp",
//    Name = "GaTeParams_hard_3",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//}); ;
////Console.WriteLine("a");

//RunGaTe(new GaTempParams()
//{
//    FileName = "hard_3.ttp",
//    Name = "GaTeParams_hard_3_cooling",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(1d / 2000d)
//});
//Console.WriteLine("a");

//RunGaTe(new GaTempParams()
//{
//    FileName = "hard_4.ttp",
//    Name = "GaTeParams_hard_4",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(-1d / 2000d)
//}); ;
//Console.WriteLine("a");

//RunGaTe(new GaTempParams()
//{
//    FileName = "hard_4.ttp",
//    Name = "GaTeParams_hard_4_cooling",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 1,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    Cooling = new LinearCooling(1d / 2000d)
//});
//Console.WriteLine("a");

//===========================

// Przykłady
//RunTsExperiment(new TS_Experiment()
//{
//    dataSet = "medium_0.ttp",
//    experimentName = "TEST",
//    generator = new InverseGenerator(),
//    neightbourhoodSize = 180,
//    repeats = 1,
//    tabuSize = 400,
//});

//RunSaExperiment(new SA_Experiment()
//{
//    dataSet = "medium_1.ttp",
//    ExperimentName = "TEST",
//    CoolingStrategy = new ExponentialCooling(0.001),
//    InitialTemperature = 200,
//    MinTemperature = 10,
//    NeighbourGenerator = new SwapNeighbourGenerator(1),
//    NeightbourhoodSize = 1,
//    repeats = 1,
//});

//RunExperiment(new Experiment()
//{
//    FileName = "medium_0.ttp",
//    Name = "Ga_medium_0",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//});

//RunGaSa(new GaSaParams()
//{
//    FileName = "hard_4.ttp",
//    Name = "GaSaParams_hard_4",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.8,
//    MutationStrategy = new SwapCountMutation(1),
//    MutationTreshold = 0.65,
//    PopSize = 50,
//    Repeats = 10,
//    SelctionStrategy = new Tournament(0.05),
//    saGroupSize = 25,
//    saIterationsLimit = 2000,
//    saRunFrequency = 50,
//});


Console.WriteLine("DONE");
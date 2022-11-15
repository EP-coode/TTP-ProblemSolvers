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

Console.WriteLine("HELLO");

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
    List<ProblemSlover> problemSolvers = new();


    for (int i = 0; i < e.Repeats; i++)
    {
        CsvFileLogger logger = new CsvFileLogger();
        logger.SetFileDest(Path.Combine(desktopLocation, e.Name, $"{i}_pop-{e.PopSize}_mut-{e.MutationStrategy}_{e.MutationTreshold}_cross-{e.CrossingStrategy}_{e.CrossingTreshold}_selection-{e.SelctionStrategy}.csv"), new string[] { "min", "max", "avg" });
        int[] avalibleGens = problem.GetGens();
        ProblemSlover gaSolver = new GeneticAlghoritm.GA.GeneticAlghoritm(avalibleGens,
            e.PopSize, e.MutationTreshold, e.CrossingTreshold, evaluator, e.MutationStrategy, e.CrossingStrategy, e.SelctionStrategy)
        {
            logger = logger,
        };
        gaSolver.Run(e.PopSize);
        logger.Flush();
        problemSolvers.Add(gaSolver);
        Console.WriteLine("Task: " + e.Name + $" {i}/{e.Repeats} done");
    }

    CsvFileLogger logger2 = new CsvFileLogger();
    logger2.SetFileDest(Path.Combine(desktopLocation, e.Name, "best_of_the_bast.csv"), new string[] { "values" });
    foreach (var problemSolver in problemSolvers)
    {
        logger2.Log(new string[] { problemSolver.BestIndividual.Value.ToString() });
    };
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

        tsSolver.Run(5_000);
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

    logger2.SetFileDest(Path.Combine(desktopLocation, e.ExperimentName, $"summary_n-size_{e.NeightbourhoodSize}_start-t_{e.InitialTemperature}_min-t_{e.MinTemperature}_{e.CoolingStrategy}.csv"), new string[] { "best_value_in_run" });

    Individual[] best = new Individual[e.repeats];
    List<int> experimentIds = Enumerable.Range(0, e.repeats).ToList();

    Parallel.ForEach(experimentIds, parallelOptions, experimentId =>
    {
        CsvFileLogger logger = new CsvFileLogger();
        //$"{experimentId}_n-size_{e.NeightbourhoodSize}_start-t_{e.InitialTemperature}_min-t_{e.MinTemperature}_{e.CoolingStrategy}.csv"
        logger.SetFileDest(Path.Combine(desktopLocation, e.ExperimentName, $"test.csv"), new string[] { "current", "best", "temperature" });


        var tsSolver = new SimulatedAnnealing(avalibleGens, evaluator, e.NeighbourGenerator, e.NeightbourhoodSize, e.InitialTemperature, e.MinTemperature, e.CoolingStrategy)
        {
            logger = logger
        };

        tsSolver.Run(5_000);
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
    InitialTemperature = Enumerable.Range(1, 250).Select(x => x * 200d), // od 100 do 50_000; krok 200
    MinTemperature = new List<double>() { 10 },
    CoolingStrategy = Enumerable.Range(0, 1000).Select(x => new ExponentialCooling(x * 0.0001)), // 0.0001 do 0.1
};

var tsParametersSpace = new TsParametersSpace()
{
    neightbourhoodSize = Enumerable.Range(1, 150),
    tabuSize = Enumerable.Range(1, 300).Select(x => x * 30), // od 30 do 9_000; krok 30
};


var gaParametersSpace = new EaParamsSpace()
{
    PopSize = Enumerable.Range(1, 25).Select(x => x * 40), // od 10 do 1000; krok 20
    CrossingTreshold = Enumerable.Range(0, 20).Select(x => x * 0.05), // 0 do 1; krok 0,05
    MutationTreshold = Enumerable.Range(0, 20).Select(x => x * 0.05), // 0 do 1; krok 0,05
    SelctionStrategy = Enumerable.Range(0, 20).Select(x => (ISelector)new Tournament(x * 0.05)) //  0 do 0.5; krok 0,01
        .Union(Enumerable.Range(2, 7).Select(x => (ISelector)new Roulette(x))),
    CrossingStrategy = new List<ICrossingStrategy>() { new OrderedCrossover(), new CycleCrossover() },
    MutationStrategy = new List<IMutationStrategy>() { new InverseMutation(), new SwapCountMutation(1) },
};

RunSaParameterSpaceOverwiew(saParametersSpace, "easy_0.ttp", "easy_0-SA", 10, 5_000);
Console.WriteLine("SA DONE");

RunTsParameterSpaceOverwiew(tsParametersSpace, "easy_0.ttp", "easy_0-TS", 10, 5_000);
Console.WriteLine("TS DONE");

RunEaParameterSpaceOverwiew(gaParametersSpace, "easy_0.ttp", "easy_0-GA", 10, 1_000);
Console.WriteLine("GA DONE");

// ================================================

RunSaParameterSpaceOverwiew(saParametersSpace, "medium_0.ttp", "medium_0-SA", 10, 5_000);
Console.WriteLine("SA DONE - MED 0");

RunTsParameterSpaceOverwiew(tsParametersSpace, "medium_0.ttp", "medium_0-TS", 10, 5_000);
Console.WriteLine("TS DONE - MED 0");

RunEaParameterSpaceOverwiew(gaParametersSpace, "medium_0.ttp", "medium_0-GA", 10, 1_000);
Console.WriteLine("GA DONE - MED 0");

// ================================================

RunSaParameterSpaceOverwiew(saParametersSpace, "medium_1.ttp", "medium_1-SA", 10, 5_000);
Console.WriteLine("SA DONE - MED 1");

RunTsParameterSpaceOverwiew(tsParametersSpace, "medium_1.ttp", "medium_1-TS", 10, 5_000);
Console.WriteLine("TS DONE - MED 1");

RunEaParameterSpaceOverwiew(gaParametersSpace, "medium_1.ttp", "medium_1-GA", 10, 1_000);
Console.WriteLine("GA DONE - MED 1");

// ================================================

RunSaParameterSpaceOverwiew(saParametersSpace, "medium_2.ttp", "medium_2-SA", 10, 5_000);
Console.WriteLine("SA DONE - MED 3");

RunTsParameterSpaceOverwiew(tsParametersSpace, "medium_2.ttp", "medium_2-TS", 10, 5_000);
Console.WriteLine("TS DONE - MED 2");

RunEaParameterSpaceOverwiew(gaParametersSpace, "medium_2.ttp", "medium_2-GA", 10, 1_000);
Console.WriteLine("GA DONE - MED 2");

// ================================================

RunSaParameterSpaceOverwiew(saParametersSpace, "hard_3.ttp", "hard_3-SA", 10, 10_000);
Console.WriteLine("SA DONE - HARD 3");

RunTsParameterSpaceOverwiew(tsParametersSpace, "hard_3.ttp", "hard_3-TS", 10, 10_000);
Console.WriteLine("TS DONE - HARD ");

RunEaParameterSpaceOverwiew(gaParametersSpace, "hard_3.ttp", "hard_3-GA", 10, 3_000);
Console.WriteLine("GA DONE - HARD");

// ================================================


//RunTsExperiment(new TS_Experiment()
//{
//    dataSet = "easy_4.ttp",
//    experimentName = "TEST",
//    generator = new InverseGenerator(),
//    neightbourhoodSize = 100,
//    repeats = 1,
//    tabuSize = 5_000,
//});

//RunSaExperiment(new SA_Experiment()
//{
//    dataSet = "medium_1.ttp",
//    ExperimentName = "TEST3",
//    CoolingStrategy = new ExponentialCooling(0.001),
//    InitialTemperature = 10_000,
//    MinTemperature = -1,
//    NeighbourGenerator = new SwapNeighbourGenerator(1),
//    NeightbourhoodSize = 1,
//     repeats = 1,
//});

//RunExperiment(new Experiment() 
//{
//    FileName = "easy_1.ttp",
//    Name = "TEST",
//    CrossingStrategy = new OrderedCrossover(),
//    CrossingTreshold = 0.7,
//    MutationStrategy = new SwapCountMutation(2),
//    MutationTreshold = 0.3,
//    PopSize = 300,
//    Repeats = 1,
//    SelctionStrategy = new Tournament(0.03),
//});

Console.WriteLine("DONE");
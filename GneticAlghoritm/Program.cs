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

Console.WriteLine("HELLO");

GreedyItemsSelector itemsSelector = new GreedyItemsSelector();
var desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

IMutationStrategy inverseMutator = new InverseMutation();
ICrossingStrategy orderedCrossover = new OrderedCrossover();

IMutationStrategy swapMutation = new SwapMutation(0.15);
ICrossingStrategy cycleCrossover = new CycleCrossover();

ISelector tournamentSelector = new Tournament(3);
ISelector rouletteSelector = new Roulette(7);


void RunExperiment(Experiment e)
{
    Console.WriteLine("ThreadID: " + Thread.CurrentThread.ManagedThreadId);
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{e.FileName}");
    IEvaluator evaluator = new TTPEvaluator(problem, itemsSelector);
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

void RunTsExperiment(TS_Experiment e)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{e.dataSet}");
    IEvaluator evaluator = new TTPEvaluator(problem, itemsSelector);
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

        tsSolver.Run(2_000);
        logger.Flush();
        logger2.Log(new string[] { $"{tsSolver.BestIndividual.Value}" });
    });

    logger2.Flush();
}

void RunSaExperiment(SA_Experiment e)
{
    var problem = new TravelingThiefProblem();
    problem.LoadFromFile($".\\lab1\\dane\\{e.dataSet}");
    IEvaluator evaluator = new TTPEvaluator(problem, itemsSelector);
    int[] avalibleGens = problem.GetGens();
    CsvFileLogger logger2 = new CsvFileLogger();
    
    logger2.SetFileDest(Path.Combine(desktopLocation, e.ExperimentName, $"summary_n-size_{e.NeightbourhoodSize}_start-t_{e.InitialTemperature}_min-t_{e.MinTemperature}_{e.CoolingStrategy}.csv"), new string[] { "best_value_in_run" });

    Individual[] best = new Individual[e.repeats];
    List<int> experimentIds = Enumerable.Range(0, e.repeats).ToList();

    Parallel.ForEach(experimentIds, experimentId =>
    {
        CsvFileLogger logger = new CsvFileLogger();
        //$"{experimentId}_n-size_{e.NeightbourhoodSize}_start-t_{e.InitialTemperature}_min-t_{e.MinTemperature}_{e.CoolingStrategy}.csv"
        logger.SetFileDest(Path.Combine(desktopLocation, e.ExperimentName, $"test.csv"), new string[] { "current", "best", "temperature" });


        var tsSolver = new SimulatedAnnealing(avalibleGens, evaluator, e.NeighbourGenerator, e.NeightbourhoodSize, e.InitialTemperature, e.MinTemperature, e.CoolingStrategy)
        {
            logger = logger
        };

        tsSolver.Run(3_000);
        logger.Flush();
        logger2.Log(new string[] { $"{tsSolver.BestIndividual.Value}" });
    });

    logger2.Flush();
}

List<TS_Experiment> ts_experiments = new List<TS_Experiment>()
{
    //new TS_Experiment()
    //{
    //    experimentName = "optimal_med_3",
    //    dataSet = "medium_3.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 10,
    //    neightbourhoodSize = 10,
    //    tabuSize = 200
    //}, new TS_Experiment()
    //{
    //    experimentName = "optimal_med_4",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 10,
    //    neightbourhoodSize = 10,
    //    tabuSize = 200
    //}, new TS_Experiment()
    //{
    //    experimentName = "optimal_easy_3",
    //    dataSet = "easy_3.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 10,
    //    neightbourhoodSize = 10,
    //    tabuSize = 200
    //},
    // new TS_Experiment()
    //{
    //    experimentName = "optimal_easy_4",
    //    dataSet = "easy_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 10,
    //    neightbourhoodSize = 10,
    //    tabuSize = 200
    //},
     new TS_Experiment()
    {
        experimentName = "optimal_hard_0",
        dataSet = "hard_0.ttp",
        generator = new InverseGenerator(),
        repeats = 10,
        neightbourhoodSize = 30,
        tabuSize = 1000
    },
          new TS_Experiment()
    {
        experimentName = "optimal_hard_1",
        dataSet = "hard_1.ttp",
        generator = new InverseGenerator(),
        repeats = 10,
        neightbourhoodSize = 30,
        tabuSize = 1000
    },
    //new TS_Experiment()
    //{
    //    experimentName = "wielkość_sąsiedztwa",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 2,
    //    tabuSize = 200
    //}, new TS_Experiment()
    //{
    //    experimentName = "wielkość_sąsiedztwa",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 6,
    //    tabuSize = 200
    //}, new TS_Experiment()
    //{
    //    experimentName = "wielkość_sąsiedztwa",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 10,
    //    tabuSize = 200
    //},
    // new TS_Experiment()
    //{
    //    experimentName = "wielkość_sąsiedztwa",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 30,
    //    tabuSize = 200
    //},
    //  new TS_Experiment()
    //{
    //    experimentName = "wielkość_sąsiedztwa",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 50,
    //    tabuSize = 200
    //},
    //  new TS_Experiment()
    //  {
    //    experimentName = "wielkość_tabu",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 7,
    //    tabuSize = 0
    //  },
    //   new TS_Experiment()
    //  {
    //    experimentName = "wielkość_tabu",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 7,
    //    tabuSize = 50
    //  }
    //   , new TS_Experiment()
    //  {
    //    experimentName = "wielkość_tabu",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 7,
    //    tabuSize = 100
    //  },
    //    new TS_Experiment()
    //  {
    //    experimentName = "wielkość_tabu",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 7,
    //    tabuSize = 200
    //  }, new TS_Experiment()
    //  {
    //    experimentName = "wielkość_tabu",
    //    dataSet = "medium_4.ttp",
    //    generator = new InverseGenerator(),
    //    repeats = 1,
    //    neightbourhoodSize = 7,
    //    tabuSize = 300
    //  }
    // new TS_Experiment()
    //{
    //  experimentName = "metoda_generacji_sąsiadów",
    //  dataSet = "medium_4.ttp",
    //  generator = new InverseGenerator(),
    //  repeats = 1,
    //  neightbourhoodSize = 10,
    //  tabuSize = 200
    //},
    //        new TS_Experiment()
    //{
    //  experimentName = "metoda_generacji_sąsiadów",
    //  dataSet = "medium_4.ttp",
    //  generator = new SwapNeighbourGenerator(1),
    //  repeats = 1,
    //  neightbourhoodSize = 10,
    //  tabuSize = 200
    //},
};

//Parallel.ForEach(ts_experiments, e =>
//{
//    RunTsExperiment(e);
//});

List<SA_Experiment> sa_experiments = new List<SA_Experiment>()
{
    //new SA_Experiment()
    //{
    //    ExperimentName = "TestRun",
    //    CoolingStrategy = new LinearCooling(1.5),
    //    dataSet="medium_4.ttp",
    //    InitialTemperature=3_000,
    //    MinTemperature=10,
    //    NeighbourGenerator = new InverseGenerator(),
    //    NeightbourhoodSize = 1,
    //    repeats = 1
    //},
     new SA_Experiment()
    {
        ExperimentName = "TestRun",
        CoolingStrategy = new ExponentialCooling(0.0015),
        dataSet="medium_4.ttp",
        InitialTemperature=1_000,
        MinTemperature=10,
        NeighbourGenerator = new SwapNeighbourGenerator(1),
        NeightbourhoodSize = 1,
        repeats = 1
    },
    //      new SA_Experiment()
    //{
    //    ExperimentName = "TestRun",
    //    CoolingStrategy = new ExponentialCooling(0.002),
    //    dataSet="medium_4.ttp",
    //    InitialTemperature=5_000,
    //    MinTemperature=0,
    //    NeighbourGenerator = new InverseGenerator(),
    //    NeightbourhoodSize = 1,
    //    repeats = 1
    //},
    //new SA_Experiment()
    //{
    //    ExperimentName = "TestRun",
    //    CoolingStrategy =  new ExponentialCooling(0.003),
    //    dataSet="hard_4.ttp",
    //    InitialTemperature=10_000,
    //    MinTemperature=0,
    //    NeighbourGenerator = new InverseGenerator(),
    //    NeightbourhoodSize = 1,
    //    repeats = 1
    //},
};

Parallel.ForEach(sa_experiments, e =>
{
    RunSaExperiment(e);
});


Console.WriteLine("DONE");
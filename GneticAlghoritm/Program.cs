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

GreedyItemsSelector itemsSelector = new GreedyItemsSelector();
var desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

IMutationStrategy inverseMutator = new InverseMutation();
ICrossingStrategy orderedCrossover = new OrderedCrossover();

IMutationStrategy swapMutation = new SwapMutation(0.15);
ICrossingStrategy cycleCrossover = new CycleCrossover();

ISelector tournamentSelector = new Tournament(3);
ISelector rouletteSelector = new Roulette(7);


async Task RunExperiment(Experiment e)
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
    foreach(var problemSolver in problemSolvers)
    {
        logger2.Log(new string[] { problemSolver.BestIndividual.Value.ToString() });  
    };
    logger2.Flush();
}

var experiments = new List<Experiment>()
{
    //new Experiment
    //{
    //    Name = "PopSize",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //new Experiment
    //{
    //    Name = "PopSize",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 50,
    //    Repeats = 1,
    //},
    //new Experiment
    //{
    //    Name = "PopSize",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 20,
    //    Repeats = 1,
    //},
    // new Experiment
    //{
    //    Name = "PopSize",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 200,
    //    Repeats = 1,
    //},
    // new Experiment
    //{
    //    Name = "MutTreshold",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.02,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //  new Experiment
    //{
    //    Name = "MutTreshold",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //   new Experiment
    //{
    //    Name = "MutTreshold",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.4,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //     new Experiment
    //{
    //    Name = "CrossTreshold",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.5,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //  new Experiment
    //{
    //    Name = "CrossTreshold",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //   new Experiment
    //{
    //    Name = "CrossTreshold",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 1,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //   new Experiment
    //{
    //    Name = "SelStrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = tournamentSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //   new Experiment
    //{
    //    Name = "SelStrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = rouletteSelector,
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //   new Experiment
    //{
    //    Name = "SelStrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = new Tournament(7),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //   new Experiment
    //{
    //    Name = "SelStrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = new Tournament(1),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //   new Experiment
    //{
    //    Name = "SelStrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = new Tournament(52),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //  new Experiment
    //{
    //    Name = "CrossStrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = new Tournament(3),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //        new Experiment
    //{
    //    Name = "CrossStrategy",
    //    CrossingStrategy = new CycleCrossover(),
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = new Tournament(3),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //  new Experiment
    //{
    //    Name = "MutSrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = swapMutation,
    //    SelctionStrategy = new Tournament(3),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //     new Experiment
    //{
    //    Name = "MutSrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = new SwapMutation(0.3),
    //    SelctionStrategy = new Tournament(3),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //},
    //        new Experiment
    //{
    //    Name = "MutSrategy",
    //    CrossingStrategy = orderedCrossover,
    //    MutationStrategy = new InverseMutation(),
    //    SelctionStrategy = new Tournament(3),
    //    CrossingTreshold = 0.7,
    //    MutationTreshold = 0.2,
    //    FileName = "medium_4.ttp",
    //    PopSize = 100,
    //    Repeats = 1,
    //}
    //{
    new Experiment{
    Name = "CompareRandomAndGreedyAndAG_EASY4",
    CrossingStrategy = orderedCrossover,
    MutationStrategy = swapMutation,
    SelctionStrategy = new Tournament(3),
    CrossingTreshold = 0.7,
    MutationTreshold = 0.2,
    FileName = "easy_4.ttp",
    PopSize = 500,
    Repeats = 10,
},
        new Experiment{
    Name = "CompareRandomAndGreedyAndAG_EASY3",
    CrossingStrategy = orderedCrossover,
    MutationStrategy = swapMutation,
    SelctionStrategy = new Tournament(3),
    CrossingTreshold = 0.7,
    MutationTreshold = 0.2,
    FileName = "easy_3.ttp",
    PopSize = 500,
    Repeats = 10,
},
            new Experiment{
    Name = "CompareRandomAndGreedyAndAG_MED3",
    CrossingStrategy = orderedCrossover,
    MutationStrategy = swapMutation,
    SelctionStrategy = new Tournament(3),
    CrossingTreshold = 0.7,
    MutationTreshold = 0.2,
    FileName = "medium_3.ttp",
    PopSize = 500,
    Repeats = 10,
},
                new Experiment{
    Name = "CompareRandomAndGreedyAndAG_MED4",
    CrossingStrategy = orderedCrossover,
    MutationStrategy = swapMutation,
    SelctionStrategy = new Tournament(3),
    CrossingTreshold = 0.7,
    MutationTreshold = 0.2,
    FileName = "medium_4.ttp",
    PopSize = 500,
    Repeats = 10,
},
                    new Experiment{
    Name = "CompareRandomAndGreedyAndAG_HARD0",
    CrossingStrategy = orderedCrossover,
    MutationStrategy = swapMutation,
    SelctionStrategy = new Tournament(7),
    CrossingTreshold = 0.7,
    MutationTreshold = 0.2,
    FileName = "hard_0.ttp",
    PopSize = 500,
    Repeats = 10,
},
                        new Experiment{
    Name = "CompareRandomAndGreedyAndAG_HARD1",
    CrossingStrategy = orderedCrossover,
    MutationStrategy = swapMutation,
    SelctionStrategy = new Tournament(7),
    CrossingTreshold = 0.7,
    MutationTreshold = 0.2,
    FileName = "hard_1.ttp",
    PopSize = 500,
    Repeats = 10,
}
};

ThreadPool.SetMinThreads(16, 16);
ThreadPool.SetMaxThreads(16, 16);
var tasks = new List<Task>();

foreach (var experiment in experiments)
{
    tasks.Add(Task.Run(() => RunExperiment(experiment)));
}

await Task.WhenAll(tasks);

// GREEDY AND RANDOM

//var problem = new TravelingThiefProblem();
//problem.LoadFromFile(".\\lab1\\dane\\easy_4.ttp");

//IEvaluator evaluator = new TTPEvaluator(problem, itemsSelector);

//CsvFileLogger logger = new CsvFileLogger();
//logger.SetFileDest(Path.Combine(desktopLocation, "random.csv"), new string[] { "value" });

//CsvFileLogger logger2 = new CsvFileLogger();
//logger2.SetFileDest(Path.Combine(desktopLocation, "greedy.csv"), new string[] { "value" });

//int[] avalibleGens = problem.GetGens();

//var random = new RandomSolver(avalibleGens, evaluator, 100)
//{
//    logger = logger
//};
//random.Run(100);

//var bestIndividual = GreedyCityPathPicker.GetBestIndividual(logger2, problem);

//logger.Flush();
//logger2.Flush();

Console.WriteLine("DONE");
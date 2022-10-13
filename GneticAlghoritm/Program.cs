// See https://aka.ms/new-console-template for more information

using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.ProblemLoader;
using GeneticAlghoritm.Logger;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Selection;

var problem = new TravelingThiefProblem();
problem.LoadFromFile(".\\lab1\\dane\\easy_4.ttp");

GreedyItemsSelector itemsSelector = new GreedyItemsSelector();
IEvaluator evaluator = new TTPEvaluator(problem, itemsSelector);

// loggers
//CsvFileLogger logger = new CsvFileLogger();
var desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//logger.SetFileDest(Path.Combine(desktopLocation, "random_1.csv"), new string[] { "min", "max", "avg" });
CsvFileLogger logger2 = new CsvFileLogger();
logger2.SetFileDest(Path.Combine(desktopLocation, "ga_1.csv"), new string[] { "min", "max", "avg" });


IMutationStrategy mutator = new SwapMutation(0.3);
ICrossingStrategy crossingStrategy = new OrderedCrossover();
ISelector parentSelector = new Tournament(5);

int[] avalibleGens = problem.GetGens();

//ProblemSlover randomSolver = new RandomSolver(avalibleGens, evaluator, 10)
//{
//    logger = logger,
//};

ProblemSlover gaSolver = new GeneticAlghoritm.GA.GeneticAlghoritm(avalibleGens, 200, 0.4, 0, evaluator, mutator, crossingStrategy, parentSelector)
{
    logger = logger2,
};

gaSolver.Run(500);
//randomSolver.Run(10);

//logger.Flush();
logger2.Flush();

Console.WriteLine("END");
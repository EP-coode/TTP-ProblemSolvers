// See https://aka.ms/new-console-template for more information

using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.ProblemLoader;
using GeneticAlghoritm.Logger;

var problem = new TravelingThiefProblem();
problem.LoadFromFile(".\\lab1\\dane\\easy_0.ttp");

var itemsSelector = new GreedyItemsSelector();
IEvaluator evaluator = new TTPEvaluator(problem, itemsSelector);
CsvFileLogger logger = new CsvFileLogger();
var desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
logger.SetFileDest(Path.Combine(desktopLocation, "random_1.csv"), new string[] { "min", "max", "avg" });

int[] avalibleGens = problem.GetGens();

ProblemSlover randomSolver = new RandomSolver(avalibleGens, evaluator)
{
    logger = logger,
};

randomSolver.Run(100);

logger.Flush();

Console.WriteLine("HELLO");
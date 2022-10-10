// See https://aka.ms/new-console-template for more information

using GneticAlghoritm.GA;
using GneticAlghoritm.GA.Evaluation;
using GneticAlghoritm.ProblemLoader;

var loader = new TravelingThiefProblem();

loader.LoadFromFile("C:\\Users\\epenl\\Desktop\\GitHub\\TTP-GA\\GneticAlghoritm\\lab1\\dane\\easy_0.ttp");

IEvaluator evaluator = new TTPEvaluator();

var gaSolver = new GeneticAlghoritm(loader.GetGens(), 0.1, evaluator)
{
   evaluator = evaluator,
};

Console.WriteLine("HELLO");
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemLoader;
using ProblemSolvers.ProblemSolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.Logger;
using System.Reflection;

namespace ProblemSolvers.Experiments;



//IEnumerable<EaParameters> GetEaParams(EaParamsSpace parametersSpace)
//{
//    foreach (var popSize in parametersSpace.PopSize)
//    {
//        foreach (var MutationStrategy in parametersSpace.MutationStrategy)
//        {
//            foreach (var MutationTreshold in parametersSpace.MutationTreshold)
//            {
//                foreach (var crossingStrategy in parametersSpace.CrossingStrategy)
//                {
//                    foreach (var SelctionStrategy in parametersSpace.SelctionStrategy)
//                    {
//                        foreach (var CrossingTreshold in parametersSpace.CrossingTreshold)
//                        {
//                            yield return new EaParameters()
//                            {
//                                PopSize = popSize,
//                                MutationStrategy = MutationStrategy,
//                                CrossingStrategy = crossingStrategy,
//                                SelctionStrategy = SelctionStrategy,
//                                CrossingTreshold = CrossingTreshold,
//                                MutationTreshold = MutationTreshold,
//                            };
//                        }
//                    }
//                }
//            }
//        }
//    }
//}

//void RunExperiment(Experiment e)
//{
//    Console.WriteLine("ThreadID: " + Thread.CurrentThread.ManagedThreadId);
//    var problem = new TravelingThiefProblem();
//    problem.LoadFromFile($".\\lab1\\dane\\{e.FileName}");
//    IEvaluator evaluator = new TTPEvaluator(problem, greedyItemsSelector);
//    PermutationIndividual[] best = new PermutationIndividual[e.Repeats];
//    List<int> experimentIds = Enumerable.Range(0, e.Repeats).ToList();
//    CsvFileLogger summaryLogger = new CsvFileLogger();
//    summaryLogger.SetFileDest(Path.Combine(desktopLocation, e.Name, $"best_of_best.csv"), new string[] { "value" });
//    int[] avaligbleGens = problem.GetGens();

//    Parallel.ForEach(experimentIds, experimentId =>
//    {
//        CsvFileLogger logger = new CsvFileLogger();
//        logger.SetFileDest(Path.Combine(desktopLocation, e.Name, $"{experimentId}_pop-{e.PopSize}_mut-{e.MutationStrategy}_{e.MutationTreshold}_cross-{e.CrossingStrategy}_{e.CrossingTreshold}_selection-{e.SelctionStrategy}.csv"), new string[] { "min", "max", "avg" });


//        ProblemSlover gaSolver = new ProblemSolvers.ProblemSolvers.ProblemSolvers(avaligbleGens,
//           e.PopSize, e.MutationTreshold, e.CrossingTreshold, evaluator, e.MutationStrategy, e.CrossingStrategy, e.SelctionStrategy)
//        {
//            logger = logger,
//        };
//        gaSolver.Run(2000);
//        logger.Flush();

//        summaryLogger.Log(new string[] { $"{gaSolver.BestIndividual.Value}" });
//    });

//    summaryLogger.Flush();
//}

public class ExperimentRunner<P, T, L> where P : ProblemSolverParams where T : ProblemSlover<L> where L : ILogable
{
    readonly static ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
    public static void RunExperiments(Experiment<P>[] experiments, ProblemSolverFactory<T,P,L> solverFactory, string problemFileName, string baseDst)
    {
        var problem = new TravelingThiefProblem();
        problem.LoadFromFile($".\\lab1\\dane\\{problemFileName}");
        IEvaluator evaluator = new TTPEvaluator(problem, new GreedyItemsSelector());
        int[] avaligbleGens = problem.GetGens();

        Parallel.ForEach(experiments, parallelOptions, experiment =>
        {
            CsvFileLogger<SingleValue> summaryLogger = new();
            summaryLogger.SetFileDest(Path.Combine(baseDst, experiment.Name, $"summary.csv"));
            double[] scores = new double[experiment.Repeats];
            var experimentNumbers = Enumerable.Range(0, experiment.Repeats);
            Parallel.ForEach(experimentNumbers, parallelOptions, experimentNo =>
            {
                CsvFileLogger<L> experimentLogger = new();
                var fileName = GenerateFileName(experiment.ProblemSolverParams, experimentNo);
                experimentLogger.SetFileDest(Path.Combine(baseDst, experiment.Name, fileName));
                var solver = solverFactory.GetProblemSolver(experiment.ProblemSolverParams, avaligbleGens, evaluator, experimentLogger);
                solver.Run(experiment.MaxIterations);
                scores[experimentNo] = solver.BestIndividual.Value;
            });
        });
    }

    private static string GenerateFileName(P problemSolverParams, int experimentNumber)
    {
        var sb = new StringBuilder();
        sb.Append(experimentNumber.ToString());
        sb.Append('_');

        foreach (PropertyInfo propertyInfo in problemSolverParams.GetType().GetProperties())
        {
            sb.Append($"{propertyInfo.Name}={propertyInfo.GetValue(problemSolverParams, null)}_");
        }

        sb.Append(".csv");

        return sb.ToString();
    }
}


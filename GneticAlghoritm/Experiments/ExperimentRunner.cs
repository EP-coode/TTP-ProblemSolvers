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

public class ExperimentRunner<P, T, L> where P : ProblemSolverParams where T : ProblemSlover<L> where L : ILogable
{
    readonly static ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
    public static void RunExperiments(IEnumerable<Experiment<P>> experiments, ProblemSolverFactory<T,P,L> solverFactory, string problemFileName, string baseDst)
    {
        var problem = new TravelingThiefProblem();
        problem.LoadFromFile($".\\lab1\\dane\\{problemFileName}");
        IEvaluator evaluator = new TTPEvaluator(problem, new GreedyItemsSelector());
        int[] avaligbleGens = problem.GetGens();

        Parallel.ForEach(experiments, parallelOptions, experiment =>
        {
            CsvFileLogger<SingleValue> summaryLogger = new();
            summaryLogger.SetFileDest(Path.Combine(baseDst, experiment.OutputFolder, $"summary.csv"));
            var experimentNumbers = Enumerable.Range(0, experiment.Repeats);
            Parallel.ForEach(experimentNumbers, parallelOptions, experimentNo =>
            {
                CsvFileLogger<L> experimentLogger = new();
                var fileName = GenerateFileName(experiment.ProblemSolverParams, experimentNo);
                experimentLogger.SetFileDest(Path.Combine(baseDst, experiment.OutputFolder, fileName));
                var solver = solverFactory.GetProblemSolver(experiment.ProblemSolverParams, avaligbleGens, evaluator, experimentLogger);
                solver.Run(experiment.MaxIterations);
                summaryLogger.Log(new SingleValue(solver.BestIndividual.Value));
                experimentLogger.Flush();
            });

            summaryLogger.Flush();
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


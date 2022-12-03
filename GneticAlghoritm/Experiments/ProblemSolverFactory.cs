using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers.GA;
using ProblemSolvers.ProblemSolvers.SA;

namespace ProblemSolvers.Experiments;

public abstract class ProblemSolverFactory<T, P, L> where T : ProblemSlover<L> where P : ProblemSolverParams where L : ILogable
{
    public abstract T GetProblemSolver(P solverParams, int[] avalibleGens, IEvaluator evaluator, ILogger<L>? logger = null);
}

public class GaSolverFactory : ProblemSolverFactory<GeneticAlghoritm, GaParams, MinMaxAvg>
{
    public override GeneticAlghoritm GetProblemSolver(GaParams solverParams, int[] avalibleGens, IEvaluator evaluator, ILogger<MinMaxAvg> logger)
    {
        return new GeneticAlghoritm(solverParams, avalibleGens, evaluator)
        {
            logger = logger
        };
    }
}

public class SaSolverFactory : ProblemSolverFactory<SimulatedAnnealing, SaParams, SaLog>
{
    public override SimulatedAnnealing GetProblemSolver(SaParams solverParams, int[] avalibleGens, IEvaluator evaluator, ILogger<SaLog> logger)
    {
        return new SimulatedAnnealing(solverParams, avalibleGens, evaluator)
        {
            logger = logger
        };
    }
}
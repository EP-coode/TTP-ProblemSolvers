using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.RandomSolver;

internal class RandomSolver : ProblemSlover<SingleValue>
{
    public RandomSolver(int[] populationGenes, IEvaluator evaluator) : base(new ProblemSolverParams(1), populationGenes, evaluator)
    {
    }

    public override void Run(int generations)
    {
        for (int i = 0; i < generations; i++)
        {
            NextGeneration(i);
        }
    }

    protected override void NextGeneration(int currentGeneration)
    {
        GenRandomPopulation();
        //SaveStatsToLog();
        foreach (var individual in Population)
        {
            logger?.Log(new SingleValue(individual.Value));
        }
        PermutationIndividual generationBestIndividual = GetBestIndividualOfGeneration();
        if (BestIndividual is null || generationBestIndividual.Value > BestIndividual.Value)
        {
            BestIndividual = new PermutationIndividual(generationBestIndividual);
        }
    }
}


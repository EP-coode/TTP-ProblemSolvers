using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers.Mutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers.GA.Selection;

namespace ProblemSolvers.ProblemSolvers.GA;

public class GeneticAlghoritm : ProblemSlover<MinMaxAvg>
{
    private Random r = new Random();
    protected double mutationFrequencyTreshold;
    private double crossingFrequency;
    public ICrossingStrategy crossingStrategy { get; init; }
    public IMutationStrategy mutationStrategy { get; init; }
    public ISelector parentSelector { get; init; }

    public GeneticAlghoritm(GaParams gaParams, int[] populationGenes, IEvaluator evaluator)
        : base(gaParams, populationGenes, evaluator)
    {
        mutationFrequencyTreshold = gaParams.MutationFrequencyTreshold;
        mutationStrategy = gaParams.MutationStrategy;
        crossingStrategy = gaParams.CrossingStrategy;
        parentSelector = gaParams.ParentSelector;
        crossingFrequency = gaParams.CrossingFrequency;
    }

    public override void Run(int generations)
    {

        for (int i = 0; i < generations; i++)
        {
            NextGeneration(i);
            SaveStatsToLog();
        }
    }

    protected override void NextGeneration(int generation)
    {
        //Console.WriteLine($"Pokolenie {i}");
        PermutationIndividual[] nextGeneration = new PermutationIndividual[Population.Length];

        // reproduction
        for (int j = 0; j < nextGeneration.Length;)
        {
            var p1 = parentSelector.SelectParent(Population);
            var p2 = parentSelector.SelectParent(Population);
            bool willCross = r.NextDouble() < crossingFrequency;

            if (willCross)
            {
                var childrens = crossingStrategy.Cross(p1, p2);

                for (int k = 0; k < childrens.Length && k + j < nextGeneration.Length; k++)
                {
                    nextGeneration[j + k] = childrens[k];
                }

                j += childrens.Length;
            }
            else
            {
                nextGeneration[j] = p1;
                j++;

                if (nextGeneration.Length > j)
                {
                    nextGeneration[j] = p2;
                    j++;
                }
            }

        }

        // mutation
        for (int j = 0; j < nextGeneration.Length; j++)
        {
            bool willMutate = r.NextDouble() < mutationFrequencyTreshold;

            if (willMutate)
            {
                nextGeneration[j] = new PermutationIndividual(nextGeneration[j]);
                mutationStrategy.Mutate(nextGeneration[j]);
            }
        }

        Population = nextGeneration;

        var bestOfGen = GetBestIndividualOfGeneration();
        if (bestOfGen is not null && (BestIndividual is null || bestOfGen.Value > BestIndividual.Value))
        {
            BestIndividual = new PermutationIndividual(bestOfGen);
        }


    }

    protected void SaveStatsToLog()
    {
        if (logger is null) return;

        var scores = Population.Select(individual => evaluator.Evaluate(individual));
        var (max, min, avg) = (scores.Max(), scores.Min(), scores.Average()); ;
        logger?.Log(new MinMaxAvg(min, max, avg));
    }
}


using GeneticAlghoritm.Logger;
using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlghoritm.GA.Crossing;

namespace GeneticAlghoritm.GA;

public class GeneticAlghoritm : ProblemSlover
{
    private double mutationFrequencyTreshold;
    private double crossingFrequency;
    Random r = new Random();
    public ICrossingStrategy crossingStrategy { get; init; }
    public IMutationStrategy mutationStrategy { get; init; }
    public ISelector parentSelector { get; init; }

    public GeneticAlghoritm(int[] populationGenes, int populationSize, double mutationFrequencyTreshold, double crossingFrequency, IEvaluator evaluator,
        IMutationStrategy mutationStrategy, ICrossingStrategy crossingStrategy, ISelector parentSelector)
        : base(populationGenes, evaluator, populationSize)
    {
        this.mutationFrequencyTreshold = mutationFrequencyTreshold;
        this.mutationStrategy = mutationStrategy;
        this.crossingStrategy = crossingStrategy;
        this.parentSelector = parentSelector;
        this.crossingFrequency = crossingFrequency;
    }

    public override void Run(int generations)
    {

        for (int i = 0; i < generations; i++)
        {
            if (StopPredicates is not null && StopPredicates.Length != 0 && StopPredicates.Any(predicate => predicate.MustStop(this)))
                return;

            Individual[] nextGeneration = new Individual[Population.Length];

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
                    mutationStrategy.Mutate(nextGeneration[j]);
            }

            Population = nextGeneration;

            var bestOfGen = GetBestIndividualOfGeneration();
            if (BestIndividual is null || bestOfGen.Value > BestIndividual.Value)
            {
                BestIndividual = new Individual(bestOfGen);
            }


            SaveStatsToLog();
        }
    }

}


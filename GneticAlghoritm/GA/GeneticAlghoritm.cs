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

namespace GeneticAlghoritm.GA;

public class GeneticAlghoritm : ProblemSlover
{
    private double mutationFrequencyTreshold;
    public ICrossingStrategy crossingStrategy { get; init; }
    public IMutationStrategy mutationStrategy { get; init; }
    public ISelector parentSelector { get; init; }

    public GeneticAlghoritm(int[] populationGenes, double mutationFrequencyTreshold, IEvaluator evaluator) 
        : base(populationGenes, evaluator)
    {
        this.mutationFrequencyTreshold = mutationFrequencyTreshold;
    }

    public override void Run(int generations)
    {
        Random r = new Random();

        for (int i = 0; i < generations; i++)
        {
            if (StopPredicates.Length != 0 && StopPredicates.Any(predicate => predicate.MustStop(this)))
                return;

            Individual[] nextGeneration = new Individual[Population.Length];

            // reproduction
            for (int j = 0; j < nextGeneration.Length;)
            {
                var p1 = parentSelector.SelectParent(Population);
                var p2 = parentSelector.SelectParent(Population);

                var childrens = crossingStrategy.Cross(p1, p2);

                for (int k = 0; k < childrens.Length; k++)
                {
                    nextGeneration[j + k] = childrens[k];
                }

                j += childrens.Length;
            }

            // mutation
            for (int j = 0; j < nextGeneration.Length;)
            {
                bool willMutate = r.NextDouble() < mutationFrequencyTreshold;

                if (willMutate)
                    mutationStrategy.Mutate(nextGeneration[j]);
            }

            Population = nextGeneration;

            SaveStatsToLog();
        }
    }

}


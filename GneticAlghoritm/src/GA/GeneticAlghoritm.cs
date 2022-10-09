using GneticAlghoritm.Logger;
using GneticAlghoritm.src.GA;
using GneticAlghoritm.src.GA.Evaluation;
using GneticAlghoritm.src.GA.Mutation;
using GneticAlghoritm.src.GA.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA;

internal class GeneticAlghoritm
{
    public Individual[] Population { get; private set; }
    private int[] genPool;
    private double mutationFrequencyTreshold;
    private ICrossingStrategy crossingStrategy { get; init; }
    private IMutationStrategy mutationStrategy { get; init; }
    private ISelector parentSelector { get; init; }
    private ILogger logger { get; init; }
    private IStopPredicate[] StopPredicates { get; init; }
    private IEvaluator evaluator { get; }

    public GeneticAlghoritm(int[] populationGenes, double mutationFrequencyTreshold, IEvaluator evaluator)
    {
        genPool = populationGenes;
        GenRandomPopulation();
        this.evaluator = evaluator;
        this.mutationFrequencyTreshold = mutationFrequencyTreshold;
    }

    public void GenRandomPopulation()
    {
        Population = new Individual[genPool.Length];

        for (int i = 0; i < Population.Length; i++)
        {
            Population[i] = new Individual(genPool);
        }
    }

    public void Run(int generations)
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

            logStats();
        }
    }

    private void logStats()
    {
        var (max, min, avg) = GetPopulationStats();
        logger.Log($"{min},{max},{avg}");
    }

    public Individual? GetBestIndividual()
    {
        return Population.MaxBy(individual => evaluator.Evaluate(individual));
    }

    public (double, double, double) GetPopulationStats()
    {
        var scores = Population.Select(individual => evaluator.Evaluate(individual));

        return (scores.Max(), scores.Min(), scores.Average());
    }
}


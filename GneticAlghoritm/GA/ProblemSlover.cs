using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.Logger;

namespace GeneticAlghoritm.GA;

public abstract class ProblemSlover
{
    public Individual[] Population { get; protected set; }
    public Individual BestIndividual { get; protected set; }

    public int PopulationSize { get; private set; }

    private int[] genPool;
    public ILogger? logger { get; init; }
    public IStopPredicate[] StopPredicates { get; init; }
    protected IEvaluator evaluator;

    public ProblemSlover(int[] populationGenes, IEvaluator evaluator, int populationSize)
    {
        genPool = populationGenes;
        this.evaluator = evaluator;
        this.PopulationSize = populationSize;
        GenRandomPopulation();
    }

    public void GenRandomPopulation()
    {
        Population = new Individual[PopulationSize];

        for (int i = 0; i < PopulationSize; i++)
        {
            if (Population[i] is null)
            {
                Population[i] = new Individual(genPool, evaluator, shuffleGenome: true);
            }

            Population[i].Randomize();
        }

        UpdateBestIndividual();
    }

    public void UpdateBestIndividual()
    {
        var bestOfGen = GetBestIndividualOfGeneration();
        if (BestIndividual is null || bestOfGen.Value > BestIndividual.Value)
        {
            BestIndividual = new Individual(bestOfGen);
        }
    }

    public abstract void Run(int generations);

    public void SaveStatsToLog()
    {
        var (max, min, avg) = GetPopulationStats();
        logger?.Log(new string[] { min.ToString(), max.ToString(), avg.ToString() });
    }

    public Individual? GetBestIndividualOfGeneration()
    {
        return Population.MaxBy(individual => individual.Value);
    }

    public (double, double, double) GetPopulationStats()
    {
        var scores = Population.Select(individual => evaluator.Evaluate(individual));

        return (scores.Max(), scores.Min(), scores.Average());
    }
}

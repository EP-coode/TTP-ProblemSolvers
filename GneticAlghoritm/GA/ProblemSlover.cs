using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.Logger;

namespace GeneticAlghoritm.GA;

public abstract class ProblemSlover
{
    public Individual[] Population { get; protected set; }
    public Individual BestIndividual { get; protected set; }

    private int[] genPool;
    public ILogger logger { get; init; }
    public IStopPredicate[] StopPredicates { get; init; }
    protected IEvaluator evaluator;

    public ProblemSlover(int[] populationGenes, IEvaluator evaluator)
    {
        genPool = populationGenes;
        this.evaluator = evaluator;
        GenRandomPopulation();
    }

    public void GenRandomPopulation()
    {
        Population = new Individual[genPool.Length];

        for (int i = 0; i < Population.Length; i++)
        {
            if (Population[i] is null)
                Population[i] = new Individual(genPool, evaluator);
            else
                Population[i].Randomize();
        }
    }

    public abstract void Run(int generations);

    public void SaveStatsToLog()
    {
        var (max, min, avg) = GetPopulationStats();
        logger.Log(new string[] { min.ToString(), max.ToString(), avg.ToString() });
    }

    public Individual? GetBestIndividualOfGeneration()
    {
        return Population.MaxBy(individual => evaluator.Evaluate(individual));
    }

    public (double, double, double) GetPopulationStats()
    {
        var scores = Population.Select(individual => evaluator.Evaluate(individual));

        return (scores.Max(), scores.Min(), scores.Average());
    }
}

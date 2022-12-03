using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.Logger;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers;

public abstract class ProblemSlover<L> where L : ILogable
{
    public PermutationIndividual[] Population { get; protected set; }
    public PermutationIndividual BestIndividual { get; protected set; }
    public int PopulationSize { get; private set; }
    protected readonly int[] genPool;
    public ILogger<L>? logger { get; init; }
    protected IEvaluator evaluator;

    public ProblemSlover(ProblemSolverParams problemParams, int[] populationGenes, IEvaluator evaluator)
    {
        genPool = populationGenes;
        this.evaluator = evaluator;
        PopulationSize = problemParams.PopulationSize;
        GenRandomPopulation();
    }

    public void GenRandomPopulation()
    {
        Population = new PermutationIndividual[PopulationSize];

        for (int i = 0; i < PopulationSize; i++)
        {
            if (Population[i] is null)
            {
                Population[i] = new PermutationIndividual(genPool, evaluator, shuffleGenome: true);
            }

            Population[i].Randomize();
        }

        UpdateBestIndividual();
    }

    public void UpdateBestIndividual()
    {

        var bestOfGen = GetBestIndividualOfGeneration();

        if (bestOfGen == null) return;

        if (BestIndividual is null || bestOfGen.Value > BestIndividual.Value)
        {
            BestIndividual = new PermutationIndividual(bestOfGen);
        }
    }

    public abstract void Run(int generations);

    protected abstract void NextGeneration(int currentGeneration);

    public PermutationIndividual? GetBestIndividualOfGeneration()
    {
        return Population.MaxBy(individual => individual.Value);
    }
}

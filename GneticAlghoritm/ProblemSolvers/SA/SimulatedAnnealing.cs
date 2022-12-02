using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.SA;
using ProblemSolvers.ProblemSolvers.SA.Cooling;
using ProblemSolvers.ProblemSolvers.TS.NeighbourHood;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.SA;

public class SimulatedAnnealing : ProblemSlover<CurrentBestTemp>
{
    private INeighbourGenerator neighbourGenerator;
    private int neightbourhoodSize;
    private double initialTemperature;
    private double minTemperature;
    private PermutationIndividual currentIndividual;
    private Random random = new Random();
    private ICoolingStrategy coolingStrategy;
    public double AcceptAmmount { get; private set; } = 0;

    public SimulatedAnnealing(SaParams saParams, int[] populationGenes, IEvaluator evaluator) : this(saParams, new PermutationIndividual(populationGenes, evaluator, shuffleGenome: true), populationGenes, evaluator)
    {
    }

    public SimulatedAnnealing(SaParams saParams, PermutationIndividual startingIndividual, int[] populationGenes, IEvaluator evaluator) : base(saParams, populationGenes, evaluator)
    {
        neighbourGenerator = saParams.neighbourGenerator;
        neightbourhoodSize = 0;
        initialTemperature = saParams.InitialTemperature;
        minTemperature = saParams.MinTemperature;
        currentIndividual = startingIndividual;
        BestIndividual = new PermutationIndividual(currentIndividual);
        coolingStrategy = saParams.CoolingStrategy;
    }

    public override void Run(int generations)
    {
        logger?.Log(new CurrentBestTemp(BestIndividual.Value, currentIndividual.Value, initialTemperature));

        for (int i = 0; i < generations; i++)
        {
            NextGeneration(i);

            if (coolingStrategy.GetTemperature(i, initialTemperature) < minTemperature)
            {
                break;
            }
        }

    }

    private bool CanAcceptNeighbour(PermutationIndividual neightbour)
    {
        double deltaFit = neightbour.Value - currentIndividual.Value;

        if (deltaFit > 0)
        {
            return true;
        }

        double acceptProb = Math.Exp(deltaFit / initialTemperature);
        bool accept = random.NextDouble() < acceptProb;
        return accept;
    }

    protected override void NextGeneration(int currentGeneration)
    {
        PermutationIndividual[] neightbours = neighbourGenerator.GetNeighbours(neightbourhoodSize, currentIndividual);
        PermutationIndividual bestNeightbour = neightbours.MaxBy(individual => individual.Value);

        if (CanAcceptNeighbour(bestNeightbour))
        {
            currentIndividual = bestNeightbour;
            AcceptAmmount += 1;

            if (BestIndividual.Value < bestNeightbour.Value)
            {
                BestIndividual = new PermutationIndividual(bestNeightbour);
            }
        }

        var temperature = coolingStrategy.GetTemperature(currentGeneration, initialTemperature);


        logger?.Log(new CurrentBestTemp(BestIndividual.Value, currentIndividual.Value, temperature));
    }
}


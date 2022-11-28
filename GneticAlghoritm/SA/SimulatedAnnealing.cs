using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;
using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.SA;

public class SimulatedAnnealing : ProblemSlover
{
    private INeighbourGenerator neighbourGenerator;
    private int neightbourhoodSize;
    private double initialTemperature;
    private double minTemperature;
    private Individual currentIndividual;
    private Random random = new Random();
    private ICoolingStrategy coolingStrategy;
    public double AcceptAmmount { get; private set; } = 0;

    public SimulatedAnnealing(int[] populationGenes, IEvaluator evaluator, INeighbourGenerator neighbourGenerator,
        int neightbourhoodSize, double initialTemperature, double minTemperature, ICoolingStrategy coolingStrategy) : this(populationGenes, evaluator, neighbourGenerator,
     neightbourhoodSize, initialTemperature, minTemperature, coolingStrategy, new Individual(populationGenes, evaluator, shuffleGenome: true))
    {

    }

    public SimulatedAnnealing(int[] populationGenes, IEvaluator evaluator, INeighbourGenerator neighbourGenerator,
    int neightbourhoodSize, double initialTemperature, double minTemperature, ICoolingStrategy coolingStrategy, Individual startingIndividual) : base(populationGenes, evaluator, 1)
    {
        this.neighbourGenerator = neighbourGenerator;
        this.neightbourhoodSize = neightbourhoodSize;
        this.initialTemperature = initialTemperature;
        this.minTemperature = minTemperature;
        currentIndividual = startingIndividual;
        BestIndividual = new Individual(currentIndividual);
        this.coolingStrategy = coolingStrategy;
    }

    public override void Run(int generations)
    {
        logger?.Log(new string[] { $"{currentIndividual.Value}", $"{BestIndividual.Value}", $"{initialTemperature}" });

        for (int i = 0; i < generations; i++)
        {
            NextGeneration(i);

            if (coolingStrategy.GetTemperature(i, initialTemperature) < minTemperature)
            {
                break;
            }
        }

    }

    private bool CanAcceptNeighbour(Individual neightbour)
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
        Individual[] neightbours = neighbourGenerator.GetNeighbours(neightbourhoodSize, currentIndividual);
        Individual bestNeightbour = neightbours.MaxBy(individual => individual.Value);

        if (CanAcceptNeighbour(bestNeightbour))
        {
            currentIndividual = bestNeightbour;
            AcceptAmmount += 1;

            if (BestIndividual.Value < bestNeightbour.Value)
            {
                BestIndividual = new Individual(bestNeightbour);
            }
        }

        var temperature = coolingStrategy.GetTemperature(currentGeneration, initialTemperature);


        logger?.Log(new string[] { $"{currentIndividual.Value}", $"{BestIndividual.Value}", $"{temperature}" });
    }
}


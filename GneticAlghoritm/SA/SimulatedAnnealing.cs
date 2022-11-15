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
    private double temperature;
    private double minTemperature;
    private Individual currentIndividual;
    private Random random = new Random();
    private ICoolingStrategy coolingStrategy;
    public double AcceptAmmount { get; private set; } = 0;

    public SimulatedAnnealing(int[] populationGenes, IEvaluator evaluator, INeighbourGenerator neighbourGenerator,
        int neightbourhoodSize, double initialTemperature, double minTemperature, ICoolingStrategy coolingStrategy) : base(populationGenes, evaluator, 1)
    {
        this.neighbourGenerator = neighbourGenerator;
        this.neightbourhoodSize = neightbourhoodSize;
        this.temperature = initialTemperature;
        this.minTemperature = minTemperature;
        currentIndividual = new Individual(populationGenes, evaluator, shuffleGenome: true);
        BestIndividual = new Individual(currentIndividual);
        this.coolingStrategy = coolingStrategy;
    }

    public override void Run(int generations)
    {
        logger?.Log(new string[] { $"{currentIndividual.Value}", $"{BestIndividual.Value}", $"{temperature}" });
        int iterations = 0;

        for (int i = 0; i < generations; i++)
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

            temperature = coolingStrategy.CoolDown(temperature);


            logger?.Log(new string[] { $"{currentIndividual.Value}", $"{BestIndividual.Value}", $"{temperature}" });

            iterations = i;

            if (temperature < minTemperature)
            {
                break;
            }
        }
        //Console.WriteLine($"Wyrażenie akceptowane w {(AcceptAmmount/iterations) * 100} % przypadków");
    }

    private bool CanAcceptNeighbour(Individual neightbour)
    {
        double deltaFit = neightbour.Value - currentIndividual.Value;

        if (deltaFit > 0)
        {
            return true;
        }

        double acceptProb = Math.Exp(deltaFit / temperature);
        bool accept = random.NextDouble() < acceptProb;
        return accept;
    }
}


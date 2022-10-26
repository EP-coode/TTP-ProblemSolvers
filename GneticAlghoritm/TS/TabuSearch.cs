using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;

namespace GneticAlghoritm.TS;

public class TabuSearch : ProblemSlover
{
    private INeighbourGenerator neighbourGenerator;
    private Queue<Individual> tabuList;
    private int tabuSize;
    private int neightbourhoodSize;
    private Individual currentIndividual;

    public TabuSearch(int[] populationGenes, IEvaluator evaluator, INeighbourGenerator neighbourGenerator, int neightbourhoodSize, int tabuSize) : base(populationGenes, evaluator, 1)
    {
        tabuList = new Queue<Individual>();
        this.tabuSize = tabuSize;
        this.neighbourGenerator = neighbourGenerator;
        this.neightbourhoodSize = neightbourhoodSize;
        this.currentIndividual = new Individual(populationGenes, evaluator, shuffleGenome: true);
    }

    public override void Run(int generations)
    {
        for (int i = 0; i < generations; i++)
        {
            Individual[] neightbours = neighbourGenerator.GetNeighbours(neightbourhoodSize, BestIndividual);
            Individual bestSuccesor = neightbours.Where(n => !tabuList.Any(t => t.Equals(n))).MaxBy(n => n.Value);

            logger.Log(new string[] { $"{currentIndividual.Value}", $"{BestIndividual.Value}" });

            if (bestSuccesor.Value > BestIndividual.Value)
            {
                BestIndividual = bestSuccesor;
            }

            tabuList.Enqueue(bestSuccesor);

            if (tabuList.Count() > tabuSize)
            {
                tabuList.Dequeue();
            }

            currentIndividual = bestSuccesor;
        }
    }
}


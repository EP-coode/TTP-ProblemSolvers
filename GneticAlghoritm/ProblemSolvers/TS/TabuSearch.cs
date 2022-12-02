using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.TS;
using ProblemSolvers.ProblemSolvers.TS.NeighbourHood;

namespace ProblemSolvers.ProblemSolvers.TS;

public class TabuSearch : ProblemSlover<TsLog>
{
    private INeighbourGenerator neighbourGenerator;
    private Queue<PermutationIndividual> tabuList;
    private int tabuSize;
    private int neightbourhoodSize;
    private PermutationIndividual currentIndividual;

    public TabuSearch(TsParams tsParams, int[] populationGenes, IEvaluator evaluator) : base(tsParams, populationGenes, evaluator)
    {
        tabuList = new Queue<PermutationIndividual>();
        tabuSize = tsParams.TabuSize;
        neighbourGenerator = tsParams.NeighbourGenerator;
        neightbourhoodSize = tsParams.NeightbourhoodSize;
        currentIndividual = new PermutationIndividual(populationGenes, evaluator, shuffleGenome: true);
    }

    public override void Run(int generations)
    {
        for (int i = 0; i < generations; i++)
        {
            NextGeneration(i);
        }
    }

    protected override void NextGeneration(int currentGeneration)
    {
        PermutationIndividual[] neightbours = neighbourGenerator.GetNeighbours(neightbourhoodSize, currentIndividual);
        PermutationIndividual bestSuccesor = neightbours.Where(n => !tabuList.Any(t => t.Equals(n))).MaxBy(n => n.Value) ?? currentIndividual;

        logger?.Log(new TsLog(currentIndividual.Value, BestIndividual.Value, neightbours.Select(n => n.Value).Min(), neightbours.Select(n => n.Value).Max(), neightbours.Select(n => n.Value).Average()));

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


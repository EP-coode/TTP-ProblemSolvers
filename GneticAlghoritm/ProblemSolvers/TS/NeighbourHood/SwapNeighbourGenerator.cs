using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.ProblemSolvers.Mutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.TS.NeighbourHood;

public class SwapNeighbourGenerator : INeighbourGenerator
{
    private IMutationStrategy mutator;

    public SwapNeighbourGenerator(int swapsCount)
    {
        mutator = new SwapCountMutation(swapsCount);
    }

    public PermutationIndividual[] GetNeighbours(int count, PermutationIndividual baseIndividual)
    {
        PermutationIndividual[] neighbours = new PermutationIndividual[count];

        for (int i = 0; i < count; i++)
        {
            var neightbour = new PermutationIndividual(baseIndividual);
            mutator.Mutate(neightbour);
            neighbours[i] = neightbour;
        }

        return neighbours;
    }

    public override string ToString()
    {
        return "swap";
    }
}


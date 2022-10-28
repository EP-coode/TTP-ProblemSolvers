using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Mutation;
using GneticAlghoritm.GA.Mutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.TS;

public class SwapNeighbourGenerator : INeighbourGenerator
{
    private IMutationStrategy mutator;

    public SwapNeighbourGenerator(int swapsCount)
    {
        mutator = new SwapCountMutation(swapsCount);
    }

    public Individual[] GetNeighbours(int count, Individual baseIndividual)
    {
        Individual[] neighbours = new Individual[count];

        for (int i = 0; i < count; i++)
        {
            var neightbour = new Individual(baseIndividual);
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


using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Mutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.TS;

internal class InverseGenerator : INeighbourGenerator
{
    private IMutationStrategy mutation = new InverseMutation();
    public Individual[] GetNeighbours(int count, Individual baseIndividual)
    {
        Individual[] neighbours = new Individual[count];

        for(int i = 0; i < count; i++)
        {
            Individual neighbour = new Individual(baseIndividual);
            mutation.Mutate(neighbour);
            neighbours[i] = neighbour;
        }

        return neighbours;
    }

    public override string ToString()
    {
        return "inverse";
    }
}


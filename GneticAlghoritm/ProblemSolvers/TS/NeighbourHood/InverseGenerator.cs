using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Mutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.TS.NeighbourHood;

internal class InverseGenerator : INeighbourGenerator
{
    private IMutationStrategy mutation = new InverseMutation();
    public PermutationIndividual[] GetNeighbours(int count, PermutationIndividual baseIndividual)
    {
        PermutationIndividual[] neighbours = new PermutationIndividual[count];

        for (int i = 0; i < count; i++)
        {
            PermutationIndividual neighbour = new PermutationIndividual(baseIndividual);
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


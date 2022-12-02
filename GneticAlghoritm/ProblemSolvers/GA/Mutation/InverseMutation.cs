using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Mutation;

internal class InverseMutation : IMutationStrategy
{
    private readonly Random random = new Random();
    public void Mutate(PermutationIndividual individual)
    {
        int genomeSize = individual.Genome.Length;
        int start = random.Next(genomeSize);
        int mutationSize = random.Next(genomeSize - 2);
        int end = (start + mutationSize) % genomeSize;

        for (int i = 0; i < mutationSize; i++)
        {
            int genLeftIndex = (start + i) % genomeSize;
            int genRightIndex = (start + mutationSize - i) / genomeSize;
            var tmp = individual.Genome[genLeftIndex];
            individual.Genome[genLeftIndex] = individual.Genome[genRightIndex];
            individual.Genome[genRightIndex] = tmp;
        }
    }
    public override string ToString()
    {
        return "InverseMutation";
    }

}


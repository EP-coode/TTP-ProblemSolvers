using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Crossing;

internal class OrderedCrossover : ICrossingStrategy
{
    private Random random = new Random();

    public PermutationIndividual[] Cross(PermutationIndividual parent1, PermutationIndividual parent2)
    {
        int genomeSize = parent1.Genome.Length;
        int start = random.Next(0, genomeSize);
        int end = (start + random.Next(0, genomeSize)) % genomeSize;

        if (start > end)
        {
            int tmp = end;
            end = start;
            start = tmp;
        }

        int[] offspring1Genome = new int[genomeSize];
        int[] offspring2Genome = new int[genomeSize];

        for (int i = 0; i < genomeSize; i++)
        {
            if (i < start || i > end)
            {
                offspring1Genome[i] = parent1.Genome[i];
                offspring2Genome[i] = parent2.Genome[i];
            }
            else
            {
                offspring1Genome[i] = parent2.Genome[i];
                offspring2Genome[i] = parent1.Genome[i];
            }
        }

        return new PermutationIndividual[] { new PermutationIndividual(offspring1Genome, parent1.Evaluator), new PermutationIndividual(offspring2Genome, parent1.Evaluator) };
    }

    public override string ToString()
    {
        return "OrderedCrossover";
    }
}


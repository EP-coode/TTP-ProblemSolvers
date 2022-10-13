using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlghoritm.GA.Crossing;

internal class OrderedCrossover : ICrossingStrategy
{
    private Random random = new Random();

    public Individual[] Cross(Individual parent1, Individual parent2)
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

        return new Individual[] { new Individual(offspring1Genome, parent1.Evaluator), new Individual(offspring2Genome, parent1.Evaluator) };
    }
}


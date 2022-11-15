using GeneticAlghoritm.GA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GneticAlghoritm.GA.Mutation;

public class SwapCountMutation : GeneticAlghoritm.GA.Mutation.IMutationStrategy
{
    private Random random = new Random();
    private int swapsCount;


    public SwapCountMutation(int swapsCount)
    {
        this.swapsCount = swapsCount;
    }

    public void Mutate(Individual individual)
    {
        var genome = individual.Genome;

        for (int i = 0; i < swapsCount; i++)
        {
            int indexToSwap = random.Next(0, genome.Length - 1) % genome.Length;
            int indexToSwap2 = (random.Next(0, genome.Length - 2) + indexToSwap) % genome.Length;
            Swap(indexToSwap, indexToSwap2, genome);
        }
    }

    private void Swap(int x, int y, int[] array)
    {
        var temp = array[x];
        array[x] = array[y];
        array[y] = temp;
    }

    public override string ToString()
    {
        return $"SwapCountMutation({swapsCount})";
    }
}


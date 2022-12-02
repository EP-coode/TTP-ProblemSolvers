using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Mutation;
internal class SwapMutation : IMutationStrategy
{
    private Random random = new Random();
    private double mutationStrangthTreshold;


    public SwapMutation(double mutationStrangthTreshold)
    {
        this.mutationStrangthTreshold = mutationStrangthTreshold;
    }

    public void Mutate(PermutationIndividual individual)
    {
        var genome = individual.Genome;

        for (int i = 0; i < genome.Length; i++)
        {
            bool canMutate = random.NextDouble() < mutationStrangthTreshold / 2;
            if (canMutate)
            {
                int indexToSwap = (random.Next(0, genome.Length - 1) + i) % genome.Length;
                Swap(i, indexToSwap, genome);
            }
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
        return $"SwapMutation({mutationStrangthTreshold})";
    }
}


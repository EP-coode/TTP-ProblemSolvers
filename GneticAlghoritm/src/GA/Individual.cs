using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA;

internal class Individual
{
    public int[] Genome { get; }

    public Individual(int[] dimensions)
    {
        Genome = new int[dimensions.Length];
        Array.Copy(dimensions, Genome, dimensions.Length);
        Shuffle(Genome);
    }

    public void Mutate(float mutationStrangthTreshold)
    {
        Random rand = new Random();

        for (int i = 0; i < Genome.Length; i++)
        {
            bool canMutate = rand.NextDouble() < mutationStrangthTreshold / 2;
            if (canMutate)
            {
                int indexToSwap = (rand.Next(0, Genome.Length - 1) + i) % Genome.Length;
                Swap(i, indexToSwap, Genome);
            }
        }
    }

    private void Shuffle(int[] array)
    {
        Random rand = new Random();

        for (int i = 0; i < array.Length; i++)
        {
            int indexToSwap = (rand.Next(0, array.Length - 1) + i) % array.Length;
            Swap(i, indexToSwap, array);
        }
    }

    private void Swap(int x, int y, int[] array)
    {
        var temp = array[x];
        array[x] = array[y];
        array[y] = temp;
    }

    public Individual CrossOver(Individual otherGenome)
    {
        throw new NotImplementedException();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticAlghoritm.GA.Evaluation;

namespace GeneticAlghoritm.GA;

public class Individual
{
    public int[] Genome { get; }
    private double? _value;
    public double? Value
    {
        get
        {
            if (_value is null)
            {
                double evaluation = Evaluator.Evaluate(this);
                _value = evaluation;
                return _value;
            }

            return _value;

        }
        private set { _value = value; }
    }

    private IEvaluator _evaluator;
    public IEvaluator Evaluator
    {
        get { return _evaluator; }
        set
        {
            _evaluator = value;
            Value = null;
        }
    }

    public Individual(int[] genPool, IEvaluator evaluator, bool shuffleGenome = true)
    {
        Genome = new int[genPool.Length];
        Array.Copy(genPool, Genome, genPool.Length);
        Evaluator = evaluator;

        if (shuffleGenome)
            Shuffle(Genome);
    }

    public Individual(Individual individual)
    {
        Genome = new int[individual.Genome.Length];
        Evaluator = individual.Evaluator;
        Array.Copy(individual.Genome, Genome, individual.Genome.Length);
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

    public void Randomize()
    {
        Shuffle(Genome);
    }

    private void Shuffle(int[] array)
    {
        Random rand = new Random();

        for (int i = 0; i < array.Length; i++)
        {
            int indexToSwap = (rand.Next(0, array.Length - 1) + i) % array.Length;
            Swap(i, indexToSwap, array);
        }
        Value = null;
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


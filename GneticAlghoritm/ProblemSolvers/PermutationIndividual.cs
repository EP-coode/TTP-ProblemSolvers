using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProblemSolvers.ProblemSolvers.Evaluation;

namespace ProblemSolvers.ProblemSolvers;

public class PermutationIndividual : IEquatable<PermutationIndividual>, IComparable<PermutationIndividual>
{
    public int[] Genome { get; }
    private double _value = double.NaN;
    public double Value
    {
        get
        {
            if (double.IsNaN(_value))
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
            Value = double.NaN;
        }
    }

    public PermutationIndividual(int[] genPool, IEvaluator evaluator, bool shuffleGenome = true)
    {
        Genome = new int[genPool.Length];
        Array.Copy(genPool, Genome, genPool.Length);
        Evaluator = evaluator;

        if (shuffleGenome)
            Shuffle(Genome);
    }

    public PermutationIndividual(int[] genome, IEvaluator evaluator)
    {
        this.Evaluator = evaluator;
        Genome = genome;
    }

    public PermutationIndividual(PermutationIndividual individual)
    {
        Genome = new int[individual.Genome.Length];
        Evaluator = individual.Evaluator;
        Array.Copy(individual.Genome, Genome, individual.Genome.Length);
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
        Value = double.NaN;
    }

    private void Swap(int x, int y, int[] array)
    {
        var temp = array[x];
        array[x] = array[y];
        array[y] = temp;
    }

    public bool Equals(PermutationIndividual? other)
    {
        if (other == null)
            return false;

        if (other.Genome.Length != Genome.Length)
            return false;

        for(int i = 0; i < Genome.Length; i++)
        {
            if(Genome[i] != other.Genome[i])
                return false;
        }

        return true;
    }

    public int CompareTo(PermutationIndividual? other)
    {
        if(other == null)   
            return 1;

        return other.Value.CompareTo(Value);
    }
}


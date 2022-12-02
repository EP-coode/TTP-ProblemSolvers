using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.GA.Selection;

public class Roulette : ISelector
{
    private Random random = new Random();
    private int pow;

    public Roulette(int pow = 1)
    {
        this.pow = pow;
    }

    private double PrcessValue(double value, int pow)
    {
        return (value > 0 ? 1 : -1) * Math.Abs(Math.Pow(value, pow));
    }

    public PermutationIndividual SelectParent(PermutationIndividual[] population)
    {
        double currentBreakPoint = 0;
        double lastValue = PrcessValue(population[0].Value, pow);
        double[] breakPoints = new double[population.Length];

        for (int i = 1; i < population.Length; i++)
        {
            double individualValue = PrcessValue(population[i].Value, pow);
            double diff = Math.Abs(individualValue - lastValue);
            lastValue = individualValue;
            currentBreakPoint += diff;
            breakPoints[i] = currentBreakPoint;
        }

        double roulettePointerStop = random.NextDouble() * currentBreakPoint;

        int slectedItemIndex = 0;

        for (int i = 0; i < breakPoints.Length; i++)
        {
            if (breakPoints[i] > roulettePointerStop)
                break;

            slectedItemIndex++;
        }

        return population[slectedItemIndex];
    }

    public override string ToString()
    {
        return $"Roulette{pow}";
    }
}


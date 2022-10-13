using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlghoritm.GA.Selection;

public class Roulette : ISelector
{
    private Random random = new Random();

    public Individual SelectParent(Individual[] population)
    {
        double currentBreakPoint = 0;
        double lastValue = population[0].Value;
        double[] breakPoints = new double[population.Length];

        for (int i = 1; i < population.Length; i++)
        {
            double individualValue = population[i].Value;
            double diff = Math.Abs(individualValue - lastValue);
            currentBreakPoint += diff;
            breakPoints[i] = currentBreakPoint;
        }

        double? roulettePointerStop = random.NextDouble()*currentBreakPoint;

        int slectedItemIndex = 0;

        for (int i = 0; i < breakPoints.Length; i++)
        {
            if (breakPoints[i] > roulettePointerStop)
                break;

            slectedItemIndex++;
        }

        return population[slectedItemIndex];
    }
}


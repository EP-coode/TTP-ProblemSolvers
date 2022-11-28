using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.SA;

public class LinearCooling : ICoolingStrategy
{
    private double CoolingStep { get; init; }

    public LinearCooling(double coolingStep)
    {
        CoolingStep = coolingStep;
    }

    public double GetTemperature(int generation, double initialTemperature)
    {
        return initialTemperature - CoolingStep * generation;
    }

    public override string ToString()
    {
        return $"{CoolingStep}"; ;
    }
}


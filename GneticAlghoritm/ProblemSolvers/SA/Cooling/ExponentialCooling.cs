using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.SA.Cooling;

internal class ExponentialCooling : ICoolingStrategy
{
    private double CoolingTreshold { get; set; }

    public ExponentialCooling(double coolingTreshold)
    {
        CoolingTreshold = coolingTreshold;
    }

    public double GetTemperature(int generation, double initialTemperature)
    {
        return initialTemperature * Math.Pow(CoolingTreshold, generation);
    }

    public override string ToString()
    {
        return $"{CoolingTreshold}";
    }
}


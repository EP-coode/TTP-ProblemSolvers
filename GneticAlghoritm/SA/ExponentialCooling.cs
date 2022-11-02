using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.SA;

internal class ExponentialCooling : ICoolingStrategy
{
    private double CoolingTreshold { get; set; }

    public ExponentialCooling(double coolingTreshold)
    {
        CoolingTreshold = coolingTreshold;
    }

    public double CoolDown(double temperature)
    {
        return temperature * (1 - CoolingTreshold);
    }

    public override string ToString()
    {
        return "Exponential";
    }
}


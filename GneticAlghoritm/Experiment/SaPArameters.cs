using GneticAlghoritm.SA;
using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public record SaParams
{
    public ICoolingStrategy CoolingStrategy { get; init; }
    public double InitialTemperature { get; init; }
    public double MinTemperature { get; init; }
    public INeighbourGenerator neighbourGenerator { get; init; }
}


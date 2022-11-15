using GneticAlghoritm.SA;
using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public record SaParameterSpace
{
    public IEnumerable<ICoolingStrategy> CoolingStrategy { get; init; }
    public IEnumerable<double> InitialTemperature { get; init; }
    public IEnumerable<double> MinTemperature { get; init; }
    public IEnumerable<INeighbourGenerator> neighbourGenerator { get; init; }
}


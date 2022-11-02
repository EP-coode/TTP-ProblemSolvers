using GneticAlghoritm.SA;
using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public class SA_Experiment
{
    public string ExperimentName { get; init; }
    public INeighbourGenerator NeighbourGenerator { get; init; }
    public ICoolingStrategy CoolingStrategy { get; init; }
    public int NeightbourhoodSize { get; init; }
    public double InitialTemperature { get; init; }
    public double MinTemperature { get; init; }
    public string dataSet { get; init; }
    public int repeats { get; init; }
}


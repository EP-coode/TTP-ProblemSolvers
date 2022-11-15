using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public record TsParametersSpace
{
    public IEnumerable<int> neightbourhoodSize { get; init; }
    public IEnumerable<int> tabuSize { get; init; }
    public IEnumerable<INeighbourGenerator> neighbourGenerator { get; init; }
}


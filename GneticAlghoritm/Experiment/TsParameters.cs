using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public record TsParameters
{
    public int neightbourhoodSize { get; init; }
    public int tabuSize { get; init; }
    public INeighbourGenerator neighbourGenerator { get; init; }
}


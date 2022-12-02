using ProblemSolvers.ProblemSolvers.TS.NeighbourHood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.TS;

public record TsParams : ProblemSolverParams
{
    public TsParams(int neightbourhoodSize,
        int tabuSize, INeighbourGenerator generator,
        ProblemSolverParams problemSolverParams)
        : base(problemSolverParams)
    {
        PopulationSize = 0;
        NeightbourhoodSize = neightbourhoodSize;
        TabuSize = tabuSize;
        this.NeighbourGenerator = generator;
    }

    public int NeightbourhoodSize { get; init; }
    public int TabuSize { get; init; }
    public INeighbourGenerator? NeighbourGenerator { get; init; }
}


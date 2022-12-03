using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers.SA.Cooling;
using ProblemSolvers.ProblemSolvers.TS.NeighbourHood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.SA;

public record SaParams : ProblemSolverParams
{
    public ICoolingStrategy CoolingStrategy { get; init; }
    public double InitialTemperature { get; init; }
    public double MinTemperature { get; init; }
    public INeighbourGenerator neighbourGenerator { get; init; }

    public SaParams(ICoolingStrategy coolingStrategy,
                double initialTemperature,
                double minTemperature,
                INeighbourGenerator neighbourGenerator) : base(0)
    {
        CoolingStrategy = coolingStrategy;
        InitialTemperature = initialTemperature;
        MinTemperature = minTemperature;
        this.neighbourGenerator = neighbourGenerator;
    }

    public SaParams(ICoolingStrategy coolingStrategy,
               double initialTemperature,
               double minTemperature,
               INeighbourGenerator neighbourGenerator,
               ProblemSolverParams solverParams) : base(solverParams)
    {
        PopulationSize = 0;
        CoolingStrategy = coolingStrategy;
        InitialTemperature = initialTemperature;
        MinTemperature = minTemperature;
        this.neighbourGenerator = neighbourGenerator;
    }
}


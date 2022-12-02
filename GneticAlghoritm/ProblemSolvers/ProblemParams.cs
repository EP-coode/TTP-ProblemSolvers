using ProblemSolvers.ProblemSolvers.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers;

public record ProblemSolverParams
{
    public int PopulationSize { get; init; }

    public ProblemSolverParams(int populationSize)
    {
        PopulationSize = populationSize;
    }
}

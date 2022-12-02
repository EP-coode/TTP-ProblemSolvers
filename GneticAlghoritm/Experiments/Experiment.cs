using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.ProblemSolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.Experiments;

public record Experiment<T> where T : ProblemSolverParams
{
    public string Name { get; private set; }
    public string DstPathName { get; private set; }
    public int Repeats { get; private set; }
    public int MaxIterations { get; private set; }
    public T ProblemSolverParams { get; private set; }

    public Experiment(string name, string dstPathName, int repeats, int maxIterations, T problemSolverParams)
    {
        Name = name;
        DstPathName = dstPathName;
        Repeats = repeats;
        MaxIterations = maxIterations;
        ProblemSolverParams = problemSolverParams;
    }
}


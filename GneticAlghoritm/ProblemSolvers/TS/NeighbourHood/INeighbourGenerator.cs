using ProblemSolvers.ProblemSolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.TS.NeighbourHood;

public interface INeighbourGenerator
{
    public PermutationIndividual[] GetNeighbours(int count, PermutationIndividual baseIndividual);
}


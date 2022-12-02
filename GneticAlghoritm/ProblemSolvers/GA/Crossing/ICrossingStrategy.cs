using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Crossing;

public interface ICrossingStrategy
{
    public PermutationIndividual[] Cross(PermutationIndividual parent1, PermutationIndividual parent2);
}


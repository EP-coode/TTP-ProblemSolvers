using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.GA.Selection;

public interface ISelector
{
    public PermutationIndividual SelectParent(PermutationIndividual[] population);
}


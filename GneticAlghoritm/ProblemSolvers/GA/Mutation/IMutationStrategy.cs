using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Mutation;

public interface IMutationStrategy
{
    public void Mutate(PermutationIndividual individual);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Evaluation;

public interface IEvaluator
{
    public double Evaluate(PermutationIndividual individual);
}


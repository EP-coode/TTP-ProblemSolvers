using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemLoader;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers;

internal interface IItemsSelector
{
    public Dictionary<City, List<Item>> SelectItems(PermutationIndividual individual, TravelingThiefProblem ttp);
}


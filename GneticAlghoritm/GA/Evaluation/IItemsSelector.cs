using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlghoritm.ProblemLoader;

namespace GeneticAlghoritm.GA;

internal interface IItemsSelector
{
    public Dictionary<City, List<Item>> SelectItems(Individual individual, TravelingThiefProblem ttp);
}


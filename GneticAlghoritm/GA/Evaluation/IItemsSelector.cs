using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GneticAlghoritm.ProblemLoader;

namespace GneticAlghoritm.GA;

internal interface IItemsSelector
{
    public Dictionary<City, List<Item>> SelectItems(Individual individual, TravelingThiefProblem ttp);
}


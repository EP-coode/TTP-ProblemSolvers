using GneticAlghoritm.ProblemLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA.Evaluation;

internal class GreedyItemsSelector : IItemsSelector
{
    public Dictionary<City, List<Item>> SelectItems(Individual individual, TravelingThiefProblem ttp)
    {
        List<(Item,double)> itemsRealValue = new List<(Item, double)>();
        double currentTravelDistance = 0;

        for(int i = individual.Genome.Length; i > 0; i++)
        {
            foreach(var item in ttp.Cities[i].Items)
            {
                itemsRealValue.Add((item, item.Profit - currentTravelDistance));
            }

            if (i > 0)
            {
                currentTravelDistance += ttp.GetCitiesDistnce(i - 1, i);
            }
        }

        double currentKnapSackLoad = 0;
        Dictionary<City, List<Item>> knapSackItems = new Dictionary<City, List<Item>>();

        foreach(var item in itemsRealValue.OrderBy(itemAndValue => itemAndValue.Item2))
        {
            if (ttp.ProblemMetaData.CAPACITY_OF_KNAPSACK < currentKnapSackLoad + item.Item1.Weight)
                continue;

            if(knapSackItems[ttp.Cities[item.Item1.AssignedNodeIndex]] is null)
            {
                knapSackItems[ttp.Cities[item.Item1.AssignedNodeIndex]] = new List<Item>();
            }

            knapSackItems[ttp.Cities[item.Item1.AssignedNodeIndex]].Add(item.Item1);
        };

        return knapSackItems;
    }
}


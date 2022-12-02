using ProblemSolvers.ProblemLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.Evaluation;

internal class GreedyItemsSelector : IItemsSelector
{
    public Dictionary<City, List<Item>> SelectItems(PermutationIndividual individual, TravelingThiefProblem ttp)
    {
        List<(Item, double)> itemsRealValue = new List<(Item, double)>();
        double currentTravelDistance = 0;
        int lastCityIndex = individual.Genome[individual.Genome.Length - 1];
        // calc real items value
        foreach (int cityIndex in individual.Genome.Reverse())
        {
            foreach (var item in ttp.Cities[cityIndex].Items)
            {
                itemsRealValue.Add((item, item.Profit - currentTravelDistance));
            }

            currentTravelDistance += ttp.GetCitiesDistnce(lastCityIndex, cityIndex);
            lastCityIndex = cityIndex;

        }

        double currentKnapSackLoad = 0;
        Dictionary<City, List<Item>> knapSackItems = new Dictionary<City, List<Item>>();

        var itemsOrderedByRealValue = itemsRealValue.OrderBy(itemAndValue => itemAndValue.Item2 * -1);

        // select best items and join them with coresponding cities
        foreach (var item in itemsOrderedByRealValue)
        {
            if (ttp.ProblemMetaData.CAPACITY_OF_KNAPSACK < currentKnapSackLoad + item.Item1.Weight)
                continue;

            if (!knapSackItems.ContainsKey(ttp.Cities[item.Item1.AssignedNodeIndex]))
            {
                knapSackItems[ttp.Cities[item.Item1.AssignedNodeIndex]] = new List<Item>();
            }

            knapSackItems[ttp.Cities[item.Item1.AssignedNodeIndex]].Add(item.Item1);
            currentKnapSackLoad += item.Item1.Weight;
        };

        return knapSackItems;
    }
}


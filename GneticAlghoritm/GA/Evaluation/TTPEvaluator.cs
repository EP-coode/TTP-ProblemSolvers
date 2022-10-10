using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GneticAlghoritm.ProblemLoader;

namespace GneticAlghoritm.GA.Evaluation;

internal class TTPEvaluator : IEvaluator
{
    public TravelingThiefProblem ttp { get; init; }
    public IItemsSelector itemsSelector { get; init; }

    public double Evaluate(Individual individual)
    {
        var selectedItems = itemsSelector.SelectItems(individual, ttp);
        double totalItemsValue = selectedItems
            .Select(kvp => kvp.Value)
            .Sum(items => items.Sum(item => item.Profit));
        double travelCost = 0;
        double currentKnapSackWeight = 0;

        for (int i = 1; i < ttp.Cities.Length; i++)
        {
            City srcCity = ttp.Cities[i - 1];
            City dstCity = ttp.Cities[i];
            currentKnapSackWeight += selectedItems[srcCity].Sum(item => item.Weight);
            double travelSpeed = GetTravelSpeed(currentKnapSackWeight);
            double distance = ttp.GetCitiesDistnce(i - 1, i);
            double travelTime = distance / travelSpeed;
            travelCost += travelTime;
        }

        return totalItemsValue - travelCost;
    }

    private double GetTravelSpeed(double currentKnapsackWeight)
    {
        return ttp.ProblemMetaData.MAX_SPEED -
            ((ttp.ProblemMetaData.MAX_SPEED - ttp.ProblemMetaData.MIN_SPEED) * currentKnapsackWeight)
                / ttp.ProblemMetaData.CAPACITY_OF_KNAPSACK;
    }


}


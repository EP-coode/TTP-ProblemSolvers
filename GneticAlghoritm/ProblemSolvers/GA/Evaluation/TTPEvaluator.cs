using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemLoader;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.Evaluation;

internal class TTPEvaluator : IEvaluator
{
    private TravelingThiefProblem Ttp { get; init; }
    public IItemsSelector ItemsSelector { get; init; }

    public TTPEvaluator(TravelingThiefProblem ttp, IItemsSelector itemsSelector)
    {
        Ttp = ttp;
        ItemsSelector = itemsSelector;
    }

    public double Evaluate(PermutationIndividual individual)
    {
        var selectedItems = ItemsSelector.SelectItems(individual, Ttp);
        double totalItemsValue = selectedItems
            .Select(kvp => kvp.Value)
            .Sum(items => items.Sum(item => item.Profit));
        double travelCost = 0;
        double currentKnapSackWeight = 0;

        for (int i = 1; i < Ttp.Cities.Length; i++)
        {
            City srcCity = Ttp.Cities[i - 1];
            City dstCity = Ttp.Cities[i];
            double distance = Ttp.GetCitiesDistnce(i - 1, i);

            if (selectedItems.ContainsKey(srcCity))
            {
                currentKnapSackWeight += selectedItems[srcCity].Sum(item => item.Weight);
            }

            double travelSpeed = GetTravelSpeed(currentKnapSackWeight);
            double travelTime = distance / travelSpeed;
            travelCost += travelTime;
        }

        return totalItemsValue - travelCost;
    }

    private double GetTravelSpeed(double currentKnapsackWeight)
    {
        return Ttp.ProblemMetaData.MAX_SPEED -
            ((Ttp.ProblemMetaData.MAX_SPEED - Ttp.ProblemMetaData.MIN_SPEED)
            * currentKnapsackWeight) / Ttp.ProblemMetaData.CAPACITY_OF_KNAPSACK;
    }


}


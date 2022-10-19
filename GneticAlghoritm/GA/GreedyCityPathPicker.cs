using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.Logger;
using GeneticAlghoritm.ProblemLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA;

internal class GreedyCityPathPicker
{
    public static Individual? GetBestIndividual(CsvFileLogger logger, TravelingThiefProblem ttp)
    {
        // very bad
        GreedyItemsSelector itemsSelector = new GreedyItemsSelector();
        Individual? bstIndividual = null;
        IEvaluator ev = new TTPEvaluator(ttp, itemsSelector);

        for (int i = 0; i < ttp.Cities.Length; i++)
        {
            int[] citiesOrder = new int[ttp.Cities.Length];
            citiesOrder[0] = i;
            var startingCity = ttp.Cities[i];
            var currentCity = startingCity;
            var unvisitedCities = ttp.Cities.Where((city) => city != startingCity).ToList();

            for (int j = 1; j < ttp.Cities.Length; j++)
            {
                var cityDistances = unvisitedCities.Select((city) => (city, TravelingThiefProblem.GetCitiesDistnce(city, currentCity))).OrderBy((i)=>i.Item2).ToArray();
                var closestCity = unvisitedCities.MinBy((city) => TravelingThiefProblem.GetCitiesDistnce(city, currentCity));
                citiesOrder[j] = closestCity.Index;
                unvisitedCities.Remove(closestCity);
                currentCity = closestCity;
            }

            // very bad
            bstIndividual = new Individual(citiesOrder, ev);
            logger.Log(new string[] { bstIndividual.Value.ToString() });
        }

        return bstIndividual;
    }
}


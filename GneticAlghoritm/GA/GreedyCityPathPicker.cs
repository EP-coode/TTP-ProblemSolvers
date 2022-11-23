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
    public static Individual? GetBestIndividual(CsvFileLogger? logger, IEvaluator ev, City[] cities)
    {
        // very bad
        GreedyItemsSelector itemsSelector = new GreedyItemsSelector();
        Individual init = new Individual(cities.Select(c => c.Index).ToArray(), ev, false);
        Individual? bestIndividual = init;

        // dla każdego miasta jako początkowe
        for (int i = 0; i < 1; i++)
        {
            Individual? currentIndividual = null;
            // ustaw jako początkowe 
            int[] citiesOrder = new int[cities.Length];
            citiesOrder[0] = i;
            var startingCity = cities[i];
            var currentCity = startingCity;

            // określ nieodwiedzone miasta
            var unvisitedCities = cities.Where((city) => city != startingCity).ToList();

            // dla dopuki isnieją nieodwiedzone miasta odwiedz je 
            for (int j = 1; j < cities.Length; j++)
            {
                // oblicz wszyskie odległości pozostałych miast od obecnego
                //var cityDistances = unvisitedCities.Select((city) => (city, TravelingThiefProblem.GetCitiesDistnce(city, currentCity))).OrderBy((i)=>i.Item2).ToArray();

                // określ najbliższe miasto 
                var closestCity = unvisitedCities.MinBy((city) => TravelingThiefProblem.GetCitiesDistnce(city, currentCity));

                citiesOrder[j] = closestCity.Index;
                unvisitedCities.Remove(closestCity);
                currentCity = closestCity;
            }

            // very bad
            currentIndividual = new Individual(citiesOrder, ev, shuffleGenome: false);

            if (bestIndividual is null || bestIndividual.Value < currentIndividual.Value)
            {
                bestIndividual = currentIndividual;
            }

            logger?.Log(new string[] { currentIndividual.Value.ToString() });
        }

        return bestIndividual;
    }
}


using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.Logger;
using ProblemSolvers.ProblemLoader;
using ProblemSolvers.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.Greedy;

internal class GreedyCityPathPicker
{
    public static PermutationIndividual? GetBestIndividual(ILogger<SingleValue>? logger, IEvaluator ev, City[] cities)
    {
        // very bad
        GreedyItemsSelector itemsSelector = new GreedyItemsSelector();
        PermutationIndividual init = new PermutationIndividual(cities.Select(c => c.Index).ToArray(), ev, false);
        PermutationIndividual? bestIndividual = init;

        // dla każdego miasta jako początkowe
        for (int i = 0; i < 1; i++)
        {
            PermutationIndividual? currentIndividual = null;
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
            currentIndividual = new PermutationIndividual(citiesOrder, ev, shuffleGenome: false);

            if (bestIndividual is null || bestIndividual.Value < currentIndividual.Value)
            {
                bestIndividual = currentIndividual;
            }

            logger?.Log(new SingleValue(currentIndividual.Value));
        }

        return bestIndividual;
    }
}


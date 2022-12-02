using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.ProblemSolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.Crossing;

internal class CycleCrossover : ICrossingStrategy
{
    private readonly Random random = new Random();
    public PermutationIndividual[] Cross(PermutationIndividual parent1, PermutationIndividual parent2)
    {
        int[] parent1CityIndexes = new int[parent1.Genome.Length];

        for (int i = 0; i < parent1.Genome.Length; i++)
        {
            parent1CityIndexes[parent1.Genome[i]] = i;
        }

        HashSet<int> visitedCities = new HashSet<int>();

        int startingIndex = 0;
        int startingCity = parent1.Genome[startingIndex];
        int endingCity = parent2.Genome[startingIndex];
        visitedCities.Add(startingCity);

        for (int i = 1; i < parent2.Genome.Length; i++)
        {
            int nextStartingCityIndex = parent1CityIndexes[endingCity];
            startingCity = parent1.Genome[nextStartingCityIndex];
            endingCity = parent2.Genome[nextStartingCityIndex];

            if (visitedCities.Contains(startingCity))
            {
                break;
            }

            visitedCities.Add(startingCity);
        }

        int[] offspringGenome = new int[parent1.Genome.Length];

        for (int i = 0; i < parent1.Genome.Length; i++)
        {
            if (visitedCities.Contains(i))
            {
                offspringGenome[i] = parent1.Genome[i];
            }
            else
            {
                offspringGenome[i] = parent2.Genome[i];
            }
        }

        return new PermutationIndividual[] { new PermutationIndividual(offspringGenome, parent1.Evaluator) };

    }

    public override string ToString()
    {
        return "CycleCrossover";
    }
}


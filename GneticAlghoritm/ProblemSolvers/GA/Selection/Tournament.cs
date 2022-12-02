using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers;

namespace ProblemSolvers.ProblemSolvers.GA.Selection;

internal class Tournament : ISelector
{
    public double TournamantSize { get; set; }
    private Random random = new Random();

    public Tournament(double tournamentSize)
    {
        TournamantSize = tournamentSize;
    }

    public PermutationIndividual SelectParent(PermutationIndividual[] population)
    {
        int sampleSize = (int)(population.Length * TournamantSize) % population.Length;
        return population
           .OrderBy(individual => random.Next())
           .Take(Math.Max(sampleSize, 1))
           .OrderBy(individual => individual.Value)
           .Reverse()
           .First();
    }

    public override string ToString()
    {
        return $"Tournament{TournamantSize}";
    }
}


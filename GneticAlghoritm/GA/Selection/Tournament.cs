using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlghoritm.GA.Selection;

internal class Tournament : ISelector
{
    public int TournamantSize { get; set; }
    private Random random = new Random();

    public Tournament(int tournamentSize)
    {
        TournamantSize = tournamentSize;
    }

    public Individual SelectParent(Individual[] population)
    {
         return population
            .OrderBy(individual => random.Next())
            .Take(TournamantSize)
            .OrderBy(individual => individual.Value)
            .Reverse()
            .First();
    }
}


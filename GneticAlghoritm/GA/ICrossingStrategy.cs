using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlghoritm.GA;

public interface ICrossingStrategy
{
    public Individual[] Cross(Individual parent1, Individual parent2);
}


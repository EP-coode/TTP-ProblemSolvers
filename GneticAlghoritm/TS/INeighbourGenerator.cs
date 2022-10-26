using GeneticAlghoritm.GA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.TS;

public interface INeighbourGenerator
{
    public Individual[] GetNeighbours(int count, Individual baseIndividual);
}


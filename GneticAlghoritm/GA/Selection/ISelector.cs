using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlghoritm.GA;

namespace GeneticAlghoritm.GA.Selection;

public interface ISelector
{
    public Individual SelectParent(Individual[] population);
}


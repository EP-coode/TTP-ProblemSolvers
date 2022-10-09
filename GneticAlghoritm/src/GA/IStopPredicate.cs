using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA;

internal interface IStopPredicate
{
    public bool MustStop(GeneticAlghoritm ga);
}


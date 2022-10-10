using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA;

public interface IStopPredicate
{
    public bool MustStop(GeneticAlghoritm ga);
}


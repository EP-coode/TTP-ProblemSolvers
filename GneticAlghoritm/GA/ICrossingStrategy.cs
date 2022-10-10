using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.GA;

public interface ICrossingStrategy
{
    public Individual[] Cross(Individual parent1, Individual parent2);
}


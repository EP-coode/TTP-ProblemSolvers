using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GneticAlghoritm.GA;

namespace GneticAlghoritm.GA.Selection;

public interface ISelector
{
    public Individual SelectParent(Individual[] population);
}


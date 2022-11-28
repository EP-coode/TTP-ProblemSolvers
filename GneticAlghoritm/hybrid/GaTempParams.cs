using GneticAlghoritm.SA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.hybrid
{
    public class GaTempParams : Experiment.Experiment
    {
        public ICoolingStrategy Cooling { get; set; }
    }
}

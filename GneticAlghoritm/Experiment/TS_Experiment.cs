using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment
{
    public class TS_Experiment
    {
        public string experimentName { get; init; }
        public string dataSet { get; init; }
        public int neightbourhoodSize { get; init; }
        public int tabuSize { get; init; }
        public int repeats { get; init; }
        public INeighbourGenerator generator { get; init; }
    }
}

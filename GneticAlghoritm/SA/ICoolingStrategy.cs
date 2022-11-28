using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.SA;

public interface ICoolingStrategy
{
    public double GetTemperature(int iteration, double initialTemperature);
}


using ProblemSolvers.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.SA;

public class SaLog : ILogable
{
    public double Best { get; private set; }
    public double Current { get; private set; }
    public double Temperature { get; private set; }

    public SaLog(double best, double current, double temperature)
    {
        Best = best;
        Current = current;
        Temperature = temperature;
    }

    public string GetHeader(char separator)
    {
        return $"Best{separator}Current{separator}Temperature";
    }

    public string ToString(char separator)
    {
        return $"{Best}{separator}{Current}{separator}{Temperature}";
    }
}


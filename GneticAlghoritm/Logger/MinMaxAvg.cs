using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.Logger;

public class MinMaxAvg : ILogable
{
    public double Min { get; init; }
    public double Max { get; init; }
    public double Avg { get; init; }

    public MinMaxAvg(double min, double max, double avg)
    {
        Min = min;
        Max = max;
        Avg = avg;
    }

    public string ToString(char separator)
    {
        return $"{Min}{separator}{Max}{separator}{Avg}";
    }

    public string GetHeader(char separator)
    {
        return $"Min{separator}Max{separator}Avg";
    }

}


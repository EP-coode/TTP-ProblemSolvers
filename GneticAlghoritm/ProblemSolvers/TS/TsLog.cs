using ProblemSolvers.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.ProblemSolvers.TS;

public class TsLog : ILogable
{
    public double Current { get; private set; }
    public double Best { get; private set; }
    public double Min { get; private set; }
    public double Max { get; private set; }
    public double Avg { get; private set; }

    public TsLog(double current, double best, double min, double max, double avg)
    {
        Current = current;
        Best = best;
        Min = min;
        Max = max;
        Avg = avg;
    }

    public string GetHeader(char separator)
    {
        return $"Current{separator}Best{separator}NMin{separator}NMax{separator}NAvg";
    }

    public string ToString(char separator)
    {
        return $"{Current}{separator}{Best}{separator}{Min}{separator}{Max}{separator}{Avg}";
    }
}


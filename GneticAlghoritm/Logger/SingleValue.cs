using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.Logger;

public class SingleValue : ILogable
{
    public double Value { get; private set; }

    public SingleValue(double v)
    {
        this.Value = v;
    }

    public string GetHeader(char separator)
    {
        return "Value";
    }

    public string ToString(char separator)
    {
        return Value.ToString();
    }
}


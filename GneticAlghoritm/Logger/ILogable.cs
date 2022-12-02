using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.Logger;

public interface ILogable
{
    public string ToString(char separator);
    public string GetHeader(char separator);
}


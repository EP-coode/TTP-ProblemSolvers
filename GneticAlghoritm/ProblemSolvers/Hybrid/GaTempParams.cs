using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.GA;
using ProblemSolvers.ProblemSolvers.SA.Cooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.hybrid
{
    public record GaTempParams : GaParams
    {
        protected GaTempParams(GaParams original, ICoolingStrategy cooling) : base(original)
        {
            Cooling = cooling;
        }

        public ICoolingStrategy Cooling { get; private set; }
    }
}

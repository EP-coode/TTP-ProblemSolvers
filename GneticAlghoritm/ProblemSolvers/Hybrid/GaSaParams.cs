using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.ProblemSolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvers.ProblemSolvers.GA;

namespace ProblemSolvers.hybrid
{
    public record GaSaParams : GaParams
    {
        protected GaSaParams(GaParams original, int saGroupSize, int saRunFrequency, int saIterationsLimit) : base(original)
        {
            this.saGroupSize = saGroupSize;
            this.saRunFrequency = saRunFrequency;
            this.saIterationsLimit = saIterationsLimit;
        }

        public int saGroupSize {get;set;}
        public int saRunFrequency {get;set;}
        public int saIterationsLimit { get; set; } 
    }
}

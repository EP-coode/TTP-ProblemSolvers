using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.ProblemSolvers.SA.Cooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.hybrid
{

    public class GaTe : ProblemSolvers.GA.GeneticAlghoritm
    {
        public ICoolingStrategy Cooling { init; get; }
        private double initialMutationFrequency;

        public GaTe(GaTempParams gaTeParams, int[] populationGenes, IEvaluator evaluator)
            : base(gaTeParams, populationGenes, evaluator)
        {
            Cooling = gaTeParams.Cooling;
            this.initialMutationFrequency = gaTeParams.MutationFrequencyTreshold;
        }

        public override void Run(int generations)
        {
            for (int i = 0; i < generations; i++)
            {
                NextGeneration(i);
                SaveStatsToLog();
                base.mutationFrequencyTreshold = Cooling.GetTemperature(i, initialMutationFrequency);
            }
        }

    }
}

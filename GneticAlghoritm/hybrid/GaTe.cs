using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using GneticAlghoritm.SA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.hybrid
{

    public class GaTe : GeneticAlghoritm.GA.GeneticAlghoritm
    {
        public ICoolingStrategy Cooling { init; get; }
        private double initialMutationFrequency;

        public GaTe(int[] populationGenes, int populationSize, double initialMutationFrequency, ICoolingStrategy colling, double crossingFrequency, IEvaluator evaluator, IMutationStrategy mutationStrategy, ICrossingStrategy crossingStrategy, ISelector parentSelector)
            : base(populationGenes, populationSize, initialMutationFrequency, crossingFrequency, evaluator, mutationStrategy, crossingStrategy, parentSelector)
        {
            Cooling = colling;
            this.initialMutationFrequency = initialMutationFrequency;
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

using GeneticAlghoritm.GA;
using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Evaluation;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using GneticAlghoritm.SA;
using GneticAlghoritm.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.hybrid
{
    internal class GaSa : GeneticAlghoritm.GA.GeneticAlghoritm
    {
        private readonly Random r = new Random();
        private int saGroupSize;
        private int saRunFrequency;
        private int saIterationsLimit;

        public GaSa(int[] populationGenes, IEvaluator evaluator, GaSaParams gaSaParams)
            : base(populationGenes, gaSaParams.PopSize, gaSaParams.MutationTreshold, gaSaParams.CrossingTreshold, evaluator, gaSaParams.MutationStrategy, gaSaParams.CrossingStrategy, gaSaParams.SelctionStrategy)
        {
            this.saGroupSize = gaSaParams.saGroupSize;
            this.saRunFrequency = gaSaParams.saRunFrequency;
            this.saIterationsLimit = gaSaParams.saIterationsLimit;
        }

        protected override void NextGeneration(int generation)
        {
            base.NextGeneration(generation);

            if (generation % saRunFrequency == 0)
            {
                var indivdualsIndexes = Enumerable.Range(0, PopulationSize);
                int topIndividualIndex = indivdualsIndexes
                    .MaxBy(i => Population[i].Value);


                List<int> indexesToRun =
                        indivdualsIndexes
                            .Where(i => i != topIndividualIndex)
                            .OrderBy(i => r.Next())
                            .Take(saGroupSize - 1)
                            .ToList();

                indexesToRun.Add(topIndividualIndex);

                foreach (int index in indexesToRun)
                {
                    Individual subject = Population[index];
                    ProblemSlover ps = new SimulatedAnnealing(genPool, evaluator, new InverseGenerator(), 1, 200, 10, new ExponentialCooling(0.001), subject);
                    ps.Run(saIterationsLimit);
                    Population[index] = ps.BestIndividual;
                }
            }
        }
    }
}

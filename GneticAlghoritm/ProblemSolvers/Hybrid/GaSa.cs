using ProblemSolvers.ProblemSolvers;
using ProblemSolvers.ProblemSolvers.Crossing;
using ProblemSolvers.ProblemSolvers.Evaluation;
using ProblemSolvers.ProblemSolvers.Mutation;
using ProblemSolvers.hybrid;
using ProblemSolvers.ProblemSolvers.SA;
using ProblemSolvers.ProblemSolvers.SA.Cooling;
using ProblemSolvers.ProblemSolvers.TS.NeighbourHood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvers.Hybrid
{
    internal class GaSa : ProblemSolvers.GA.GeneticAlghoritm
    {
        private readonly Random r = new Random();
        private int saGroupSize;
        private int saRunFrequency;
        private int saIterationsLimit;

        public GaSa(GaSaParams gaSaParams, int[] populationGenes, IEvaluator evaluator)
            : base(gaSaParams, populationGenes, evaluator)
        {
            saGroupSize = gaSaParams.saGroupSize;
            saRunFrequency = gaSaParams.saRunFrequency;
            saIterationsLimit = gaSaParams.saIterationsLimit;
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
                    PermutationIndividual subject = Population[index];
                    var ps = new SimulatedAnnealing(new SaParams(new ExponentialCooling(0.001), 200, 0, new InverseGenerator()), subject, genPool, evaluator);
                    ps.Run(saIterationsLimit);
                    Population[index] = ps.BestIndividual;
                }
            }
        }
    }
}

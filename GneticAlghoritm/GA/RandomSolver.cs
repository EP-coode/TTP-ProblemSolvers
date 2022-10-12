using GeneticAlghoritm.GA.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlghoritm.GA;

internal class RandomSolver : ProblemSlover
{
    public RandomSolver(int[] populationGenes, IEvaluator evaluator) : base(populationGenes, evaluator)
    {
    }

    public override void Run(int generations)
    {
        for(int i = 0; i < generations; i++)
        {
            GenRandomPopulation();
            SaveStatsToLog();
            Individual generationBestIndividual = GetBestIndividualOfGeneration();
            if(BestIndividual is null || generationBestIndividual.Value > BestIndividual.Value)
            {
                BestIndividual = new Individual(generationBestIndividual);
            }
        }
    }
}


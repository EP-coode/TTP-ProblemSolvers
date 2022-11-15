using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public record EaParamsSpace
{
    public IEnumerable<ISelector> SelctionStrategy { get; init; }
    public IEnumerable<IMutationStrategy> MutationStrategy { get; init; }
    public IEnumerable<ICrossingStrategy> CrossingStrategy { get; init; }
    public IEnumerable<int> PopSize { get; init; }
    public IEnumerable<double> MutationTreshold { get; init; }
    public IEnumerable<double> CrossingTreshold { get; init; }
}


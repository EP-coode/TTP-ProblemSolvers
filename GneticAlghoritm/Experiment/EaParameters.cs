using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public record EaParameters
{
    public ISelector SelctionStrategy { get; init; }
    public IMutationStrategy MutationStrategy { get; init; }
    public ICrossingStrategy CrossingStrategy { get; init; }
    public int PopSize { get; init; }
    public double MutationTreshold { get; init; }
    public double CrossingTreshold { get; init; }
}


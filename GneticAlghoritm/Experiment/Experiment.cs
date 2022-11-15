using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GneticAlghoritm.Experiment;

public class Experiment
{
    public string Name { get; set; }
    public string FileName { get; set; }
    public ISelector SelctionStrategy { get; set; }
    public IMutationStrategy MutationStrategy { get; set; }
    public ICrossingStrategy CrossingStrategy { get; set; }
    public int Repeats { get; set; }
    public int PopSize { get; set ; }
    public double MutationTreshold { get; set; }
    public double CrossingTreshold { get; set; }
}


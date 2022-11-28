using GeneticAlghoritm.GA.Crossing;
using GeneticAlghoritm.GA.Mutation;
using GeneticAlghoritm.GA.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GneticAlghoritm.hybrid
{
    public class GaSaParams : GneticAlghoritm.Experiment.Experiment
    {
        public int saGroupSize {get;set;}
        public int saRunFrequency {get;set;}
        public int saIterationsLimit { get; set; } 
    }
}

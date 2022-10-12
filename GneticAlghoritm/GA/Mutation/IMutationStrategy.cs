using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlghoritm.GA;

namespace GeneticAlghoritm.GA.Mutation;

public interface IMutationStrategy
{
    public void Mutate(Individual individual);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GneticAlghoritm.GA;

namespace GneticAlghoritm.GA.Mutation;

public interface IMutationStrategy
{
    public void Mutate(Individual individual);
}


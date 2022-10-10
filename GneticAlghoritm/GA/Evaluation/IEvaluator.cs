using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GneticAlghoritm.GA;

namespace GneticAlghoritm.GA.Evaluation;

public interface IEvaluator
{
    public double Evaluate(Individual individual);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSwarmOptimization.Functions.Objective
{
    public interface IObjectiveFunction
    {
        double Min { get; }

        double Max { get; }

        double Evaluate(double[] input);
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}

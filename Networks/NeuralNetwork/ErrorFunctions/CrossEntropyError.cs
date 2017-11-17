using System;
using System.Linq;
using NeuralNetwork.MLP.Backpropagation;

namespace NeuralNetwork.ErrorFunctions
{
    public class CrossEntropyError : IDifferentiableErrorFunction
    {
        public double Evaluate(double[] output, double[] target)
            => -output.Zip(target, (o, e) => (output: o, target: e)).Sum(t => Math.Log(t.output) * t.target);

        public double EvaluateDerivative(BackpropagationNeuron outputNeuron, double target)
            => outputNeuron.Output - target;
    }
}

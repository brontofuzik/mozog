using System;
using System.Linq;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;

namespace NeuralNetwork.ErrorFunctions
{
    public class MeanSquaredError : IDifferentiableErrorFunction
    {
        public double Evaluate(double[] output, double[] target)
            => 0.5 * output.Zip(target, (o, e) => (output: o, expected: e)).Sum(p => Math.Pow(p.Item1 - p.Item2, 2));

        public double EvaluateDerivative(BackpropagationNeuron outputNeuron, double target)
        {
            var derivative = outputNeuron.Layer.ActivationFunc1.EvaluateDerivative(outputNeuron.Input);
            return derivative * (outputNeuron.Output - target);
        }
    }
}

using System;
using System.Linq;
using NeuralNetwork.MLP.Backpropagation;

namespace NeuralNetwork.ErrorFunctions
{
    public class MeanSquaredError : IDifferentiableErrorFunction
    {
        public double Evaluate(double[] output, double[] target)
            => 0.5 * output.Zip(target, (o, t) => (output: o, target: t)).Sum(p => Math.Pow(p.output - p.target, 2));

        public double EvaluateDerivative(BackpropagationNeuron outputNeuron, double target)
        {
            var derivative = outputNeuron.Layer.ActivationFunc1.EvaluateDerivative(outputNeuron.Input);
            return derivative * (outputNeuron.Output - target);
        }
    }
}

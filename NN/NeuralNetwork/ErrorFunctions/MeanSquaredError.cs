using System;
using System.Linq;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;

namespace NeuralNetwork.ErrorFunctions
{
    public class MeanSquaredError : IDifferentiableErrorFunction
    {
        public double Evaluate(double[] output, double[] expectedOutput)
            => 0.5 * output.Zip(expectedOutput, (o, e) => (output: o, expected: e)).Sum(p => Math.Pow(p.Item1 - p.Item2, 2));

        public double EvaluateDerivative(BackpropagationNeuron outputNeuron, double expectedOutput)
        {
            var derivative = outputNeuron.Layer.ActivationFunc1.EvaluateDerivative(outputNeuron.Input);
            return derivative * (outputNeuron.Output - expectedOutput);
        }
    }
}

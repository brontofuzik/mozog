using System;
using System.Linq;

namespace NeuralNetwork.MLP.ActivationFunctions
{
    public class SoftmaxFunction : IDifferentiableActivationFunction2
    {
        public double[] Evaluate(double[] inputs)
        {
            var sum = inputs.Sum(i => Math.Exp(i));
            return inputs.Select(i => Math.Exp(i) / sum).ToArray();
        }

        public double[] EvaluateDerivative(double[] input)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => "Softmax";
    }
}

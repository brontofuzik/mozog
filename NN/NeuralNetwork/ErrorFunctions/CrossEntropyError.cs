﻿using System;
using System.Linq;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;

namespace NeuralNetwork.ErrorFunctions
{
    public class CrossEntropyError : IDifferentiableErrorFunction
    {
        public double Evaluate(double[] output, double[] target)
            => -output.Zip(target, (o, e) => (output: o, expected: e)).Sum(t => Math.Log(t.Item1) * t.Item2);

        public double EvaluateDerivative(BackpropagationNeuron outputNeuron, double target)
            => outputNeuron.Output - target;
    }
}

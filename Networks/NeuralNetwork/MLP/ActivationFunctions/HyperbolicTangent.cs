﻿using System;

namespace NeuralNetwork.MLP.ActivationFunctions
{
    /// <summary>
    /// http://en.wikipedia.org/wiki/Hyperbolic_tangent
    /// </summary>
    public class HyperbolicTangent : IDifferentiableActivationFunction1
    {
        public double Evaluate(double x) => Math.Tanh(x);

        public double EvaluateDerivative(double x)
        {
            double y = Evaluate(x);
            return 1 - Math.Pow(y, 2);
        }

        public override string ToString() => "Tanh";
    }
}

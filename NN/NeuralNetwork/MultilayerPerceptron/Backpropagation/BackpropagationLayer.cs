using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    class BackpropagationLayer : ActivationLayer
    {
        #region Construction

        internal BackpropagationLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
        }

        protected override IActivationNeuron MakeNeuron()
            => new BackpropagationNeuron();

        #endregion // Construction

        private new IEnumerable<BackpropagationNeuron> Neurons
            => base.Neurons.Cast<BackpropagationNeuron>();

        internal new IDifferentiableActivationFunction ActivationFunction
            => (IDifferentiableActivationFunction) base.ActivationFunction;

        // Replaces steps b, c, d with one.
        public void Backpropagate(double[] desiredOutputVector)
        {
            if (desiredOutputVector == null)
            {
                throw new ArgumentNullException(nameof(desiredOutputVector));
            }

            if (desiredOutputVector.Length != NeuronCount)
            {
                throw new ArgumentException(nameof(desiredOutputVector));
            }

            Neurons.ForEach((n, i) => n.Backpropagate(desiredOutputVector[i]));
        }

        // Replaces steps b, c, d with one.
        public void Backpropagate()
        {
            Neurons.ForEach(n => n.Backpropagate());
        }

        public override string ToString() => "Bp-" + base.ToString();
    }
}
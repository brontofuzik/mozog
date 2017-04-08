using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationLayer : ActivationLayer
    {
        #region Construction

        internal BackpropagationLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
        }

        protected override ActivationNeuron MakeNeuron()
            => new BackpropagationNeuron();

        #endregion // Construction

        public new IEnumerable<BackpropagationNeuron> Neurons => NeuronList;

        private new IEnumerable<BackpropagationNeuron> NeuronList
            => base.NeuronList.Cast<BackpropagationNeuron>();

        public new IDifferentiableActivationFunction1 ActivationFunc1
            => (IDifferentiableActivationFunction1)base.ActivationFunc1;

        public new BackpropagationNetwork Network => base.Network as BackpropagationNetwork;

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

            NeuronList.ForEach((n, i) => n.Backpropagate(desiredOutputVector[i]));
        }

        // Replaces steps b, c, d with one.
        public void Backpropagate()
        {
            NeuronList.ForEach(n => n.Backpropagate());
        }

        public override string ToString() => "Bp-" + base.ToString();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Construction;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationNetwork : Network
    {
        #region Construction

        internal BackpropagationNetwork(NetworkArchitecture architecture)
            : base(architecture)
        {
        }

        protected override ILayer MakeLayer(NetworkArchitecture.Layer layerPlan)
            => layerPlan.Activation != null ? (ILayer)new BackpropagationLayer(layerPlan) : (ILayer)new InputLayer(layerPlan);

        #endregion // Construction

        private new IEnumerable<BackpropagationSynapse> Synapses => base.Synapses.Cast<BackpropagationSynapse>();

        public double Error { get; private set; }

        public void SetLearningRates(double learningRate)
        {
            Synapses.ForEach(s => s.LearningRate = learningRate);
        }

        public void ResetError()
        {
            Error = 0.0;
        }

        public void ResetPartialDerivatives()
        {
            Synapses.ForEach(s => s.ResetPartialDerivative());
        }

        public void UpdateError()
        {
            double partialError = 0.0;
            foreach (BackpropagationNeuron outputNeuron in OutputLayer.Neurons)
            {
                partialError += Math.Pow(outputNeuron.PartialDerivative, 2);
            }

            Error += 0.5 * partialError;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double[] desiredOutputVector)
        {
            (OutputLayer as BackpropagationLayer).Backpropagate(desiredOutputVector);

            // Hidden layers (backwards).
            for (int i = Layers.Count - 2; i > 0; i--)
            {
                (Layers[i] as BackpropagationLayer).Backpropagate();
            }
        }

        public void UpdatePartialDerivatives()
        {
            Synapses.ForEach(s => s.UpdatePartialDerivative());
        }

        public void UpdateWeights()
        {
            Synapses.ForEach(s => s.UpdateWeight());
        }

        public void UpdateSynapseLearningRates()
        {
            Synapses.ForEach(s => s.UpdateLearningRate());
        }

        public override string ToString() => "BP" + base.ToString();
    }
}


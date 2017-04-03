using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationNetwork : Network
    {
        #region Construction

        internal BackpropagationNetwork(INetworkArchitecture architecture)
            : base(architecture)
        {
        }

        protected override ILayer MakeLayer(NetworkArchitecture.Layer layerPlan)
            => layerPlan.Activation != null ? (ILayer)new BackpropagationLayer(layerPlan) : (ILayer)new InputLayer(layerPlan);

        #endregion // Construction

        private new BackpropagationLayer OutputLayer => (BackpropagationLayer)base.OutputLayer;

        private IEnumerable<BackpropagationLayer> HiddenLayersReverse()
        {
            for (int i = Layers.Count - 2; i > 0; i--)
            {
                yield return Layers[i] as BackpropagationLayer;
            }
        }

        private new IEnumerable<BackpropagationSynapse> Synapses => base.Synapses.Cast<BackpropagationSynapse>();

        public double Error { get; private set; }

        public void SetLearningRate(double learningRate)
        {
            Synapses.ForEach(s => s.LearningRate = learningRate);
        }

        public void SetMomentum(double momentum)
        {
            Synapses.ForEach(s => s.Momentum = momentum);
        }

        public void ResetError()
        {
            Error = 0.0;
        }

        public void ResetPartialDerivatives()
        {
            Synapses.ForEach(s => s.ResetPartialDerivative());
        }

        // Mean-squared error
        public void UpdateError()
        {
            // TODO Remove casting.
            Error += 0.5 * OutputLayer.Neurons_Typed.Sum(n => Math.Pow(n.PartialDerivative, 2));
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double[] expectedOutput)
        {
            OutputLayer.Backpropagate(expectedOutput);
            HiddenLayersReverse().ForEach(l => l.Backpropagate());
        }

        public void UpdatePartialDerivatives()
        {
            Synapses.ForEach(s => s.UpdatePartialDerivative());
        }

        public void UpdateWeights()
        {
            Synapses.ForEach(s => s.UpdateWeight());
        }

        public void UpdateLearningRates()
        {
            Synapses.ForEach(s => s.UpdateLearningRate());
        }

        public override string ToString() => "Bp-" + base.ToString();
    }
}


using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.ErrorFunctions;
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

        private new BackpropagationLayer OutputLayer => base.OutputLayer as BackpropagationLayer;

        private IEnumerable<BackpropagationLayer> HiddenLayersReverse()
        {
            for (int i = Layers.Count - 2; i > 0; i--)
            {
                yield return Layers[i] as BackpropagationLayer;
            }
        }

        private new IEnumerable<BackpropagationSynapse> Synapses => base.Synapses.Cast<BackpropagationSynapse>();

        // TODO
        public IDifferentiableErrorFunction ErrorFunc => errorFunc as IDifferentiableErrorFunction;

        public void Initialize(BackpropagationArgs args)
        {
            Synapses.ForEach(s => s.Initialize(args));
        }

        public void ResetPartialDerivatives()
        {
            Synapses.ForEach(s => s.ResetPartialDerivative());
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double[] target)
        {
            OutputLayer.Backpropagate(target);
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


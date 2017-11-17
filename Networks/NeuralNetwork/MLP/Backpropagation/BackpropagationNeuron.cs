using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MLP.Backpropagation
{
    public class BackpropagationNeuron : ActivationNeuron
    {
        #region Construction

        internal BackpropagationNeuron()
        {
        }

        protected override ISynapse MakeSynapse() => new BackpropagationSynapse();

        #endregion // Construction

        // "Delta"
        public double Error { get; set; }

        private new IEnumerable<BackpropagationSynapse> SourceSynapses
            => base.SourceSynapses.Cast<BackpropagationSynapse>();

        private new IEnumerable<BackpropagationSynapse> TargetSynapses
            => base.TargetSynapses.Cast<BackpropagationSynapse>();

        public new BackpropagationLayer Layer => base.Layer as BackpropagationLayer;

        public void Initialize(BackpropagationArgs args)
        {
            base.Initialize();

            Error = 0.0;
        }

        // Output layer
        public void Backpropagate(double target)
        {
            Error = Layer.Network.ErrorFunc.EvaluateDerivative(this, target);

            SourceSynapses.ForEach(s => s.Backpropagate());
        }

        // Hidden layer
        public void Backpropagate()
        {
            var derivative = Layer.ActivationFunc1.EvaluateDerivative(Input);
            Error = derivative * TargetSynapses.Select(s => s.TargetNeuron.Error * s.Weight).Sum();

            SourceSynapses.ForEach(s => s.Backpropagate());
        }

        public override string ToString() => "Bp-" + base.ToString();
    }
}
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
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

        private new IEnumerable<BackpropagationSynapse> TargetSynapses
            => base.TargetSynapses.Cast<BackpropagationSynapse>();

        public new BackpropagationLayer Layer => base.Layer as BackpropagationLayer;

        public void Backpropagate(double target)
        {
            Error = Layer.Network.ErrorFunc.EvaluateDerivative(this, target);
        }

        public void Backpropagate()
        {
            var derivative = Layer.ActivationFunc1.EvaluateDerivative(Input);
            Error = derivative * TargetSynapses.Select(s => s.TargetNeuron.Error * s.Weight).Sum();
        }

        public override string ToString() => "Bp-" + base.ToString();
    }
}
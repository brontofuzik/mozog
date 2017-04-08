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

        //public double PartialDerivative { get; private set; }

        // "Delta"
        public double Error { get; set; }

        //public double ActivationDerivative => Layer.ActivationFunc1.EvaluateDerivative(Input);

        private new IEnumerable<BackpropagationSynapse> TargetSynapses
            => base.TargetSynapses.Cast<BackpropagationSynapse>();

        public new BackpropagationLayer Layer => base.Layer as BackpropagationLayer;

        public override void Initialize()
        {
            base.Initialize();

            Error = 0.0;
        }

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
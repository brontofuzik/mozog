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

        public double PartialDerivative { get; private set; }

        public double Error { get; private set; }

        public double ActivationFunctionDerivative => Layer.ActivationFunction.EvaluateDerivative(Input);

        private new IEnumerable<BackpropagationSynapse> TargetSynapses
            => base.TargetSynapses.Cast<BackpropagationSynapse>();

        private new BackpropagationLayer Layer => (BackpropagationLayer)base.Layer;

        public override void Initialize()
        {
            base.Initialize();

            PartialDerivative = 0.0;
            Error = 0.0;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double desiredOutput)
        {
            PartialDerivative = Output - desiredOutput;
            Error =  PartialDerivative * ActivationFunctionDerivative;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            PartialDerivative = 0.0;
            foreach (var targetSynapse in TargetSynapses)
            {
                BackpropagationNeuron targetNeuron = targetSynapse.TargetNeuron as BackpropagationNeuron;
                PartialDerivative += targetNeuron.Error * targetSynapse.Weight;
            }

            Error = PartialDerivative * ActivationFunctionDerivative;
        }

        public override string ToString() => "Bp-" + base.ToString();
    }
}
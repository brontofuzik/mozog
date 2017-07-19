using System;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationNeuron : NeuronBase, IActivationNeuron
    {
        #region Construction

        internal ActivationNeuron()
        {
        }

        protected override ISynapse MakeSynapse() => new Synapse();

        #endregion // Construction

        private new IActivationLayer Layer => base.Layer as IActivationLayer;

        protected void Initialize()
        {
            Input = 0.0;
            Output = 0.0;
        }

        public virtual void Evaluate()
        {
            EvaluateInput();
            EvaluateOutput();
        }

        public void EvaluateInput()
        {
            Input = SourceSynapses.Sum(s => s.SourceNeuron.Output * s.Weight);
        }

        public void EvaluateOutput()
        {
            Output = Layer.ActivationFunc1.Evaluate(Input);
        }

        public override string ToString() => $"AN([{String.Join(", ", SourceSynapses)}], {Input:F2}, {Output:F2})";
    }
}
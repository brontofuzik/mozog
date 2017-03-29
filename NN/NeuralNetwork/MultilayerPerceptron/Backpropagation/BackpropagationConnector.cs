using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Construction;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationConnector : Connector
    {
        internal BackpropagationConnector(NetworkArchitecture.Connector connector)
            : base(connector)
        {
        }

        public double Momentum { get; set; }

        private new IEnumerable<BackpropagationSynapse> Synapses => base.Synapses.Cast<BackpropagationSynapse>();

        public void SetLearningRates(double learningRate)
        {
            Synapses.ForEach(s => s.LearningRate = learningRate);
        }

        public void ResetPartialDerivatives()
        {
            Synapses.ForEach(s => s.ResetPartialDerivative());
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

        public override string ToString() => "BP" + base.ToString();
    }
}


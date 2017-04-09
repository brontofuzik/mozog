using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Synapse : ISynapse
    {
        #region Construction

        internal Synapse()
        {
        }

        #endregion // Construction

        public double Weight { get; set; } = StaticRandom.Double(-1, +1);

        public INeuron SourceNeuron { get; set; }

        // Owner neuron
        public INeuron TargetNeuron { get; set; }

        public override string ToString() => Weight.ToString("F2");
    }
}
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

        // TODO Jitter
        //public void Jitter(double noiseLimit)
        //{
        //    Weight += StaticRandom.Double(-noiseLimit, +noiseLimit);
        //}

        public override string ToString() => Weight.ToString("F2");
    }
}
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

        public double Weight { get; set; }

        public INeuron SourceNeuron { get; set; }

        // Owner neuron
        public INeuron TargetNeuron { get; set; }

        public virtual void Initialize()
        {
            Weight = StaticRandom.Double(-1, +1);
        }

        // TODO Jitter
        //public void Jitter(double noiseLimit)
        //{
        //    Weight += StaticRandom.Double(-noiseLimit, +noiseLimit);
        //}

        public override string ToString() => Weight.ToString("F2");
    }
}
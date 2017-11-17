using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MLP
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

        protected void Initialize()
        {
            Weight = StaticRandom.Double(-1, +1);
        }

        public override string ToString() => Weight.ToString("F2");
    }
}
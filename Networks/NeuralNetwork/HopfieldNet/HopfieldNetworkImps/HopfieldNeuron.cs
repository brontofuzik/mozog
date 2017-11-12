using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.HopfieldNet.HopfieldNetworkImps
{
    /*
    class HopfieldNeuron
    {
        private readonly ActivationFunction activationFunction;

        public HopfieldNeuron(int index, ActivationFunction activationFunction)
        {
            Require.IsNonNegative(index, nameof(index));
            Require.IsNotNull(activationFunction, nameof(activationFunction));

            Index = index;
            this.activationFunction = activationFunction;
        }

        public int Index { get; }

        public ICollection<HopfieldSynapse> Synapses { get; } = new HashSet<HopfieldSynapse>();

        public double Bias { get; set; }

        public double Output { get; set; }

        public void Evaluate(double progress)
        {
            var input = Synapses.Select(s => s.GetOtherNeuron(this).Output * s.Weight).Sum();
            input += Bias;
            Output = activationFunction(input, progress);
        }
    }
    */
}

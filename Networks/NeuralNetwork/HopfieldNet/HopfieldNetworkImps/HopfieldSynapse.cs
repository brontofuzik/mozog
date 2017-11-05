using Mozog.Utils;

namespace NeuralNetwork.HopfieldNet.HopfieldNetworkImps
{
    class HopfieldSynapse
    {
        public HopfieldSynapse(HopfieldNeuron neuron1, HopfieldNeuron neuron2)
        {
            Require.IsNotNull(neuron1, nameof(neuron1));
            Require.IsNotNull(neuron2, nameof(neuron2));

            Neuron1 = neuron1;
            Neuron2 = neuron2;
            Weight = 0.0;
        }

        public HopfieldNeuron Neuron1 { get; set; }

        public HopfieldNeuron Neuron2 { get; set; }

        public double Weight { get; set; }

        public HopfieldNeuron GetOtherNeuron(HopfieldNeuron neuron)
            => neuron == Neuron1 ? Neuron2 : Neuron1;
    }
}

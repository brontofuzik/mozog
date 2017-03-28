using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class InputNeuron : NeuronBase, IInputNeuron
    {
        public InputNeuron(InputLayer parentLayer)
        {
            Require.IsNotNull(parentLayer, nameof(parentLayer));
            Layer = parentLayer;
        }

        // Factory
        internal InputNeuron()
        {
        }

        public IInputLayer Layer { get; set; }

        public override string ToString() => $"IN({Output:F2})";
    }
}

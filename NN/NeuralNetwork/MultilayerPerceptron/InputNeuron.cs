using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class InputNeuron : NeuronBase, IInputNeuron
    {
        internal InputNeuron()
        {
        }

        public override string ToString() => $"IN({Output:F2})";
    }
}

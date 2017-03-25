using System.Collections.Generic;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    public class InputNeuron : INeuron
    {
        private InputLayer parentLayer;

        public InputNeuron(InputLayer parentLayer)
        {
            Require.IsNotNull(parentLayer, nameof(parentLayer));
            this.parentLayer = parentLayer;
        }

        public double Output { get; set; }

        public List<ISynapse> TargetSynapses { get; } = new List<ISynapse>();

        public ILayer ParentLayer
        {
            get { return parentLayer; }
            set { parentLayer = value as InputLayer; }
        }

        public void Initialize()
        {
            Output = 0.0;
        }

        public override string ToString() => $"IN({Output:F2})";
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    public class InputLayer : ILayer
    {
        private readonly List<InputNeuron> neurons;

        public InputLayer(LayerBlueprint blueprint, INetwork parentNetwork)
        {
            // Create the neurons.
            neurons = new List<InputNeuron>(blueprint.NeuronCount);
            for (int i = 0; i < blueprint.NeuronCount; i++)
            {
                InputNeuron neuron = new InputNeuron(this);
                neurons.Add(neuron);
            }

            TargetConnectors = new List<IConnector>();

            // Validate the parent network.
            Require.IsNotNull(parentNetwork, "parentNetwork");
            ParentNetwork = parentNetwork;
        }

        public List<INeuron> Neurons => new List<INeuron>(neurons);

        public int NeuronCount => neurons.Count;

        public List<IConnector> TargetConnectors { get; }

        public INetwork ParentNetwork { get; set; }

        public INeuron GetNeuronByIndex(int neuronIndex) => neurons[neuronIndex];

        public void Initialize()
        {
            // Do nothing.
        }

        public void SetInputVector(double[] inputVector)
        {
            if (inputVector == null)
            {
                throw new ArgumentNullException(nameof(inputVector));
            }
            if (inputVector.Length != NeuronCount)
            {
                throw new ArgumentException(nameof(inputVector));
            }

            for (int i = 0; i < NeuronCount; i++)
            {
                neurons[i].Output = inputVector[i];
            }
        }

        // TODO
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("IL\n[\n");

            int neuronIndex = 0;
            foreach (InputNeuron neuron in neurons)
            {
                sb.Append("  " + neuronIndex++ + " : " + neuron + "\n");
            }
            sb.Append("]");
            
            return sb.ToString();
        }
    }
}

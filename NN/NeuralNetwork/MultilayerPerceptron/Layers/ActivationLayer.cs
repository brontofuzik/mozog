using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    public class ActivationLayer : IActivationLayer
    {
        public ActivationLayer(ActivationLayerBlueprint blueprint, INetwork parentNetwork)
        {
            // Create the neurons.
            Neurons = new List<IActivationNeuron>(blueprint.NeuronCount);
            for (int i = 0; i < blueprint.NeuronCount; i++)
            {
                IActivationNeuron neuron = new ActivationNeuron(this);
                Neurons.Add(neuron);
            }

            Require.IsNotNull(blueprint.ActivationFunction, nameof(blueprint.ActivationFunction));
            ActivationFunction = blueprint.ActivationFunction;

            Require.IsNotNull(parentNetwork, nameof(parentNetwork));
            this.ParentNetwork = parentNetwork;
        }

        public List<IActivationNeuron > Neurons { get; }

        List<INeuron> ILayer.Neurons
        {
            get
            {
                List<INeuron> iNeurons = new List<INeuron>(Neurons.Count);
                foreach (INeuron neuron in Neurons)
                {
                    iNeurons.Add(neuron);
                }
                return iNeurons;
            }
        }

        public int NeuronCount => Neurons.Count;

        public INeuron this[int neuronIndex]
        {
            get
            {
                return Neurons[neuronIndex];
            }
        }

        public IActivationFunction ActivationFunction { get; }

        public List<IConnector> SourceConnectors { get; } = new List<IConnector>();

        public List<IConnector> TargetConnectors { get; } = new List<IConnector>();

        public INetwork ParentNetwork { get; set; }

        public INeuron GetNeuronByIndex(int neuronIndex) => Neurons[neuronIndex];

        public void Initialize()
        {
            // Ref
            Neurons.AsEnumerable().ForEach(n => n.Initialize());
        }

        public void Evaluate()
        {
            // Ref
            Neurons.AsEnumerable().ForEach(n => n.Evaluate());
        }

        // Ref
        public double[] GetOutputVector() => Neurons.Select(n => n.Output).ToArray();

        // TODO
        public override string ToString()
        {
            var sb = new StringBuilder("AL\n[\n");

            int neuronIndex = 0;
            foreach (IActivationNeuron neuron in Neurons)
            {
                sb.Append("  " + neuronIndex++ + " : " + neuron + "\n");
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
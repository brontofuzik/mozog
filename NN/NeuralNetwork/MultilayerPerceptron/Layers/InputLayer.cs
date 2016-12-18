using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;


namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <remarks>
    /// An input layer.
    /// </remarks>
    public class InputLayer
        : ILayer
    {
        /// <summary>
        /// 
        /// </summary>
        private List<InputNeuron> neurons;

        /// <summary>
        /// The target connector of the layer.
        /// </summary>
        private List<IConnector> targetConnectors;

        /// <summary>
        /// The parent network of the layer.
        /// </summary>
        private INetwork parentNetwork;

        /// <summary>
        /// 
        /// </summary>
        public List<INeuron> Neurons
        {
            get
            {
                List<INeuron> iNeurons = new List<INeuron>(neurons.Count);
                foreach (INeuron neuron in neurons)
                {
                    iNeurons.Add(neuron);
                }
                return iNeurons;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NeuronCount
        {
            get
            {
                return neurons.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IConnector> TargetConnectors
        {
            get
            {
                return targetConnectors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public INetwork ParentNetwork
        {
            get
            {
                return parentNetwork;
            }
            set
            {
                parentNetwork = value;
            }
        }

        /// <summary>
        /// Creates a new input layer.
        /// </summary>
        /// 
        /// <param name="blueprint">The blueprint of the input layer.</param>
        /// <param name="parentNetwork">The parent network.</param>
        public InputLayer(LayerBlueprint blueprint, INetwork parentNetwork)
        {
            // Create the neurons.
            neurons = new List< InputNeuron >(blueprint.NeuronCount);
            for (int i = 0; i < blueprint.NeuronCount; i++)
            {
                InputNeuron neuron = new InputNeuron(this);
                neurons.Add(neuron);
            }

            targetConnectors = new List<IConnector>();

            // Validate the parent network.
            Utilities.RequireObjectNotNull(parentNetwork, "parentNetwork");
            this.parentNetwork = parentNetwork;
        }

        /// <summary>
        /// Gets a neuron (specified by its index within the layer).
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>
        /// The neuron.
        /// </returns>
        public INeuron GetNeuronByIndex(int neuronIndex)
        {
            return neurons[neuronIndex];
        }

        /// <summary>
        /// Initializes the layer.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVector"></param>
        /// 
        /// <exception cref="System.ArgumentNullException">
        /// Condition: <c>inputVector</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Condition: the length of <c>inputVector</c> differs from the number of neurons in the input layer.
        /// </exception>
        public void SetInputVector(double[] inputVector)
        {
            // Validate the input vector.
            if (inputVector == null)
            {
                throw new ArgumentNullException(nameof(inputVector));
            }
            // Validate the length of the input vector.
            if (inputVector.Length != NeuronCount)
            {
                throw new ArgumentException("inputVector");
            }

            for (int i = 0; i < NeuronCount; i++)
            {
                neurons[i].Output = inputVector[i];
            }
        }

        /// <summary>
        /// Returns a string representation of the input layer.
        /// </summary>
        /// <returns>
        /// A string representation of the input layer.
        /// </returns>
        public override string ToString()
        {
            StringBuilder inputLayerSB = new StringBuilder();

            inputLayerSB.Append("IL\n[\n");
            int neuronIndex = 0;
            foreach (InputNeuron neuron in neurons)
            {
                inputLayerSB.Append("  " + neuronIndex++ + " : " + neuron + "\n");
            }
            inputLayerSB.Append("]");
            
            return inputLayerSB.ToString();
        }
    }
}

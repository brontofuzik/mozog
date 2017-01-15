using System;
using System.Text;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.Utils;


namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    /// <remarks>
    /// A network blueprint.
    /// </remarks>
    public class NetworkBlueprint
    {
        /// <summary>
        /// The blueprint of the bias layer.
        /// </summary>
        private LayerBlueprint biasLayerBlueprint;

        /// <summary>
        /// The blueprint of the input layer.
        /// </summary>
        private LayerBlueprint inputLayerBlueprint;

        /// <summary>
        /// The blueprints of the hidden layers.
        /// </summary>
        private ActivationLayerBlueprint[] hiddenLayerBlueprints;

        /// <summary>
        /// The buleprint of the output layer.
        /// </summary>
        private ActivationLayerBlueprint outputLayerBlueprint;

        /// <summary>
        /// The blueprints of the connectors (as an array).
        /// </summary>
        private ConnectorBlueprint[] connectorBlueprints;

        /// <summary>
        /// Gets the number of (input, hidden, and output) layers comprising the network.
        /// </summary>
        /// <value>
        /// The number of (input, hidden, and output) layers comprising the network.
        /// </value>
        public int LayerCount
        {
            get
            {
                return 1 + HiddenLayerCount + 1;
            }
        }

        /// <summary>
        /// Gets the blueprint of the bias layer.
        /// </summary>
        /// <value>
        /// The blueprint of the bias layer.
        /// </value>
        public LayerBlueprint BiasLayerBlueprint
        {
            get
            {
                return biasLayerBlueprint;
            }
        }

        /// <summary>
        /// Gets the number of neurons comprising the bias layer.
        /// </summary>
        /// <value>
        /// The number of neurons comprising the bias layer.
        /// </value>
        public int BiasLayerNeuronCount
        {
            get
            {
                return biasLayerBlueprint.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the blueprint of the input layer.
        /// </summary>
        /// <value>
        /// The blueprint of the input layer.
        /// </value>
        public LayerBlueprint InputLayerBlueprint
        {
            get
            {
                return inputLayerBlueprint;
            }
        }

        /// <summary>
        /// Gets the number of neurons comprising the input layer.
        /// </summary>
        /// <value>
        /// The number of neurons comprising the input layer.
        /// </value>
        public int InputLayerNeuronCount
        {
            get
            {
                return inputLayerBlueprint.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the blueprints of the hidden layers.
        /// </summary>
        /// <value>
        /// The blueprints of the hidden layers.
        /// </value>
        public ActivationLayerBlueprint[] HiddenLayerBlueprints
        {
            get
            {
                return hiddenLayerBlueprints;
            }
        }

        /// <summary>
        /// Gets the number of hidden layers.
        /// </summary>
        /// <value>
        /// The number of hidden layers.
        /// </value>
        public int HiddenLayerCount
        {
            get
            {
                return hiddenLayerBlueprints.Length;
            }
        }

        /// <summary>
        /// Gets the blueprint of the output layer.
        /// </summary>
        /// <value>
        /// The blueprint of the output layer.
        /// </value>
        public ActivationLayerBlueprint OutputLayerBlueprint
        {
            get
            {
                return outputLayerBlueprint;
            }
        }

        /// <summary>
        /// Gets the number of neurons comprising the output layer.
        /// </summary>
        /// <value>
        /// The number of neurons comprising the output layer.
        /// </value>
        public int OutputLayerNeuronCount
        {
            get
            {
                return outputLayerBlueprint.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the blueprints of the connectors.
        /// </summary>
        /// <value>
        /// The blueprints of the connectors.
        /// </value>
        public ConnectorBlueprint[] ConnectorBlueprints
        {
            get
            {
                return connectorBlueprints;
            }
        }

        /// <summary>
        /// Gets the number of connectors.
        /// </summary>
        /// <value>
        /// The number of connectors.
        /// </value>
        public int ConnectorCount
        {
            get
            {
                return connectorBlueprints.Length;
            }
        }

        /// <summary>
        /// Creates a new network blueprint.
        /// </summary>
        /// <param name="inputLayerBlueprint">The blueprint of the input layer.</param>
        /// <param name="hiddenLayerBlueprints">The blueprints of the hidden layers.</param>
        /// <param name="outputLayerBlueprint">The blueprint of the output layer.</param>
        public NetworkBlueprint(LayerBlueprint inputLayerBlueprint, ActivationLayerBlueprint[] hiddenLayerBlueprints, ActivationLayerBlueprint outputLayerBlueprint)
        {
            Initialize(inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint);
        }

        /// <summary>
        /// Creates a new network blueprint.
        /// </summary>
        /// <param name="inputLayerBlueprint">The blueprint of the input layer.</param>
        /// <param name="hiddenLayerBlueprint">The blueprint of the hidden layer.</param>
        /// <param name="outputLayerBlueprint">The blueprint of the output layer.</param>
        public NetworkBlueprint(LayerBlueprint inputLayerBlueprint, ActivationLayerBlueprint hiddenLayerBlueprint, ActivationLayerBlueprint outputLayerBlueprint)
            : this(inputLayerBlueprint, new ActivationLayerBlueprint[1] { hiddenLayerBlueprint }, outputLayerBlueprint)
        {
        }

        /// <summary>
        /// Creates a new network blueprint.
        /// </summary>
        /// <<param name="layerNeuronCounts">The numbers of neurons comprising the layers.</param>
        public NetworkBlueprint(params int[] layerNeuronCounts)
        {
            if (layerNeuronCounts.Length < 2)
            {
                throw new ArgumentException("layerNeuronCounts");
            }

            // Create the input layer blueprint.
            int inputLayerNeuronCount = layerNeuronCounts[0];
            LayerBlueprint inputLayerBlueprint = new LayerBlueprint(inputLayerNeuronCount);
            
            // Create the hidden layer blueprints.
            int hiddenLayerCount = layerNeuronCounts.Length - 2;
            ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[hiddenLayerCount];
            for (int i = 0; i < hiddenLayerCount; i++)
            {
                int hiddenLayerNeuronCount = layerNeuronCounts[1 + i];
                hiddenLayerBlueprints[i] = new ActivationLayerBlueprint(hiddenLayerCount);
            }

            // Create the output layer blueprint.
            int outputLayerNeuronCount = layerNeuronCounts[layerNeuronCounts.Length - 1];
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint(outputLayerNeuronCount);

            Initialize(inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint);
        }

        /// <summary>
        /// Initializes the network blueprint.
        /// </summary>
        /// <param name="inputLayerBlueprint">The blueprint of the input layer.</param>
        /// <param name="hiddenLayerBlueprints">The blueprints of the hidden layers.</param>
        /// <param name="outputLayerBlueprint">The blueprint of the output layer.</param>
        public void Initialize(LayerBlueprint inputLayerBlueprint, ActivationLayerBlueprint[] hiddenLayerBlueprints, ActivationLayerBlueprint outputLayerBlueprint)
        {
            // 1. Create the layer blueprints.

            biasLayerBlueprint = new LayerBlueprint(1);

            Require.IsNotNull(inputLayerBlueprint, "inputLayerBlueprint");
            this.inputLayerBlueprint = inputLayerBlueprint;

            Require.IsNotNull(hiddenLayerBlueprints, "hiddenLayerBlueprints");
            foreach (ActivationLayerBlueprint hiddenLayerBlueprint in hiddenLayerBlueprints)
            {
                Require.IsNotNull(hiddenLayerBlueprint, "hiddenLayerBlueprint");
            }
            this.hiddenLayerBlueprints = hiddenLayerBlueprints;

            Require.IsNotNull(outputLayerBlueprint, "outputLayerBlueprint");
            this.outputLayerBlueprint = outputLayerBlueprint;

            // 2. Create the connector blueprints.
            
            connectorBlueprints = new ConnectorBlueprint[(HiddenLayerCount + 1) * 2];
            
            int i = 0;
            for (int targetLayerIndex = 1; targetLayerIndex < LayerCount; targetLayerIndex++)
            {
                // 2.3. Create the connector between the source layer (bias) and the target layer.
                int sourceLayerIndex = -1;
                int sourceLayerNeuronCount = 1;
                int targetLayerNeuronCount = GetLayerNeuronCount(targetLayerIndex);
                connectorBlueprints[i++] = new ConnectorBlueprint(sourceLayerIndex, sourceLayerNeuronCount, targetLayerIndex, targetLayerNeuronCount);

                // 2.4. Create the connector between the source layer (previous) and the target layer.
                sourceLayerIndex = targetLayerIndex - 1;
                sourceLayerNeuronCount = GetLayerNeuronCount(sourceLayerIndex);
                connectorBlueprints[i++] = new ConnectorBlueprint(sourceLayerIndex, sourceLayerNeuronCount, targetLayerIndex, targetLayerNeuronCount);
            }
        }

        /// <summary>
        /// Gets the number of neurons comprising a layer (specified by its index).
        /// </summary>
        /// 
        /// <param name="layerIndex">The index of the layer.</param>
        /// 
        /// <returns>
        /// The number of neurons comprising the layer.
        /// </returns>
        public int GetLayerNeuronCount(int layerIndex)
        {
            if (layerIndex == -1)
            {
                return BiasLayerNeuronCount;
            }
            else if (layerIndex == 0)
            {
                return InputLayerNeuronCount;
            }
            else if (0 < layerIndex && layerIndex < LayerCount - 1)
            {
                return hiddenLayerBlueprints[layerIndex - 1].NeuronCount;
            }
            else if (layerIndex == LayerCount - 1)
            {
                return OutputLayerNeuronCount;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public int GetHiddenLayerNeuronCount(int hiddenLayerIndex)
        {
            return hiddenLayerBlueprints[hiddenLayerIndex].NeuronCount;
        }

        /// <summary>
        /// Converts a network blueprint to its string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the network blueprint.
        /// </returns>
        public override string ToString()
        {
            StringBuilder networkBlueprintSB = new StringBuilder();

            networkBlueprintSB.Append("MLP(" + inputLayerBlueprint + ", [");
            foreach (ActivationLayerBlueprint hiddenLayerBlueprint in hiddenLayerBlueprints)
            {
                networkBlueprintSB.Append(hiddenLayerBlueprint + ", ");
            }
            if (hiddenLayerBlueprints.Length > 0)
            {
                networkBlueprintSB.Remove(networkBlueprintSB.Length - 2, 2);
            }
            networkBlueprintSB.Append("], " + outputLayerBlueprint + ")");

            return networkBlueprintSB.ToString();
        }
    }
}
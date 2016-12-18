using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Neurons;
using NeuralNetwork.MultilayerPerceptron.Synapses;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    /// <summary>
    /// A (multilayer feedforward) neural network.
    /// </summary>
    public class Network
        : INetwork
    {
        /// <summary>
        /// Creates a new neural network (from its blueprint).
        /// </summary>
        /// 
        /// <param name="blueprint">The blueprint of the network.</param>
        public Network(NetworkBlueprint blueprint)
        {
            // 0. Validate the blueprint.
            Utilities.RequireObjectNotNull(blueprint, "blueprint");
            _blueprint = blueprint;

            // 1. Create the network components.
            // 1.1. Create the layers.
            // 1.1.1. Create the bias layer.
            _biasLayer = new InputLayer(_blueprint.BiasLayerBlueprint, this);
            _biasLayer.SetInputVector(new double[] { 1.0 });

            // 1.1.2. Create the input layer.
            _inputLayer = new InputLayer(_blueprint.InputLayerBlueprint, this);

            // 1.1.3. Create the hidden layers.
            _hiddenLayers = new List< IActivationLayer >(_blueprint.HiddenLayerCount);
            foreach (ActivationLayerBlueprint hiddenLayerBlueprint in _blueprint.HiddenLayerBlueprints)
            {
                IActivationLayer hiddenLayer = new ActivationLayer(hiddenLayerBlueprint, this);
                _hiddenLayers.Add(hiddenLayer);
            }

            // 1.1.4. Create the output layer.
            _outputLayer = new ActivationLayer(_blueprint.OutputLayerBlueprint, this);

            // 1.2 Create the connectors.
            _connectors = new List< IConnector >(_blueprint.ConnectorCount);
            foreach (ConnectorBlueprint connectorBlueprint in _blueprint.ConnectorBlueprints)
            {
                IConnector connector = new Connector(connectorBlueprint, this);
                _connectors.Add(connector);
            }

            // 2. Connect the network.
            Connect();
        }

        /// <summary>
        /// Connects the network.
        /// 
        /// A network is said to be connected if:
        /// 
        /// 1. its connectors are connected.
        /// </summary>
        public void Connect()
        {
            // 1. Connect the connectors.
            foreach (IConnector connector in _connectors)
            {
                connector.Connect();
            }
        }

        /// <summary>
        /// Disconnects the network.
        /// 
        /// A network is said to be disconnected if:
        /// 
        /// 1. its connectors are disconnected.
        /// </summary>
        public void Disconnect()
        {
            // 1. Disconnect the connectors.
            foreach (IConnector connector in _connectors)
            {
                connector.Disconnect();
            }
        }

        /// <summary>
        /// Gets the layer (specified by its index).
        /// </summary>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <returns>
        /// The layer.
        /// </returns>
        public ILayer GetLayerByIndex(int layerIndex)
        {
            if (layerIndex == -1)
            {
                return _biasLayer;
            }
            else if (layerIndex == 0)
            {
                return _inputLayer;
            }
            else if (layerIndex == LayerCount - 1)
            {
                return _outputLayer;
            }
            else
            {
                return _hiddenLayers[layerIndex - 1];
            }
        }

        /// <summary>
        /// Initializes the network.
        /// </summary>
        public void Initialize()
        {
            _biasLayer.Initialize();
            _inputLayer.Initialize();
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                hiddenLayer.Initialize();
            }
            _outputLayer.Initialize();

            foreach (IConnector connector in _connectors)
            {
                connector.Initialize();
            }
        }

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>
        /// The output vector.
        /// </returns>
        public double[] Evaluate(double[] inputVector)
        {
            SetInputVector(inputVector);

            Evaluate();

            return GetOutputVector();
        }

        /// <summary>
        /// Calculate the error of the network with respect to a training set.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        public double CalculateError(TrainingSet trainingSet)
        {
            double networkError = 0.0;
            foreach (SupervisedTrainingPattern trainingPattern in trainingSet)
            {
                networkError += CalculateError(trainingPattern);
            }
            return 0.5 * networkError;
        }

        /// <summary>
        /// Calculates the error of the network with respect to a training pattern.
        /// </summary>
        /// <param name="trainingPattern">The training pattern.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        public double CalculateError(SupervisedTrainingPattern trainingPattern)
        {
            double[] outputVector = Evaluate(trainingPattern.InputVector);
            double[] desiredOutputVector = trainingPattern.OutputVector;
            
            double networkError = 0.0;
            for (int i = 0; i < outputVector.Length; i++)
            {
                networkError += Math.Pow(outputVector[i] - desiredOutputVector[i], 2);
            }

            return networkError;
        }
 
        /// <summary>
        /// Saves the weights of the network to a file.
        /// </summary>
        /// 
        /// <param name="fileName">The name of the file to save the weights to.</param>
        public void SaveWeights(string fileName)
        {
            // Open the file for writing.
            TextWriter fileWriter = new StreamWriter(fileName);

            // Write the line containing the numbers of neurons in the layers.
            StringBuilder lineSB = new StringBuilder();
            lineSB.Append(_inputLayer.NeuronCount + " ");
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                lineSB.Append(hiddenLayer.NeuronCount + " ");
            }
            lineSB.Append(_outputLayer.NeuronCount);
            string line = lineSB.ToString();

            fileWriter.WriteLine(line);

            // Write the blank line.
            fileWriter.WriteLine();

            // 1. Save the weights of source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    lineSB = new StringBuilder();
                    for (int i = 0; i < hiddenNeuron.SourceSynapses.Count; i++)
                    {
                        lineSB.Append(hiddenNeuron.SourceSynapses[i].Weight + " ");
                    }
                    if (hiddenNeuron.SourceSynapses.Count != 0)
                    {
                        lineSB.Remove(lineSB.Length - 1, 1);
                    }
                    line = lineSB.ToString();

                    fileWriter.WriteLine(line);
                }

                // Write the blank line.
                fileWriter.WriteLine();
            }

            // 2. Save the weights of source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in _outputLayer.Neurons)
            {
                lineSB = new StringBuilder();
                for (int i = 0; i < outputNeuron.SourceSynapses.Count; i++)
                {
                    lineSB.Append(outputNeuron.SourceSynapses[i].Weight + " ");
                }
                if (outputNeuron.SourceSynapses.Count != 0)
                {
                    lineSB.Remove(lineSB.Length - 1, 1);
                }
                line = lineSB.ToString();

                fileWriter.WriteLine(line);
            }

            // Close the weights file.
            fileWriter.Close();
        }

        /// <summary>
        /// Loads the weights of the network from a file.
        /// </summary>
        /// 
        /// <param name="fileName">The name of the file to load the weights from.</param>
        public void LoadWeights(string fileName)
        {
            // Open the weights file for reading.
            TextReader fileReader = new StreamReader(fileName);
            const char separator = ' ';

            // Read the line containing the numbers of neurons in the layers.
            string line = fileReader.ReadLine();
            string[] words = line.Split(separator);

            // Read the blank line.
            fileReader.ReadLine();

            //
            // 1. Load the weights of the hidden neurons.
            //

            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    line = fileReader.ReadLine();
                    words = line.Split(separator);

                    for (int i = 0; i < hiddenNeuron.SourceSynapses.Count; i++)
                    {
                        hiddenNeuron.SourceSynapses[i].Weight = Double.Parse(words[i]);
                    }
                }

                // Read the blank line.
                fileReader.ReadLine();
            }

            //
            // 2. Load the weights of the output neurons.
            //

            foreach (IActivationNeuron outputNeuron in _outputLayer.Neurons)
            {
                line = fileReader.ReadLine();
                words = line.Split(separator);

                for (int i = 0; i < outputNeuron.SourceSynapses.Count; i++)
                {
                    outputNeuron.SourceSynapses[i].Weight = Double.Parse(words[i]);
                }
            }

            // Close the weights file.
            fileReader.Close();
        }

        /// <summary>
        /// Gets the weights of the network (as an array).
        /// </summary>
        /// <returns>
        /// The weights of the network (as an array).
        /// </returns>
        public double[] GetWeights()
        {
            List<double> weights = new List<double>();

            // Get the weights of the source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    foreach (ISynapse sourceSynapse in hiddenNeuron.SourceSynapses)
                    {
                        weights.Add(sourceSynapse.Weight);
                    }
                }
            }

            // Get the weights of the source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in _outputLayer.Neurons)
            {
                foreach (ISynapse sourceSynapse in outputNeuron.SourceSynapses)
                {
                    weights.Add(sourceSynapse.Weight);
                }
            }

            return weights.ToArray();
        }

        /// <summary>
        /// Sets the weights of the network (as an  array).
        /// </summary>
        /// <param name="weights">The weights of the network (as an array).</param>
        public void SetWeights(double[] weights)
        {
            int i = 0;

            // Set the weights of the source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    foreach (ISynapse sourceSynapse in hiddenNeuron.SourceSynapses)
                    {
                        sourceSynapse.Weight = weights[i++];
                    }
                }
            }

            // Set the weights of the source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in _outputLayer.Neurons)
            {
                foreach (ISynapse sourceSynapse in outputNeuron.SourceSynapses)
                {
                    sourceSynapse.Weight = weights[i++];
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the network.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the network.
        /// </returns>
        public override string ToString()
        {
            StringBuilder networkSB = new StringBuilder();
            networkSB.Append("MLP\n[\n");
            int layerIndex = 0;

            // The input layer.
            networkSB.Append(layerIndex++ + " : " + _inputLayer + "\n");

            // The hidden layers.
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                networkSB.Append(layerIndex++ + " : " + hiddenLayer + "\n");
            }

            // The output layer.
            networkSB.Append(layerIndex + " : " + _outputLayer + "\n");

            networkSB.Append("]");
            return networkSB.ToString();
        }

        /// <summary>
        /// Gets the network blueprint.
        /// </summary>
        /// 
        /// <value>
        /// The network blueprint.
        /// </value>
        public NetworkBlueprint Blueprint
        {
            get
            {
                return _blueprint;
            }
        }

        /// <summary>
        /// Gets the bias layer.
        /// </summary>
        /// 
        /// <value>
        /// The bias layer.
        /// </value>
        public InputLayer BiasLayer
        {
            get
            {
                return _biasLayer;
            }
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// 
        /// <value>
        /// The input layer.
        /// </value>
        public InputLayer InputLayer
        {
            get
            {
                return _inputLayer;
            }
        }

        /// <summary>
        /// Gets the layers.
        /// </summary>
        /// 
        /// <value>
        /// The layers.
        /// </value>
        public List<IActivationLayer> HiddenLayers
        {
            get
            {
                return _hiddenLayers;
            }
        }

        /// <summary>
        /// Gets the number of hidden layers.
        /// </summary>
        /// 
        /// <value>
        /// The numner of hidden layers.
        /// </value>
        public int HiddenLayerCount
        {
            get
            {
                return _hiddenLayers.Count;
            }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// 
        /// <value>
        /// The output layer.
        /// </value>
        public IActivationLayer OutputLayer
        {
            get
            {
                return _outputLayer;
            }
            set
            {
                _outputLayer = value;
            }
        }

        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        ///
        /// <value>
        /// The number of layers.
        /// </value>
        public int LayerCount
        {
            get
            {
                return 1 + _hiddenLayers.Count + 1;
            }
        }

        /// <summary>
        /// Gets the list of connectors comrprising this network.
        /// </summary>
        /// 
        /// <value>
        /// The list of connectors comrprising this network.
        /// </value>
        public List<IConnector> Connectors
        {
            get
            {
                return _connectors;
            }
        }

        /// <summary>
        /// Gets the number of connectors in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of connectors in this network.
        /// </value>
        public int ConnectorCount
        {
            get
            {
                return _connectors.Count;
            }
        }

        /// <summary>
        /// Gets the number of synapses in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of synapses in this network.
        /// </value>
        public int SynapseCount
        {
            get
            {
                int synapseCount = 0;
                foreach (IConnector connector in _connectors)
                {
                    synapseCount += connector.SynapseCount;
                }
                return synapseCount;
            }
        }

        /// <summary>
        /// The blueprint of the network.
        /// </summary>
        private NetworkBlueprint _blueprint;

        /// <summary>
        /// The bias layer of the network.
        /// </summary>
        private InputLayer _biasLayer;

        /// <summary>
        /// The input layer of the network.
        /// </summary>
        private InputLayer _inputLayer;

        /// <summary>
        /// The hidden layers of the network.
        /// </summary>
        private List<IActivationLayer> _hiddenLayers;

        /// <summary>
        /// The output layer of the network.
        /// </summary>
        private IActivationLayer _outputLayer;

        /// <summary>
        /// The connectors of the network.
        /// </summary>
        private List<IConnector> _connectors;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVector"></param>
        private void SetInputVector(double[] inputVector)
        {            
            _inputLayer.SetInputVector(inputVector);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Evaluate()
        {
            // Evaluate the hidden layers.
            foreach (IActivationLayer hiddenLayer in _hiddenLayers)
            {
                hiddenLayer.Evaluate();
            }

            // Evaluate the output layer.
            _outputLayer.Evaluate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double[] GetOutputVector()
        {
            return _outputLayer.GetOutputVector();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jitterNoiseLimit"></param>
        private void Jitter(double jitterNoiseLimit)
        {
            foreach (Connector connector in _connectors)
            {
                connector.Jitter(jitterNoiseLimit);
            }
        }
    }
}
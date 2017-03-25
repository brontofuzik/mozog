using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Neurons;
using NeuralNetwork.MultilayerPerceptron.Synapses;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    public class Network : INetwork
    {
        public Network(NetworkBlueprint blueprint)
        {
            // 0. Validate the blueprint.
            Require.IsNotNull(blueprint, "blueprint");
            Blueprint = blueprint;

            // 1. Create the network components.
            // 1.1. Create the layers.
            // 1.1.1. Create the bias layer.
            BiasLayer = new InputLayer(Blueprint.BiasLayerBlueprint, this);
            BiasLayer.SetInputVector(new[] { 1.0 });

            // 1.1.2. Create the input layer.
            InputLayer = new InputLayer(Blueprint.InputLayerBlueprint, this);

            // 1.1.3. Create the hidden layers.
            HiddenLayers = new List< IActivationLayer >(Blueprint.HiddenLayerCount);
            foreach (ActivationLayerBlueprint hiddenLayerBlueprint in Blueprint.HiddenLayerBlueprints)
            {
                IActivationLayer hiddenLayer = new ActivationLayer(hiddenLayerBlueprint, this);
                HiddenLayers.Add(hiddenLayer);
            }

            // 1.1.4. Create the output layer.
            OutputLayer = new ActivationLayer(Blueprint.OutputLayerBlueprint, this);

            // 1.2 Create the connectors.
            Connectors = new List< IConnector >(Blueprint.ConnectorCount);
            foreach (ConnectorBlueprint connectorBlueprint in Blueprint.ConnectorBlueprints)
            {
                IConnector connector = new Connector(connectorBlueprint, this);
                Connectors.Add(connector);
            }

            // 2. Connect the network.
            Connect();
        }

        public NetworkBlueprint Blueprint { get; }

        public InputLayer BiasLayer { get; }

        public InputLayer InputLayer { get; }

        public List<IActivationLayer> HiddenLayers { get; }

        public int HiddenLayerCount => HiddenLayers.Count;

        public IActivationLayer OutputLayer { get; set; }

        public int LayerCount => 1 + HiddenLayers.Count + 1;

        public List<IConnector> Connectors { get; }

        public int ConnectorCount => Connectors.Count;

        public int SynapseCount => Connectors.Sum(c => c.SynapseCount);

        /// <summary>
        /// A network is connected if:
        /// 1. its connectors are connected.
        /// </summary>
        public void Connect()
        {
            Connectors.ForEach(c => c.Connect());
        }

        /// <summary>
        /// A network is disconnected if:
        /// 1. its connectors are disconnected.
        /// </summary>
        public void Disconnect()
        {
            Connectors.ForEach(c => c.Disconnect());
        }

        public ILayer GetLayerByIndex(int layerIndex)
        {
            switch (layerIndex)
            {
                case -1:
                    return BiasLayer;
                case 0:
                    return InputLayer;
                default:
                    return layerIndex != LayerCount - 1 ? HiddenLayers[layerIndex - 1] : OutputLayer;
            }
        }

        public void Initialize()
        {
            BiasLayer.Initialize();
            InputLayer.Initialize();
            HiddenLayers.ForEach(l => l.Initialize());
            OutputLayer.Initialize();
            Connectors.ForEach(c => c.Initialize());
        }

        public double[] Evaluate(double[] inputVector)
        {
            SetInputVector(inputVector);
            Evaluate();
            return GetOutputVector();
        }

        public double CalculateError(TrainingSet trainingSet)
        {
            double networkError = 0.0;
            foreach (SupervisedTrainingPattern trainingPattern in trainingSet)
            {
                networkError += CalculateError(trainingPattern);
            }
            return 0.5 * networkError;
        }

        public double CalculateError(SupervisedTrainingPattern trainingPattern)
        {
            double[] outputVector = Evaluate(trainingPattern.InputVector);
            double[] desiredOutputVector = trainingPattern.OutputVector;

            // Ref
            return outputVector.Select((o, i) => Math.Pow(o - desiredOutputVector[i], 2)).Sum();
        }
 
        public void SaveWeights(string fileName)
        {
            // Open the file for writing.
            TextWriter fileWriter = new StreamWriter(fileName);

            // Write the line containing the numbers of neurons in the layers.
            StringBuilder lineSB = new StringBuilder();
            lineSB.Append(InputLayer.NeuronCount + " ");
            foreach (IActivationLayer hiddenLayer in HiddenLayers)
            {
                lineSB.Append(hiddenLayer.NeuronCount + " ");
            }
            lineSB.Append(OutputLayer.NeuronCount);
            string line = lineSB.ToString();

            fileWriter.WriteLine(line);

            // Write the blank line.
            fileWriter.WriteLine();

            // 1. Save the weights of source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in HiddenLayers)
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
            foreach (IActivationNeuron outputNeuron in OutputLayer.Neurons)
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

            foreach (IActivationLayer hiddenLayer in HiddenLayers)
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

            foreach (IActivationNeuron outputNeuron in OutputLayer.Neurons)
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

        public double[] GetWeights()
        {
            List<double> weights = new List<double>();

            // Hidden neurons
            foreach (IActivationLayer hiddenLayer in HiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    foreach (ISynapse sourceSynapse in hiddenNeuron.SourceSynapses)
                    {
                        weights.Add(sourceSynapse.Weight);
                    }
                }
            }

            // Output neurons
            foreach (IActivationNeuron outputNeuron in OutputLayer.Neurons)
            {
                foreach (ISynapse sourceSynapse in outputNeuron.SourceSynapses)
                {
                    weights.Add(sourceSynapse.Weight);
                }
            }

            return weights.ToArray();
        }

        public void SetWeights(double[] weights)
        {
            int i = 0;

            // Hidden neurons
            foreach (IActivationLayer hiddenLayer in HiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    foreach (ISynapse sourceSynapse in hiddenNeuron.SourceSynapses)
                    {
                        sourceSynapse.Weight = weights[i++];
                    }
                }
            }

            // Output neurons
            foreach (IActivationNeuron outputNeuron in OutputLayer.Neurons)
            {
                foreach (ISynapse sourceSynapse in outputNeuron.SourceSynapses)
                {
                    sourceSynapse.Weight = weights[i++];
                }
            }
        }

        // TODO
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("MLP\n[\n");

            // The input layer.
            int layerIndex = 0;
            sb.Append(layerIndex++ + " : " + InputLayer + "\n");

            // The hidden layers.
            foreach (IActivationLayer hiddenLayer in HiddenLayers)
            {
                sb.Append(layerIndex++ + " : " + hiddenLayer + "\n");
            }

            // The output layer.
            sb.Append(layerIndex + " : " + OutputLayer + "\n]");

            return sb.ToString();
        }

        private void SetInputVector(double[] inputVector)
        {            
            InputLayer.SetInputVector(inputVector);
        }

        private void Evaluate()
        {
            HiddenLayers.ForEach(l => l.Evaluate());
            OutputLayer.Evaluate();
        }

        private double[] GetOutputVector() => OutputLayer.GetOutputVector();

        private void Jitter(double jitterNoiseLimit)
        {
            Connectors.ForEach(c => c.Jitter(jitterNoiseLimit));
        }
    }
}
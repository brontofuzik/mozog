using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron.ActivationFunctions;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Network : INetwork
    {
        public Network(int inputLayerNeurons, params (int neurons, IActivationFunction activation)[] activationLayers)
        {
            Construct(inputLayerNeurons, activationLayers);
        }

        public Network(int[] topology, IActivationFunction activation)
        {
            Construct(topology[0], topology.Skip(1).Select(n => (n, activation)));
        }

        // Factory
        internal Network()
        {
        }

        private void Construct(int inputLayerNeurons, IEnumerable<(int neurons, IActivationFunction activation)> activationLayers)
        {
            // Layers
            BiasLayer = new InputLayer(1, this);
            BiasLayer.SetOutputVector(new[] { 1.0 }); // TODO Move

            Layers.Add(new InputLayer(inputLayerNeurons, this));
            Layers.AddRange(activationLayers.Select(l => new ActivationLayer(l.neurons, l.activation, this)));

            // Connectors
            for (int l = 1; l < LayerCount; l++)
            {
                var connector = new Connector(Layers[l - 1], (IActivationLayer)Layers[l], this);
                connector.Connect();
                Connectors.Add(connector);

                connector = new Connector(BiasLayer, (IActivationLayer)Layers[l], this);
                connector.Connect();
                Connectors.Add(connector);
            }
        }

        #region Layers

        public InputLayer BiasLayer { get; private set; }

        public List<ILayer> Layers { get; } = new List<ILayer>();

        public int LayerCount => Layers.Count;

        public IEnumerable<ActivationLayer> ActivationLayers
        {
            get
            {
                for (int l = 1; l < Layers.Count; l++)
                {
                    yield return (ActivationLayer)Layers[l];
                }
            }
        }

        public InputLayer InputLayer => (InputLayer)Layers[0];

        public ActivationLayer OutputLayer => (ActivationLayer)Layers[Layers.Count - 1];

        #endregion // Layers

        #region Connectors

        public List<IConnector> Connectors { get; } = new List<IConnector>();

        public int ConnectorCount => Connectors.Count;

        public int SynapseCount => Connectors.Sum(c => c.SynapseCount);

        #endregion // Connectors

        public void Initialize()
        {
            Layers.ForEach(l => l.Initialize());
            Connectors.ForEach(c => c.Initialize());
        }

        public double[] Evaluate(double[] inputVector)
        {
            SetInputVector(inputVector);
            Evaluate();
            return GetOutputVector();
        }

        private void SetInputVector(double[] inputVector)
        {
            InputLayer.SetOutputVector(inputVector);
        }

        private void Evaluate()
        {
            ActivationLayers.ForEach(l => l.Evaluate());
            OutputLayer.Evaluate();
        }

        private double[] GetOutputVector() => OutputLayer.GetOutputVector();

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

        #region Serialization

        public void SaveWeights(string fileName)
        {
            // Open the file for writing.
            TextWriter fileWriter = new StreamWriter(fileName);

            // Write the line containing the numbers of neurons in the layers.
            StringBuilder lineSB = new StringBuilder();
            lineSB.Append(InputLayer.NeuronCount + " ");
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
            {
                lineSB.Append(hiddenLayer.NeuronCount + " ");
            }
            lineSB.Append(OutputLayer.NeuronCount);
            string line = lineSB.ToString();

            fileWriter.WriteLine(line);

            // Write the blank line.
            fileWriter.WriteLine();

            // 1. Save the weights of source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
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

            foreach (IActivationLayer hiddenLayer in ActivationLayers)
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
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
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
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
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

        #endregion // Serialization

        // TODO
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("MLP\n[\n");

            // The input layer.
            int layerIndex = 0;
            sb.Append(layerIndex++ + " : " + InputLayer + "\n");

            // The hidden layers.
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
            {
                sb.Append(layerIndex++ + " : " + hiddenLayer + "\n");
            }

            // The output layer.
            sb.Append(layerIndex + " : " + OutputLayer + "\n]");

            return sb.ToString();
        }

        private void Jitter(double jitterNoiseLimit)
        {
            Connectors.ForEach(c => c.Jitter(jitterNoiseLimit));
        }
    }
}
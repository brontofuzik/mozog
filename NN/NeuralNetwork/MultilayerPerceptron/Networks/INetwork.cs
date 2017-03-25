using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    public interface INetwork
    {
        NetworkBlueprint Blueprint { get; }

        InputLayer BiasLayer { get; }

        InputLayer InputLayer { get; }

        List<IActivationLayer> HiddenLayers { get; }

        int HiddenLayerCount { get; }

        IActivationLayer OutputLayer { get; set; }

        int LayerCount { get; }

        List<IConnector> Connectors { get; }

        int ConnectorCount { get; }

        int SynapseCount { get; }

        /// <summary>
        /// A network is connected if:
        /// 1. its layers are connected and
        /// 2. its connectors are connected.
        /// </summary>
        void Connect();

        /// <summary>
        /// A network is disconnected if:
        /// 1. its layers are disconnected and
        /// 2. its connectors are disconnected.
        /// </summary>
        void Disconnect();

        ILayer GetLayerByIndex(int layerIndex);

        void Initialize();

        double[] Evaluate(double[] inputVector);

        double CalculateError(TrainingSet trainingSet);

        double CalculateError(SupervisedTrainingPattern trainingPattern);

        void SaveWeights(string weightFileName);

        void LoadWeights(string weightFileName);

        double[] GetWeights();

        void SetWeights(double[] weights);
    }
}

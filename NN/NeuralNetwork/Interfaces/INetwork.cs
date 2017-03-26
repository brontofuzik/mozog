using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.Training;

namespace NeuralNetwork.Interfaces
{
    public interface INetwork
    {
        List<ILayer> Layers { get; }

        int LayerCount { get; }

        List<IConnector> Connectors { get; }

        int ConnectorCount { get; }

        int SynapseCount { get; }

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

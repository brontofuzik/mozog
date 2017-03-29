using System.Collections.Generic;
using NeuralNetwork.Training;

namespace NeuralNetwork.Interfaces
{
    public interface INetwork
    {
        NetworkArchitecture Architecture { get; }

        List<ILayer> Layers { get; }

        int LayerCount { get; }

        //int NeuronCount { get; }

        int SynapseCount { get; }

        void Initialize();

        double[] Evaluate(double[] inputVector);

        double CalculateError(TrainingSet trainingSet);

        double CalculateError(SupervisedTrainingPattern trainingPattern);

        double[] GetWeights();

        void SetWeights(double[] weights);
    }
}

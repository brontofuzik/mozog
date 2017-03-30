using System.Collections.Generic;
using NeuralNetwork.Training;

namespace NeuralNetwork.Interfaces
{
    public interface INetwork
    {
        INetworkArchitecture Architecture { get; }

        List<ILayer> Layers { get; }

        int LayerCount { get; }

        //int NeuronCount { get; }

        int SynapseCount { get; }

        void Initialize();

        double[] Evaluate(double[] inputVector);

        double CalculateError(DataSet dataSet);

        double CalculateError(LabeledDataPoint point);

        double[] GetWeights();

        void SetWeights(double[] weights);
    }
}

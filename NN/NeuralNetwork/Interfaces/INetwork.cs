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

        double[] Evaluate(double[] input);

        TOutput EvaluateEncoded<TInput, TOutput>(TInput input, IEncoder<TInput, TOutput> encoder);

        double CalculateError(DataSet dataSet);

        double CalculateError(LabeledDataPoint dataPoint);

        double[] GetWeights();

        void SetWeights(double[] weights);
    }
}

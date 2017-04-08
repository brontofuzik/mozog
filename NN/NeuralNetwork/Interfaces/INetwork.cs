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

        (double[] output, double error) Evaluate(double[] input, double[] target);

        TOutput EvaluateEncoded<TInput, TOutput>(TInput input, IEncoder<TInput, TOutput> encoder);

        (TOutput output, double error) EvaluateEncoded<TInput, TOutput>(TInput input, TOutput target, IEncoder<TInput, TOutput> encoder);

        double CalculateError(DataSet dataSet);

        double CalculateError(LabeledDataPoint dataPoint);

        double[] GetWeights();

        void SetWeights(double[] weights);
    }
}

using System.Collections.Generic;
using NeuralNetwork.Training;

namespace NeuralNetwork.Interfaces
{
    public interface INetwork
    {
        INetworkArchitecture Architecture { get; }

        List<ILayer> Layers { get; }

        // Only activation layers. Not used.
        int LayerCount { get; }

        // Only activation neurons. Not used.
        int NeuronCount { get; }

        int SynapseCount { get; }

        (double[] output, double error) EvaluateLabeled(double[] input, double[] target);

        (double[] output, double error) EvaluateLabeled(ILabeledDataPoint point);

        // Encoded
        (TOutput output, double error) EvaluateLabeled<TInput, TOutput>(TInput input, TOutput target, IEncoder<TInput, TOutput> encoder);

        double[] EvaluateUnlabeled(double[] input);

        double[] EvaluateUnlabeled(IDataPoint point);

        // Encoded
        TOutput EvaluateUnlabeled<TInput, TOutput>(TInput input, IEncoder<TInput, TOutput> encoder);

        double CalculateError(DataSet dataSet);

        double CalculateError(ILabeledDataPoint dataPoint);

        double[] GetWeights();

        void SetWeights(double[] weights);
    }
}

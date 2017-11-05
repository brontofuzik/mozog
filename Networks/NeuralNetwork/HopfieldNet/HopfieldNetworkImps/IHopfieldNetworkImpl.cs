namespace NeuralNetwork.HopfieldNet.HopfieldNetworkImps
{
    public interface IHopfieldNetworkImpl
    {
        int NeuronCount { get; }

        int SynapseCount { get; }

        double Energy { get; }

        double GetNeuronBias(int neuronIndex);

        void SetNeuronBias(int neuronIndex, double bias);

        double GetSynapseWeight(int neuron1Index, int neuron2Index);

        void SetSynapseWeight(int neuron1Index, int neuron2Index, double weight);

        void SetNetworkInput(double[] input);

        void Evaluate(double progress);

        double[] GetNetworkOutput();
    }
}

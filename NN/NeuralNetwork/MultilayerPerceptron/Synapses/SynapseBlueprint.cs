namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    public class SynapseBlueprint
    {
        public SynapseBlueprint(int sourceNeuronIndex, int targetNeuronIndex)
        {
            SourceNeuronIndex = sourceNeuronIndex;
            TargetNeuronIndex = targetNeuronIndex;
        }

        public int SourceNeuronIndex { get; }

        public int TargetNeuronIndex { get; }
    }
}

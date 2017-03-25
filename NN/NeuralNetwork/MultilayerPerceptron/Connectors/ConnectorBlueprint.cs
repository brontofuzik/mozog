using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    public class ConnectorBlueprint
    {
        public ConnectorBlueprint(int sourceLayerIndex, int sourceLayerNeuronCount, int targetLayerIndex, int targetLayerNeuronCount)
        {
            // 1.
            SourceLayerIndex = sourceLayerIndex;
            TargetLayerIndex = targetLayerIndex;

            // 2. Create the synapse blueprints.
            SynapseBlueprints = new SynapseBlueprint[sourceLayerNeuronCount * targetLayerNeuronCount];

            int i = 0;
            for (int targetNeuronIndex = 0; targetNeuronIndex < targetLayerNeuronCount; targetNeuronIndex++)
            {
                for (int sourceNeuronIndex = 0; sourceNeuronIndex < sourceLayerNeuronCount; sourceNeuronIndex++)
                {
                    // 2.1. Create the synpase blueprint between the source neuron and the target neuron.
                    SynapseBlueprints[i++] = new SynapseBlueprint(sourceNeuronIndex, targetNeuronIndex);
                }
            }
        }

        public SynapseBlueprint[] SynapseBlueprints { get; }

        public int SynapseCount => SynapseBlueprints.Length;

        public int SourceLayerIndex { get; }

        public int TargetLayerIndex { get; }
    }
}
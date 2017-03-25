namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    public class LayerBlueprint
    {
        public LayerBlueprint(int neuronCount)
        {
            NeuronCount = neuronCount;
        }

        public int NeuronCount { get; }

        public override string ToString() => NeuronCount.ToString();
    }
}

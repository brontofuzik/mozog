namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// A blueprint of a layer.
    /// </summary>
    public class LayerBlueprint
    {
        private int neuronCount;

        public int NeuronCount
        {
            get
            {
                return neuronCount;
            }
        }

        public LayerBlueprint(int neuronCount)
        {
            this.neuronCount = neuronCount;
        }

        /// <summary>
        /// Converts a layer blueprint to its string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the layer blueprint.
        /// </returns>
        public override string ToString()
        {
            return neuronCount.ToString();
        }
    }
}

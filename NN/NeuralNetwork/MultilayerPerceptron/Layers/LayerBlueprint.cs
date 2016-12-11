namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// A blueprint of a layer.
    /// </summary>
    public class LayerBlueprint
    {
        #region Private instance fields

        private int neuronCount;
        
        #endregion // Private instance fields

        #region Public instance properties

        public int NeuronCount
        {
            get
            {
                return neuronCount;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        public LayerBlueprint( int neuronCount )
        {
            this.neuronCount = neuronCount;
        }

        #endregion // Public instance constructors

        #region Public instance methods

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

        #endregion // Public insatnce methods
    }
}

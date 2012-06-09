using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// A bluepring of an actiavtion layer. 
    /// </summary>
    public class ActivationLayerBlueprint
        : LayerBlueprint
    {
        #region Private instance fields

        private IActivationFunction activationFunction;

        #endregion // Private instance fields

        #region Public instance properties

        public IActivationFunction ActivationFunction
        {
            get
            {
                return activationFunction;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        public ActivationLayerBlueprint( int neuronCount, IActivationFunction activationFunction )
            : base( neuronCount )
        {
            this.activationFunction = activationFunction;
        }

        public ActivationLayerBlueprint( int neuronCount )
            : this( neuronCount, new LogisticActivationFunction() )
        {
        }

        #endregion // Public instance constructors
    }
}

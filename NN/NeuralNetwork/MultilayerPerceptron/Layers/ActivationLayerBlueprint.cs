using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// A bluepring of an actiavtion layer. 
    /// </summary>
    public class ActivationLayerBlueprint
        : LayerBlueprint
    {
        private IActivationFunction activationFunction;

        public IActivationFunction ActivationFunction
        {
            get
            {
                return activationFunction;
            }
        }

        public ActivationLayerBlueprint(int neuronCount, IActivationFunction activationFunction)
            : base(neuronCount)
        {
            this.activationFunction = activationFunction;
        }

        public ActivationLayerBlueprint(int neuronCount)
            : this(neuronCount, new LogisticActivationFunction())
        {
        }
    }
}

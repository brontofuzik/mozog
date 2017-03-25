using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    public class ActivationLayerBlueprint : LayerBlueprint
    {
        public ActivationLayerBlueprint(int neuronCount, IActivationFunction activationFunction)
            : base(neuronCount)
        {
            ActivationFunction = activationFunction;
        }

        public ActivationLayerBlueprint(int neuronCount)
            : this(neuronCount, new LogisticActivationFunction())
        {
        }

        public IActivationFunction ActivationFunction { get; }
    }
}

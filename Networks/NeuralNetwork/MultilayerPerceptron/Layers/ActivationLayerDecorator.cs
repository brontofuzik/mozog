using System.Collections.Generic;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /* TODO Remove
    public abstract class ActivationLayerDecorator : IActivationLayer
    {
        protected IActivationLayer decoratedActivationLayer;

        protected ActivationLayerDecorator(IActivationLayer activationLayer, INetwork parentNetwork)
        {
            decoratedActivationLayer = activationLayer;
            ParentNetwork = parentNetwork;
        }

        public virtual List<IActivationNeuron> Neurons => decoratedActivationLayer.Neurons;

        List<INeuron> ILayer.Neurons => (decoratedActivationLayer as ILayer).Neurons;

        public virtual int NeuronCount => decoratedActivationLayer.NeuronCount;

        public virtual IActivationFunction ActivationFunction => decoratedActivationLayer.ActivationFunction;

        public virtual List<IConnector> SourceConnectors => decoratedActivationLayer.SourceConnectors;

        public virtual List<IConnector> TargetConnectors => decoratedActivationLayer.TargetConnectors;

        public virtual INetwork ParentNetwork
        {
            get { return decoratedActivationLayer.ParentNetwork; }
            set { decoratedActivationLayer.ParentNetwork = value; }
        }

        public virtual IActivationLayer GetDecoratedActivationLayer(INetwork parentNetwork)
        {
            // Undecorate the neurons.
            for (int i = 0; i < NeuronCount; i++)
            {
                Neurons[i] = (Neurons[i] as ActivationNeuronDecorator).GetDecoratedActivationNeuron(decoratedActivationLayer);
            }

            // Reintegrate.
            ParentNetwork = parentNetwork;
            return decoratedActivationLayer;
        }

        public virtual INeuron GetNeuronByIndex(int neuronIndex) => decoratedActivationLayer.GetNeuronByIndex(neuronIndex);

        public virtual void Initialize()
        {
            decoratedActivationLayer.Initialize();
        }

        public virtual void Evaluate()
        {
            decoratedActivationLayer.Evaluate();
        }

        public virtual double[] GetOutputVector() => decoratedActivationLayer.GetOutputVector();

        public override string ToString() => decoratedActivationLayer.ToString();
    }
    */
}

using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Layers;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /* TODO Remove
    public abstract class ActivationNeuronDecorator : IActivationNeuron
    {
        protected IActivationNeuron decoratedActivationNeuron;

        protected ActivationNeuronDecorator(IActivationNeuron decoratedActivationNeuron, IActivationLayer parentLayer)
        {
            this.decoratedActivationNeuron = decoratedActivationNeuron;
            ParentLayer = parentLayer;
        }

        public virtual double Input => decoratedActivationNeuron.Input;

        public virtual double Output => decoratedActivationNeuron.Output;

        public virtual List<ISynapse> SourceSynapses => decoratedActivationNeuron.SourceSynapses;

        public virtual List<ISynapse> TargetSynapses => decoratedActivationNeuron.TargetSynapses;

        public virtual ILayer ParentLayer
        {
            get { return decoratedActivationNeuron.ParentLayer; }
            set { decoratedActivationNeuron.ParentLayer = value; }
        }

        public virtual IActivationNeuron GetDecoratedActivationNeuron(IActivationLayer parentLayer)
        {
            // Reintegrate.
            ParentLayer = parentLayer;
            return decoratedActivationNeuron;
        }

        public virtual void Initialize()
        {
            decoratedActivationNeuron.Initialize();
        }

        public virtual void Evaluate()
        {
            decoratedActivationNeuron.Evaluate();
        }

        public override string ToString() => decoratedActivationNeuron.ToString();
    }
    */
}

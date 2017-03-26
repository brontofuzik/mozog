namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    /* TODO Remove
    public abstract class SynapseDecorator : ISynapse
    {
        protected ISynapse decoratedSynapse;

        protected SynapseDecorator(ISynapse decoratedSynapse, IConnector parentConnector)
        {
            this.decoratedSynapse = decoratedSynapse;
            ParentConnector = parentConnector;
        }

        public SynapseBlueprint Blueprint => decoratedSynapse.Blueprint;

        public virtual double Weight
        {
            get { return decoratedSynapse.Weight; }
            set { decoratedSynapse.Weight = value; }
        }

        public virtual INeuron SourceNeuron
        {
            get { return decoratedSynapse.SourceNeuron; }
            set { decoratedSynapse.SourceNeuron = value; }
        }

        public virtual IActivationNeuron TargetNeuron
        {
            get { return decoratedSynapse.TargetNeuron; }
            set { decoratedSynapse.TargetNeuron = value; }
        }

        public virtual IConnector ParentConnector
        {
            get { return decoratedSynapse.ParentConnector; }
            set { decoratedSynapse.ParentConnector = value; }
        }

        public abstract void Connect();

        public virtual void Connect(ISynapse synapse)
        {
            decoratedSynapse.Connect(synapse);
        }

        public abstract void Disconnect();

        public virtual void Disconnect(ISynapse synapse)
        {
            decoratedSynapse.Disconnect(synapse);
        }

        public virtual ISynapse GetDecoratedSynapse(IConnector parentConnector)
        {
            // Reintegrate.
            ParentConnector = parentConnector;
            return decoratedSynapse;
        }

        public virtual void Initialize()
        {
            decoratedSynapse.Initialize();
        }

        public virtual void Jitter(double jitterNoiseLimit)
        {
            decoratedSynapse.Jitter(jitterNoiseLimit);
        }

        public override string ToString() => decoratedSynapse.ToString();
    }
    */
}

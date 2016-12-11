using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActivationNeuronDecorator
        : IActivationNeuron
    {
        #region Protected instance fields

        /// <summary>
        /// 
        /// </summary>
        protected IActivationNeuron decoratedActivationNeuron;

        #endregion // Protected instance fields

        #region Public instance properties

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// 
        /// <value>
        /// The input.
        /// </value>
        public virtual double Input
        {
            get
            {
                return decoratedActivationNeuron.Input;
            }
        }

        /// <summary>
        /// Gets the ouput.
        /// </summary>
        /// 
        /// <value>
        /// The output.
        /// </value>
        public virtual double Output
        {
            get
            {
                return decoratedActivationNeuron.Output;
            }
        }

        /// <summary>
        /// Gets the list of source synapses.
        /// </summary>
        ///
        /// <value>
        /// The list of source synapses.
        /// </value>
        public virtual List<ISynapse> SourceSynapses
        {
            get
            {
                return decoratedActivationNeuron.SourceSynapses;
            }
        }

        /// <summary>
        /// Gets the list of target synapses.
        /// </summary>
        ///
        /// <value>
        /// The list of target synapses.
        /// </value>
        public virtual List<ISynapse> TargetSynapses
        {
            get
            {
                return decoratedActivationNeuron.TargetSynapses;
            }
        }

        /// <summary>
        /// Gets the parent layer.
        /// </summary>
        ///
        /// <value>
        /// The parent layer.
        /// </value>
        public virtual ILayer ParentLayer
        {
            get
            {
                return decoratedActivationNeuron.ParentLayer;
            }
            set
            {
                decoratedActivationNeuron.ParentLayer = value;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoratedActivationNeuron">The (activation) layer to be decorated.</param>
        /// <param name="parentLayer">The parent layer.</param>
        public ActivationNeuronDecorator( IActivationNeuron decoratedActivationNeuron, IActivationLayer parentLayer )
        {
            this.decoratedActivationNeuron = decoratedActivationNeuron;
            ParentLayer = parentLayer;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Returns the decorated (activation) neuron.
        /// </summary>
        /// <param name="parentLayer">The parent layer.</param>
        /// <returns>
        /// The decorated (activation) neuron.
        /// </returns>
        public virtual IActivationNeuron GetDecoratedActivationNeuron(IActivationLayer parentLayer)
        {
            // Reintegrate.
            ParentLayer = parentLayer;

            return decoratedActivationNeuron;
        }

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public virtual void Initialize()
        {
            decoratedActivationNeuron.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Evaluate()
        {
            decoratedActivationNeuron.Evaluate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return decoratedActivationNeuron.ToString();
        }

        #endregion // Public instance methods
    }
}

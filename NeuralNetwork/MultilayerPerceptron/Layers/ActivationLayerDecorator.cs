using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;


namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <remarks>
    /// An activation layer decorator.
    /// </remarks>
    public abstract class ActivationLayerDecorator
        : IActivationLayer
    {
        #region Protected instance fields

        /// <summary>
        /// 
        /// </summary>
        protected IActivationLayer decoratedActivationLayer;

        #endregion // Protected instance fields

        #region Public instance properties

        /// <summary>
        /// 
        /// </summary>
        List<INeuron> ILayer.Neurons
        {
            get
            {
                return (decoratedActivationLayer as ILayer).Neurons;
            }
        }

        /// <summary>
        /// Gets the list of neurons comprising the layer.
        /// </summary>
        ///
        /// <value>
        /// The list of neurons comprising the layer.
        /// </value>
        public virtual List<IActivationNeuron> Neurons
        {
            get
            {
                return decoratedActivationLayer.Neurons;
            }
        }

        /// <summary>
        /// Gets the number of neurons comprising the layer.
        /// </summary>
        /// 
        /// <value>
        /// The number of neurons comprising the layer.
        /// </value>
        public virtual int NeuronCount
        {
            get
            {
                return decoratedActivationLayer.NeuronCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IActivationFunction ActivationFunction
        {
            get
            {
                return decoratedActivationLayer.ActivationFunction;
            }
        }

        /// <summary>
        /// Gets the list of source connectors associated with the layer.
        /// </summary>
        /// 
        /// <value>
        /// The list of source connectors associated with the layer.
        /// </value>
        public virtual List<IConnector> SourceConnectors
        {
            get
            {
                return decoratedActivationLayer.SourceConnectors;
            }
        }

        /// <summary>
        /// Gets the list of target connectors associated with the layer.
        /// </summary>
        /// 
        /// <value>
        /// The list of target connectors associated with the layer.
        /// </value>
        public virtual List<IConnector> TargetConnectors
        {
            get
            {
                return decoratedActivationLayer.TargetConnectors;
            }
        }

        /// <summary>
        /// Gets or sets the parent network.
        /// </summary>
        /// 
        /// <value>
        /// The parent network.
        /// </value>
        public virtual INetwork ParentNetwork
        {
            get
            {
                return decoratedActivationLayer.ParentNetwork;
            }
            set
            {
                decoratedActivationLayer.ParentNetwork = value;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoratedActivationLayer">The (activation) layer to be decorated.</param>
        /// <param name="parentNetwork">The parent network.</param>
        public ActivationLayerDecorator( IActivationLayer activationLayer, INetwork parentNetwork )
        {
            this.decoratedActivationLayer = activationLayer;
            ParentNetwork = parentNetwork;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Returns the decorated activation layer.
        /// </summary>
        /// <param name="parentNetwork">The parent network.</param>
        /// <returns>
        /// The decorated activation layer.
        /// </returns>
        public virtual IActivationLayer GetDecoratedActivationLayer(INetwork parentNetwork)
        {
            // Undecorate the neurons.
            for (int i = 0; i < NeuronCount; i++)
            {
                Neurons[ i ] = (Neurons[ i ] as ActivationNeuronDecorator).GetDecoratedActivationNeuron(decoratedActivationLayer);
            }

            // Reintegrate.
            ParentNetwork = parentNetwork;

            return decoratedActivationLayer;
        }

        /// <summary>
        /// Gets a neuron (specified by its index within the layer).
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>
        /// The neuron.
        /// </returns>
        public virtual INeuron GetNeuronByIndex(int neuronIndex)
        {
            return decoratedActivationLayer.GetNeuronByIndex(neuronIndex);
        }

        /// <summary>
        /// Initializes the layer.
        /// </summary>
        public virtual void Initialize()
        {
            decoratedActivationLayer.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Evaluate()
        {
            decoratedActivationLayer.Evaluate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual double[] GetOutputVector()
        {
            return decoratedActivationLayer.GetOutputVector();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return decoratedActivationLayer.ToString();
        }

        #endregion // Public instance methods
    }
}

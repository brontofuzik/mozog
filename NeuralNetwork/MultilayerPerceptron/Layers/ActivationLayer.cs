using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <remarks>
    /// An activation layer.
    /// </remarks>
    public class ActivationLayer
        : IActivationLayer
    {
        #region Private instance fields

        /// <summary>
        /// The neurons of the layer.
        /// </summary>
        private List<IActivationNeuron> neurons;

        /// <summary>
        /// 
        /// </summary>
        private IActivationFunction activationFunction;

        /// <summary>
        /// The source connector of the layer.
        /// </summary>
        private List<IConnector> sourceConnectors;

        /// <summary>
        /// The target connector of the layer.
        /// </summary>
        private List<IConnector> targetConnectors;

        /// <summary>
        /// The parent network of the layer.
        /// </summary>
        private INetwork parentNetwork;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// 
        /// </summary>
        List<INeuron> ILayer.Neurons
        {
            get
            {
                List<INeuron> iNeurons = new List<INeuron>(neurons.Count);
                foreach (INeuron neuron in neurons)
                {
                    iNeurons.Add(neuron);
                }
                return iNeurons;
            }
        }

        /// <summary>
        /// Gets the list of neurons comprising the layer.
        /// </summary>
        ///
        /// <value>
        /// The list of neurons comprising the layer.
        /// </value>
        public List< IActivationNeuron > Neurons
        {
            get
            {
                return neurons;
            }
        }

        /// <summary>
        /// The layer indexer.
        /// </summary>
        ///
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        ///
        /// <returns>
        /// The neuron at the given index.
        /// </returns>
        /// 
        /// <exception cref="IndexOutOfRangeException">
        /// Condition: <c>sourceNeuronIndex</c> is out of range.
        /// </exception>
        public INeuron this[ int neuronIndex ]
        {
            get
            {
                return neurons[ neuronIndex ];
            }
        }

        /// <summary>
        /// Gets the number of neurons comprising the layer.
        /// </summary>
        /// 
        /// <value>
        /// The number of neurons comprising the layer.
        /// </value>
        public int NeuronCount
        {
            get
            {
                return neurons.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IActivationFunction ActivationFunction
        {
            get
            {
                return activationFunction;
            }
        }

        /// <summary>
        /// Gets the source connector.
        /// </summary>
        /// 
        /// <value>
        /// The source connector.
        /// </value>
        public List<IConnector> SourceConnectors
        {
            get
            {
                return sourceConnectors;
            }
        }

        /// <summary>
        /// Gets the target connector.
        /// </summary>
        /// 
        /// <value>
        /// The target connector.
        /// </value>
        public List<IConnector> TargetConnectors
        {
            get
            {
                return targetConnectors;
            }
        }

        /// <summary>
        /// Gets or sets the parent network.
        /// </summary>
        /// 
        /// <value>
        /// The parent network.
        /// </value>
        public INetwork ParentNetwork
        {
            get
            {
                return parentNetwork;
            }
            set
            {
                parentNetwork = value;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new activation layer.
        /// </summary>
        /// 
        /// <param name="neuronCount">The number of (activation) neurons.</param>
        /// <param name="activationFunction">The activation function</param>
        ///<param name="parentNetwork">The parnet network.</param>
        public ActivationLayer( ActivationLayerBlueprint blueprint, INetwork parentNetwork )
        {
            // Create the neurons.
            neurons = new List< IActivationNeuron >( blueprint.NeuronCount );
            for (int i = 0; i < blueprint.NeuronCount; i++)
            {
                IActivationNeuron neuron = new ActivationNeuron( this );
                neurons.Add( neuron );
            }

            sourceConnectors = new List< IConnector >();
            targetConnectors = new List< IConnector >();

            // Validate the activation function.
            Utilities.RequireObjectNotNull( blueprint.ActivationFunction, "activationFunction" );
            this.activationFunction = blueprint.ActivationFunction;

            // Validate the parent network.
            Utilities.RequireObjectNotNull(parentNetwork, "parentNetwork");
            this.parentNetwork = parentNetwork;
        }

        #endregion // Public instance constructors   

        #region Public instance methods

        /// <summary>
        /// Gets a neuron (specified by its index within the layer).
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>
        /// The neuron.
        /// </returns>
        public INeuron GetNeuronByIndex(int neuronIndex)
        {
            return neurons[neuronIndex];
        }

        /// <summary>
        /// Initializes the layer.
        /// </summary>
        public void Initialize()
        {
            foreach (IActivationNeuron hiddenNeuron in neurons)
            {
                hiddenNeuron.Initialize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Evaluate()
        {
            foreach (IActivationNeuron hiddenNeuron in neurons)
            {
                hiddenNeuron.Evaluate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double[] GetOutputVector()
        {
            double[] outputVector = new double[NeuronCount];

            for (int i = 0; i < NeuronCount; i++)
            {
                outputVector[i] = neurons[i].Output;
            }

            return outputVector;
        }

        /// <summary>
        /// Returns a string representation of the activation layer.
        /// </summary>
        /// <returns>
        /// A string representation of the activation layer.
        /// </returns>
        public override string ToString()
        {
            StringBuilder activationLayerSB = new StringBuilder();

            activationLayerSB.Append( "AL\n[\n" );
            int neuronIndex = 0;
            foreach (IActivationNeuron neuron in neurons)
            {
                activationLayerSB.Append( "  " + neuronIndex++ + " : " + neuron + "\n" );
            }
            activationLayerSB.Append( "]" );

            return activationLayerSB.ToString();
        }

        #endregion // Public instance methods
    }
}
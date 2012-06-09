using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// A hidden neuron.
    /// </summary>
    public class ActivationNeuron
        : IActivationNeuron
    {
        #region Private instance fields

        /// <summary>
        /// The input (or inner potential).
        /// </summary>
        private double input;

        /// <summary>
        /// The output (or state).
        /// </summary>
        private double output;

        /// <summary>
        /// The source synapses.
        /// </summary>
        private List< ISynapse > sourceSynapses;

        /// <summary>
        /// The target synapses.
        /// </summary>
        private List< ISynapse > targetSynapses;

        /// <summary>
        /// The parent layer.
        /// </summary>
        private IActivationLayer parentLayer;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// 
        /// <value>
        /// The input.
        /// </value>
        public double Input
        {
            get
            {
                return input;
            }
        }

        /// <summary>
        /// Gets the ouput.
        /// </summary>
        /// 
        /// <value>
        /// The output.
        /// </value>
        public double Output
        {
            get
            {
                return output;
            }
        }

        /// <summary>
        /// Gets the list of source synapses.
        /// </summary>
        ///
        /// <value>
        /// The list of source synapses.
        /// </value>
        public List< ISynapse > SourceSynapses
        {
            get
            {
                return sourceSynapses;
            }
        }

        /// <summary>
        /// Gets the list of target synapses.
        /// </summary>
        ///
        /// <value>
        /// The list of target synapses.
        /// </value>
        public List< ISynapse > TargetSynapses
        {
            get
            {
                return targetSynapses;
            }
        }

        /// <summary>
        /// Gets or sets the parent layer.
        /// </summary>
        ///
        /// <value>
        /// The parent layer.
        /// </value>
        public ILayer ParentLayer
        {
            get
            {
                return parentLayer;
            }
            set
            {
                parentLayer = value as IActivationLayer;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new hidden neuron.
        /// </summary>
        /// <param name="parentLayer">The parnet layer.</param>
        public ActivationNeuron(IActivationLayer parentLayer)
        {
            sourceSynapses = new List<ISynapse>();
            targetSynapses = new List<ISynapse>();

            // Validate the parent layer.
            Utilities.RequireObjectNotNull(parentLayer, "parentLayer");
            this.parentLayer = parentLayer;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public void Initialize()
        {
            input = 0.0;
            output = 0.0;
        }

        /// <summary>
        /// Evaluates the neuron.
        /// </summary>
        public virtual void Evaluate()
        {
            input = 0.0;

            foreach (ISynapse sourceSynapse in SourceSynapses)
            {
                input += sourceSynapse.SourceNeuron.Output * sourceSynapse.Weight;
            }

            output = (parentLayer as IActivationLayer).ActivationFunction.Evaluate( input );
        }

        /// <summary>
        /// Returns a string representation of the activation neuron.
        /// </summary>
        /// <returns>
        /// A string representation of the activation neuron.
        /// </returns>
        public override string ToString()
        {
            StringBuilder activationNeuronSB = new StringBuilder();

            activationNeuronSB.Append( "AN([" );
            foreach (ISynapse synapse in sourceSynapses)
            {
                activationNeuronSB.Append( synapse + ", " );
            }
            if (sourceSynapses.Count != 0)
            {
                activationNeuronSB.Remove( activationNeuronSB.Length - 2, 2 );
            }
            activationNeuronSB.Append( "]), " + input.ToString( "F2" ) + ", " + output.ToString( "F2" ) + ")" );

            return activationNeuronSB.ToString();
        }

        #endregion // Public instance methods
    }
}
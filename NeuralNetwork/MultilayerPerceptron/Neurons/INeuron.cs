using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// <para>
    /// An interface of a neuron.
    /// </para>
    /// <para>
    /// Definition: A neuron is the fundamental building block of a neural network.
    /// </para>
    /// </summary>
    public interface INeuron
    {
        #region Properties

        /// <summary>
        /// Gets the ouput.
        /// </summary>
        /// 
        /// <value>
        /// The output.
        /// </value>
        double Output
        {
            get;
        }

        /// <summary>
        /// Gets the list of target synapses.
        /// </summary>
        ///
        /// <value>
        /// The list of target synapses.
        /// </value>
        List<ISynapse> TargetSynapses
        {
            get;
        }

        /// <summary>
        /// Gets or sets the parent layer.
        /// </summary>
        ///
        /// <value>
        /// The parent layer.
        /// </value>
        ILayer ParentLayer
        {
            get;
            set;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        void Initialize();

        #endregion // Methods
    }
}

using System;
using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// An interface of a layer.
    /// </summary>
    public interface ILayer
    {
        #region Properties

        /// <summary>
        /// Gets the list of neurons comprising the layer.
        /// </summary>
        /// 
        /// <value>
        /// The list of neurons comprising the layer.
        /// </value>
        List<INeuron> Neurons
        {
            get;
        }

        /// <summary>
        /// Gets the number of neurons comprising the layer.
        /// </summary>
        /// 
        /// <value>
        /// The number of neurons comprising the layer.
        /// </value>
        int NeuronCount
        {
            get;
        }

        /// <summary>
        /// Gets the list of target connectors associated with the layer.
        /// </summary>
        /// 
        /// <value>
        /// The list of target connectors associated with the layer.
        /// </value>
        List<IConnector> TargetConnectors
        {
            get;
        }

        /// <summary>
        /// Gets or sets the parent network.
        /// </summary>
        /// 
        /// <value>
        /// The parent network.
        /// </value>
        INetwork ParentNetwork
        {
            get;
            set;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Gets a neuron (specified by its index within the layer).
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>
        /// The neuron.
        /// </returns>
        INeuron GetNeuronByIndex(int neuronIndex);

        /// <summary>
        /// Initializes the layer.
        /// </summary>
        void Initialize();

        #endregion // Methods
    }
}

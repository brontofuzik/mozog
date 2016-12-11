using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// <para>
    /// An interface of an activation layer.
    /// </para>
    /// <para>
    /// Definition: A layer is a collection of neurons.
    /// </para>
    /// </summary>
    public interface IActivationLayer
        : ILayer
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        new List<IActivationNeuron> Neurons
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IActivationFunction ActivationFunction
        {
            get;
        }

        /// <summary>
        /// Gets the list of source connectors associated with the layer.
        /// </summary>
        /// 
        /// <value>
        /// The list of source connectors associated with the layer.
        /// </value>
        List<IConnector> SourceConnectors
        {
            get;
        }

        #endregion // Properties

        #region Methods
        
        /// <summary>
        /// 
        /// </summary>
        void Evaluate();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        double[] GetOutputVector();

        #endregion // Methods
    }
}

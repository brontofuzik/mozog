using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    /// <summary>
    /// <para>
    /// An interface of an artificial neural network.
    /// </para>
    /// <para>
    /// Definition: An artificial neural network is ...
    /// </para>
    /// </summary>
    public interface INetwork
    {
        #region Methods

        /// <summary>
        /// Connects the network.
        /// 
        /// A network is said to be connected if:
        /// 
        /// 1. its layers are connected and
        /// 2. its connectors are connected.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects the network.
        /// 
        /// A network is said to be disconnected if:
        /// 
        /// 1. its layers are disconnected and
        /// 2. its connectors are disconnected.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Gets the layer (specified by its index).
        /// </summary>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <returns>
        /// The layer.
        /// </returns>
        ILayer GetLayerByIndex(int layerIndex);

        /// <summary>
        /// Initializes the network.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>
        /// The output vector.
        /// </returns>
        double[] Evaluate(double[] inputVector);

        /// <summary>
        /// Calculate the error of the network with respect to a training set.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        double CalculateError( TrainingSet trainingSet );

        /// <summary>
        /// Calculate the error of the network with respect to a trainig pattern.
        /// </summary>
        /// <param name="trainingPattern">The training pattern.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        double CalculateError( SupervisedTrainingPattern trainingPattern );

        /// <summary>
        /// Saves the weights of the network to a file.
        /// </summary>
        /// 
        /// <param name="weightsFileName">The name of the file to save the weights to.</param>
        void SaveWeights(string weightFileName);

        /// <summary>
        /// Loads the weights of the network from a file.
        /// </summary>
        /// 
        /// <param name="weightsFileName">The name of the file to load the weights from.</param>
        void LoadWeights(string weightFileName);

        /// <summary>
        /// Gets the weights of the network (as an array).
        /// </summary>
        /// <returns>
        /// The weights of the network (as an array).
        /// </returns>
        double[] GetWeights();

        /// <summary>
        /// Sets the weights of the network (as an  array).
        /// </summary>
        /// <param name="weights">The weights of the network (as an array).</param>
        void SetWeights(double[] weights);

        #endregion // Methods

        #region Properties

        /// <summary>
        /// Gets the network blueprint.
        /// </summary>
        /// 
        /// <value>
        /// The network blueprint.
        /// </value>
        NetworkBlueprint Blueprint
        {
            get;
        }

        /// <summary>
        /// Gets the bias layer.
        /// </summary>
        /// 
        /// <value>
        /// The bias layer.
        /// </value>
        InputLayer BiasLayer
        {
            get;
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// 
        /// <value>
        /// The input layer.
        /// </value>
        InputLayer InputLayer
        {
            get;
        }

        /// <summary>
        /// Gets the layers.
        /// </summary>
        /// 
        /// <value>
        /// The layers.
        /// </value>
        List<IActivationLayer> HiddenLayers
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int HiddenLayerCount
        {
            get;
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// 
        /// <value>
        /// The output layer.
        /// </value>
        IActivationLayer OutputLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        ///
        /// <value>
        /// The number of layers.
        /// </value>
        int LayerCount
        {
            get;
        }

        /// <summary>
        /// Gets the list of connectors comrprising this network.
        /// </summary>
        /// 
        /// <value>
        /// The list of connectors comrprising this network.
        /// </value>
        List<IConnector> Connectors
        {
            get;
        }

        /// <summary>
        /// Gets the number of connectors in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of connectors in this network.
        /// </value>
        int ConnectorCount
        {
            get;
        }

        /// <summary>
        /// Gets the number of synapses in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of synapses in this network.
        /// </value>
        int SynapseCount
        {
            get;
        }
        
        #endregion // Properties
    }
}

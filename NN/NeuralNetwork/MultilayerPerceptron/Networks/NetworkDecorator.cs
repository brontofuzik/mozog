using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Training;


namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    /// <summary>
    /// A network decorator.
    /// </summary>
    public abstract class NetworkDecorator
        : INetwork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoratedNetwork"></param>
        public NetworkDecorator(INetwork decoratedNetwork)
        {
            this._decoratedNetwork = decoratedNetwork;
        }

        /// <summary>
        /// Connects the network.
        /// 
        /// A network is said to be connected if:
        /// 
        /// 1. its layers are connected and
        /// 2. its connectors are connected.
        /// </summary>
        public virtual void Connect()
        {
            _decoratedNetwork.Connect();
        }

        /// <summary>
        /// Disconnects the network.
        /// 
        /// A network is said to be disconnected if:
        /// 
        /// 1. its layers are disconnected and
        /// 2. its connectors are disconnected.
        /// </summary>
        public virtual void Disconnect()
        {
            _decoratedNetwork.Disconnect();
        }

        /// <summary>
        /// Returns the decorated network.
        /// </summary>
        /// 
        /// <returns>
        /// The decorated network.
        /// </returns>
        public virtual INetwork GetDecoratedNetwork()
        {
            // 1. Disconnect the netowork.
            Disconnect();

            // 2. Undecorate the network components.
            // 2.1. Undecorate the layers.
            // 2.1.1. Undecorate the bias layer.
            BiasLayer.ParentNetwork = _decoratedNetwork;
            
            // 2.1.2. Undecorate the input layer.
            InputLayer.ParentNetwork = _decoratedNetwork;

            // 2.1.3. Undecorate the hidden layers.
            for (int i = 0; i < HiddenLayerCount; i++)
            {
                HiddenLayers[ i ] = (HiddenLayers[ i ] as ActivationLayerDecorator).GetDecoratedActivationLayer(_decoratedNetwork);
            }

            // 2.1.4. Undecorate the output layer.
            OutputLayer = (OutputLayer as ActivationLayerDecorator).GetDecoratedActivationLayer(_decoratedNetwork);
            
            // 2.2. Undecorate the connectors and their synapses.
            for (int i = 0; i < ConnectorCount; i++)
            {
                Connectors[ i ] = (Connectors[ i ] as ConnectorDecorator).GetDecoratedConnector(_decoratedNetwork);
            }

            // 3. Connect the network.
            Connect();

            return _decoratedNetwork;
        }

        /// <summary>
        /// Gets the layer (specified by its index).
        /// </summary>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <returns>
        /// The layer.
        /// </returns>
        public virtual ILayer GetLayerByIndex(int layerIndex)
        {
            return _decoratedNetwork.GetLayerByIndex(layerIndex);
        }

        /// <summary>
        /// Initializes the network.
        /// </summary>
        public virtual void Initialize()
        {
            _decoratedNetwork.Initialize();
        }

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>
        /// The output vector.
        /// </returns>
        public virtual double[] Evaluate(double[] inputVector)
        {
            return _decoratedNetwork.Evaluate(inputVector);
        }

        /// <summary>
        /// Calculate the error of the network with respect to a training set.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        public virtual double CalculateError( TrainingSet trainingSet )
        {
            return _decoratedNetwork.CalculateError( trainingSet );
        }

        /// <summary>
        /// Calculate the error of the network with respect to a training pattern.
        /// </summary>
        /// <param name="trainingSet">The trainingpattern.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        public virtual double CalculateError( SupervisedTrainingPattern trainingPattern )
        {
            return _decoratedNetwork.CalculateError( trainingPattern );
        }

        /// <summary>
        /// Saves the weights of the network to a file.
        /// </summary>
        /// 
        /// <param name="weightsFileName">The name of the file to save the weights to.</param>
        public virtual void SaveWeights(string weightsFileName)
        {
            _decoratedNetwork.SaveWeights(weightsFileName);
        }

        /// <summary>
        /// Loads the weights of the network from a file.
        /// </summary>
        /// 
        /// <param name="weightsFileName">The name of the file to load the weights from.</param>
        public virtual void LoadWeights(string weightsFileName)
        {
            _decoratedNetwork.LoadWeights(weightsFileName);
        }

        /// <summary>
        /// Gets the weights of the network (as an array).
        /// </summary>
        /// <returns>
        /// The weights of the network (as an array).
        /// </returns>
        public virtual double[] GetWeights()
        {
            return _decoratedNetwork.GetWeights();
        }

        /// <summary>
        /// Sets the weights of the network (as an  array).
        /// </summary>
        /// <param name="weights">The weights of the network (as an array).</param>
        public virtual void SetWeights( double[] weights )
        {
            _decoratedNetwork.SetWeights( weights );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _decoratedNetwork.ToString();
        }

        /// <summary>
        /// Gets the network blueprint.
        /// </summary>
        /// 
        /// <value>
        /// The network blueprint.
        /// </value>
        public virtual NetworkBlueprint Blueprint
        {
            get
            {
                return _decoratedNetwork.Blueprint;
            }
        }

        /// <summary>
        /// Gets the bias layer.
        /// </summary>
        /// 
        /// <value>
        /// The bias layer.
        /// </value>
        public virtual InputLayer BiasLayer
        {
            get
            {
                return _decoratedNetwork.BiasLayer;
            }
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// 
        /// <value>
        /// The input layer.
        /// </value>
        public virtual InputLayer InputLayer
        {
            get
            {
                return _decoratedNetwork.InputLayer;
            }
        }

        /// <summary>
        /// Gets the hidden layers.
        /// </summary>
        /// 
        /// <value>
        /// The layers.
        /// </value>
        public virtual List<IActivationLayer> HiddenLayers
        {
            get
            {
                return _decoratedNetwork.HiddenLayers;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int HiddenLayerCount
        {
            get
            {
                return _decoratedNetwork.HiddenLayerCount;
            }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// 
        /// <value>
        /// The output layer.
        /// </value>
        public virtual IActivationLayer OutputLayer
        {
            get
            {
                return _decoratedNetwork.OutputLayer;
            }
            set
            {
                _decoratedNetwork.OutputLayer = value;
            }
        }

        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        ///
        /// <value>
        /// The number of layers.
        /// </value>
        public virtual int LayerCount
        {
            get
            {
                return _decoratedNetwork.LayerCount;
            }
        }

        /// <summary>
        /// Gets the list of connectors comrprising this network.
        /// </summary>
        /// 
        /// <value>
        /// The list of connectors comrprising this network.
        /// </value>
        public virtual List<IConnector> Connectors
        {
            get
            {
                return _decoratedNetwork.Connectors;
            }
        }

        /// <summary>
        /// Gets the number of connectors in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of connectors in this network.
        /// </value>
        public virtual int ConnectorCount
        {
            get
            {
                return _decoratedNetwork.ConnectorCount;
            }
        }

        /// <summary>
        /// Gets the number of synapses in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of synapses in this network.
        /// </value>
        public virtual int SynapseCount
        {
            get
            {
                return _decoratedNetwork.SynapseCount;
            }
        }

        /// <summary>
        /// The decorated network.
        /// </summary>
        protected INetwork _decoratedNetwork;
    }
}

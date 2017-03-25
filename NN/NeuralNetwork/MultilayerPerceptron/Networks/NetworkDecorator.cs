using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    public abstract class NetworkDecorator : INetwork
    {
        protected INetwork decoratedNetwork;

        protected NetworkDecorator(INetwork decoratedNetwork)
        {
            this.decoratedNetwork = decoratedNetwork;
        }

        public virtual NetworkBlueprint Blueprint => decoratedNetwork.Blueprint;

        public virtual InputLayer BiasLayer => decoratedNetwork.BiasLayer;

        public virtual InputLayer InputLayer => decoratedNetwork.InputLayer;

        public virtual List<IActivationLayer> HiddenLayers => decoratedNetwork.HiddenLayers;

        public virtual int HiddenLayerCount => decoratedNetwork.HiddenLayerCount;

        public virtual IActivationLayer OutputLayer
        {
            get { return decoratedNetwork.OutputLayer; }
            set { decoratedNetwork.OutputLayer = value; }
        }

        public virtual int LayerCount => decoratedNetwork.LayerCount;

        public virtual List<IConnector> Connectors => decoratedNetwork.Connectors;

        public virtual int ConnectorCount => decoratedNetwork.ConnectorCount;

        public virtual int SynapseCount => decoratedNetwork.SynapseCount;

        /// <summary>
        /// A network is connected if:
        /// 1. its layers are connected and
        /// 2. its connectors are connected.
        /// </summary>
        public virtual void Connect()
        {
            decoratedNetwork.Connect();
        }

        /// <summary>
        /// A network is disconnected if:
        /// 1. its layers are disconnected and
        /// 2. its connectors are disconnected.
        /// </summary>
        public virtual void Disconnect()
        {
            decoratedNetwork.Disconnect();
        }

        public virtual INetwork GetDecoratedNetwork()
        {
            // 1. Disconnect the netowork.
            Disconnect();

            // 2. Undecorate the network components.
            // 2.1. Undecorate the layers.
            // 2.1.1. Undecorate the bias layer.
            BiasLayer.ParentNetwork = decoratedNetwork;
            
            // 2.1.2. Undecorate the input layer.
            InputLayer.ParentNetwork = decoratedNetwork;

            // 2.1.3. Undecorate the hidden layers.
            for (int i = 0; i < HiddenLayerCount; i++)
            {
                HiddenLayers[i] = (HiddenLayers[i] as ActivationLayerDecorator).GetDecoratedActivationLayer(decoratedNetwork);
            }

            // 2.1.4. Undecorate the output layer.
            OutputLayer = (OutputLayer as ActivationLayerDecorator).GetDecoratedActivationLayer(decoratedNetwork);
            
            // 2.2. Undecorate the connectors and their synapses.
            for (int i = 0; i < ConnectorCount; i++)
            {
                Connectors[i] = (Connectors[i] as ConnectorDecorator).GetDecoratedConnector(decoratedNetwork);
            }

            // 3. Connect the network.
            Connect();

            return decoratedNetwork;
        }

        public virtual ILayer GetLayerByIndex(int layerIndex) => decoratedNetwork.GetLayerByIndex(layerIndex);

        public virtual void Initialize()
        {
            decoratedNetwork.Initialize();
        }

        public virtual double[] Evaluate(double[] inputVector) => decoratedNetwork.Evaluate(inputVector);

        public virtual double CalculateError(TrainingSet trainingSet) => decoratedNetwork.CalculateError(trainingSet);

        public virtual double CalculateError(SupervisedTrainingPattern trainingPattern) => decoratedNetwork.CalculateError(trainingPattern);

        public virtual void SaveWeights(string weightsFileName)
        {
            decoratedNetwork.SaveWeights(weightsFileName);
        }

        public virtual void LoadWeights(string weightsFileName)
        {
            decoratedNetwork.LoadWeights(weightsFileName);
        }

        public virtual double[] GetWeights() => decoratedNetwork.GetWeights();

        public virtual void SetWeights(double[] weights)
        {
            decoratedNetwork.SetWeights(weights);
        }

        public override string ToString() => decoratedNetwork.ToString();
    }
}

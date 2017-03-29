using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Network : INetwork
    {
        private InputLayer biasLayer;

        #region Construction

        public Network(INetworkArchitecture architecture)
        {
            Architecture = architecture;
            architecture.Layers.ForEach(AddLayer);
            CreateBiasLayer();
        }

        protected virtual ILayer MakeLayer(NetworkArchitecture.Layer layerPlan)
            => layerPlan.Activation != null ? (ILayer)new ActivationLayer(layerPlan) : (ILayer)new InputLayer(layerPlan);

        private void AddLayer(NetworkArchitecture.Layer layerPlan)
        {
            var layer = MakeLayer(layerPlan);
            AddLayer(layer);
            ConnectLayer(layer, layerPlan);
        }

        private void AddLayer(ILayer layer)
        {
            Layers.Add(layer);
            layer.Network = this;
        }

        private void ConnectLayer(ILayer layer, NetworkArchitecture.Layer layerPlan)
        {
            layerPlan.SourceLayers.ForEach(sl => layer.Connect(Layers[sl]));     
        }

        private void CreateBiasLayer()
        {
            biasLayer = (InputLayer)MakeLayer(new NetworkArchitecture.Layer(1));
            biasLayer.Network = this;
            ActivationLayers.ForEach(l => l.Connect(biasLayer));
        }

        #endregion // Construction

        public INetworkArchitecture Architecture { get; }

        #region Layers

        public List<ILayer> Layers { get; } = new List<ILayer>();

        public int LayerCount => Layers.Count;

        public IEnumerable<ActivationLayer> ActivationLayers
        {
            get
            {
                for (int l = 1; l < Layers.Count; l++)
                {
                    yield return (ActivationLayer)Layers[l];
                }
            }
        }

        public InputLayer InputLayer => (InputLayer)Layers[0];

        public ActivationLayer OutputLayer => (ActivationLayer)Layers[Layers.Count - 1];

        #endregion // Layers

        #region Neurons

        protected IEnumerable<IInputNeuron> InputNeurons
            => InputLayer.Neurons_Typed;

        protected IEnumerable<IActivationNeuron> ActivationNeurons
            => ActivationLayers.SelectMany(l => l.Neurons_Typed);

        #endregion // Neurons

        #region Synapses

        protected IEnumerable<ISynapse> Synapses => ActivationNeurons.SelectMany(n => n.SourceSynapses);

        public int SynapseCount => Synapses.Count();

        #endregion // Synapses

        public virtual void Initialize()
        {
            Layers.ForEach(l => l.Initialize());
        }

        public double[] Evaluate(double[] inputVector)
        {
            SetInputVector(inputVector);
            Evaluate();
            return GetOutputVector();
        }

        // TODO Jitter
        //private void Jitter(double noiseLimit)
        //{
        //    Synapses.ForEach(s => Jitter(noiseLimit));
        //}

        private void SetInputVector(double[] inputVector)
        {
            InputLayer.SetOutputVector(inputVector);
        }

        private void Evaluate()
        {
            ActivationLayers.ForEach(l => l.Evaluate());
            OutputLayer.Evaluate();
        }

        private double[] GetOutputVector() => OutputLayer.GetOutputVector();

        public double CalculateError(TrainingSet trainingSet)
        {
            double networkError = 0.0;
            foreach (SupervisedTrainingPattern trainingPattern in trainingSet)
            {
                networkError += CalculateError(trainingPattern);
            }
            return 0.5 * networkError;
        }

        public double CalculateError(SupervisedTrainingPattern trainingPattern)
        {
            double[] outputVector = Evaluate(trainingPattern.InputVector);
            double[] desiredOutputVector = trainingPattern.OutputVector;

            // Ref
            return outputVector.Select((o, i) => Math.Pow(o - desiredOutputVector[i], 2)).Sum();
        }

        public double[] GetWeights() => Synapses.Select(s => s.Weight).ToArray();

        public void SetWeights(double[] weights)
        {
            Synapses.ForEach((s, i) => s.Weight = weights[i]);
        }

        // TODO ToString
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("MLP\n[\n");

            // The input layer.
            int layerIndex = 0;
            sb.Append(layerIndex++ + " : " + InputLayer + "\n");

            // The hidden layers.
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
            {
                sb.Append(layerIndex++ + " : " + hiddenLayer + "\n");
            }

            // The output layer.
            sb.Append(layerIndex + " : " + OutputLayer + "\n]");

            return sb.ToString();
        }
    }
}
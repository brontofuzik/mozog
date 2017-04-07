﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            biasLayer.Output =new[] {-1.0};
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
            => InputLayer.Neurons;

        protected IEnumerable<IActivationNeuron> ActivationNeurons
            => ActivationLayers.SelectMany(l => l.Neurons);

        #endregion // Neurons

        #region Synapses

        protected IEnumerable<ISynapse> Synapses => ActivationNeurons.SelectMany(n => n.SourceSynapses);

        public int SynapseCount => Synapses.Count();

        #endregion // Synapses

        public virtual void Initialize()
        {
            Layers.ForEach(l => l.Initialize());
        }

        public double[] Evaluate(double[] input)
        {
            Input = input;
            Evaluate();
            return Output;
        }

        public TOutput EvaluateEncoded<TInput, TOutput>(TInput input, IEncoder<TInput, TOutput> encoder)
            => encoder.DecodeOutput(Evaluate(encoder.EncodeInput(input)));

        // TODO Jitter
        //private void Jitter(double noiseLimit)
        //{
        //    Synapses.ForEach(s => Jitter(noiseLimit));
        //}

        private double[] Input
        {
            set { InputLayer.Output = value; }
        }

        private void Evaluate()
        {
            ActivationLayers.ForEach(l => l.Evaluate());
            OutputLayer.Evaluate();
        }

        private double[] Output => OutputLayer.Output;

        public double CalculateError(DataSet dataSet)
            => 0.5 * dataSet.Sum(point => CalculateError(point));

        public double CalculateError(LabeledDataPoint point)
        {
            var actualOutput = Evaluate(point.Input);
            var expectedOutput = point.Output;
            return Vector.Distance(actualOutput, expectedOutput);
        }

        public double[] GetWeights() => Synapses.Select(s => s.Weight).ToArray();

        public void SetWeights(double[] weights)
        {
            Synapses.ForEach((s, i) => s.Weight = weights[i]);
        }

        public override string ToString()
            => $"MLP[\n{String.Join(",\n", Layers.Select((l, i) => $"\t{i}: {l}"))}\n]";
    }
}
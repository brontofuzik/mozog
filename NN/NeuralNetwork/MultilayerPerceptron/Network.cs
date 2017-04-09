using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.ErrorFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Network : INetwork
    {
        protected IErrorFunction errorFunc;
        private InputLayer biasLayer;

        #region Construction

        public Network(INetworkArchitecture architecture)
        {
            Architecture = architecture;
            architecture.Layers.ForEach(AddLayer);
            CreateBiasLayer();
            errorFunc = architecture.ErrorFunction;
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

        private double[] Input
        {
            set { InputLayer.Output = value; }
        }

        private double[] Output => OutputLayer.Output;

        #region Layers

        public List<ILayer> Layers { get; } = new List<ILayer>();

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

        public InputLayer InputLayer => Layers[0] as InputLayer;

        public ActivationLayer OutputLayer => Layers[Layers.Count - 1] as ActivationLayer;

        public int LayerCount => ActivationLayers.Count();

        #endregion // Layers

        #region Neurons

        protected IEnumerable<IInputNeuron> InputNeurons
            => InputLayer.Neurons;

        protected IEnumerable<IActivationNeuron> ActivationNeurons
            => ActivationLayers.SelectMany(l => l.Neurons);

        public int NeuronCount => ActivationNeurons.Count();

        #endregion // Neurons

        #region Synapses

        protected IEnumerable<ISynapse> Synapses => ActivationNeurons.SelectMany(n => n.SourceSynapses);

        public int SynapseCount => Synapses.Count();

        #endregion // Synapses

        public double[] Evaluate(double[] input)
        {
            Input = input;
            Evaluate();
            return Output;
        }

        public (double[] output, double error) Evaluate(double[] input, double[] target)
        {
            var output = Evaluate(input);
            var error = errorFunc.Evaluate(output, target);
            return (output, error);
        }

        public TOutput EvaluateEncoded<TInput, TOutput>(TInput input, IEncoder<TInput, TOutput> encoder)
            => encoder.DecodeOutput(Evaluate(encoder.EncodeInput(input)));

        public (TOutput output, double error) EvaluateEncoded<TInput, TOutput>(TInput input, TOutput target, IEncoder<TInput, TOutput> encoder)
        {
            var result = Evaluate(encoder.EncodeInput(input), encoder.EncodeOutput(target));
            return (encoder.DecodeOutput(result.output), result.error);
        }

        private void Evaluate()
        {
            ActivationLayers.ForEach(l => l.Evaluate());
        }

        #region Error function

        //public double Error { get; private set; }

        //public void ResetError()
        //{
        //    Error = 0.0;
        //}

        //public void UpdateError(double[] target)
        //{
        //    Error += errorFunc.Evaluate(OutputLayer.Output, target);
        //}

        public double CalculateError(DataSet dataSet)
            => dataSet.Sum((ILabeledDataPoint point) => CalculateError(point)) / dataSet.Size;

        public double CalculateError(ILabeledDataPoint point)
            => errorFunc.Evaluate(Evaluate(point.Input), point.Output);

        #endregion // Error function

        public double[] GetWeights() => Synapses.Select(s => s.Weight).ToArray();

        public void SetWeights(double[] weights)
        {
            Synapses.ForEach((s, i) => s.Weight = weights[i]);
        }

        public override string ToString()
            => $"MLP[\n{String.Join(",\n", Layers.Select((l, i) => $"\t{i}: {l}"))}\n]";
    }
}
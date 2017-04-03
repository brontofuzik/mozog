using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationLayer : LayerBase<ActivationNeuron>, IActivationLayer
    {
        #region Construction

        internal ActivationLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
            ActivationFunction = layer.Activation;
        }

        protected override ActivationNeuron MakeNeuron() => new ActivationNeuron();

        #endregion // Construction

        IEnumerable<IActivationNeuron> ILayer<IActivationNeuron>.Neurons_Typed => Neurons;

        public IActivationFunction ActivationFunction { get; }

        
        public override void Initialize()
        {
            Neurons.ForEach(n => n.Initialize());
        }

        public void Evaluate()
        {
            Neurons.ForEach(n => n.Evaluate());
        }

        // TODO Jitter
        //public void Jitter(double noiseLimit)
        //{
        //    Neurons.ForEach(n => n.Jitter(noiseLimit));
        //}

        public double[] GetOutput() => Neurons.Select(n => n.Output).ToArray();

        public override string ToString() => "AL" + base.ToString();
    }
}
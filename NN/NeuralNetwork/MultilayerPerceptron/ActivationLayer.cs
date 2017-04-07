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
            Activation = layer.Activation;
        }

        protected override ActivationNeuron MakeNeuron() => new ActivationNeuron();

        #endregion // Construction

        public new IEnumerable<IActivationNeuron> Neurons => base.Neurons;

        private IActivationFunction Activation { get; }

        public IActivationFunction1 Activation1 => Activation as IActivationFunction1;

        public IActivationFunction2 Activation2 => Activation as IActivationFunction2;

        public override void Initialize()
        {
            NeuronList.ForEach(n => n.Initialize());
        }

        public void Evaluate()
        {
            if (Activation is IActivationFunction1)
            {
                Evaluate1();
            }
            else
            {
                Evaluate2();
            }
        }

        private void Evaluate1()
        {
            Neurons.ForEach(n => n.Evaluate());
        }

        private void Evaluate2()
        {
            Neurons.ForEach(n => n.EvaluateInput());
            Output = Activation2.Evaluate(Input);
        }

        // TODO Jitter
        //public void Jitter(double noiseLimit)
        //{
        //    Neurons.ForEach(n => n.Jitter(noiseLimit));
        //}

        public override string ToString() => "AL" + base.ToString();
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class InputLayer : LayerBase<InputNeuron>, IInputLayer
    {
        #region Construction

        internal InputLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
        }

        protected override InputNeuron MakeNeuron() => new InputNeuron();

        #endregion // Construction

        //public new IList<InputNeuron> Neurons_Typed => Neurons;

        IEnumerable<IInputNeuron> ILayer<IInputNeuron>.Neurons_Typed => Neurons;

        public void SetOutput(double[] output)
        {
            Debug.Assert(output.Length == NeuronCount);

            output.ForEach((e, i) => Neurons[i].Output = e);
        }

        public override string ToString() => "IL" + base.ToString();
    }
}

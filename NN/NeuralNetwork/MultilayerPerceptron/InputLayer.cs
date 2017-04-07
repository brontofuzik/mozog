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

        public new IEnumerable<IInputNeuron> Neurons => base.Neurons;

        public override string ToString() => "IL" + base.ToString();
    }
}

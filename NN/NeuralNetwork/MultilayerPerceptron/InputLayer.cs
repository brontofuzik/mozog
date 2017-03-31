using System.Collections.Generic;
using System.Diagnostics;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class InputLayer : LayerBase<IInputNeuron>, IInputLayer
    {
        #region Construction

        internal InputLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
        }

        protected override IInputNeuron MakeNeuron() => new InputNeuron();

        #endregion // Construction

        public IList<IInputNeuron> Neurons_Typed => Neurons;

        public void SetOutput(double[] output)
        {
            Debug.Assert(output.Length == NeuronCount);

            output.ForEach((e, i) => Neurons[i].Output = e);
        }

        public override string ToString() => "IL" + base.ToString();
    }
}

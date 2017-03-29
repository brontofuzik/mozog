using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Construction;
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

        public void SetOutputVector(double[] outputVector)
        {
            Debug.Assert(outputVector.Length == NeuronCount);

            outputVector.Select((e, i) => Neurons[i].Output = e);
        }

        // TODO ToString
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("IL\n[\n");

            int neuronIndex = 0;
            foreach (var neuron in Neurons)
            {
                sb.Append("  " + neuronIndex++ + " : " + neuron + "\n");
            }
            sb.Append("]");
            
            return sb.ToString();
        }
    }
}

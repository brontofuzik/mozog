using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class InputLayer : LayerBase<IInputNeuron>, IInputLayer
    {
        public InputLayer(int neuronCount, INetwork parentNetwork)
            : base(parentNetwork)
        {
            neuronCount.Times(() => Neurons.Add(new InputNeuron(this)));
        }

        public IEnumerable<INeuron> Ns => Neurons;

        public void Initialize()
        {
            // Do nothing.
        }

        public void SetOutputVector(double[] outputVector)
        {
            Debug.Assert(outputVector.Length == NeuronCount);

            outputVector.Select((e, i) => Neurons[i].Output = e);
        }

        // TODO
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

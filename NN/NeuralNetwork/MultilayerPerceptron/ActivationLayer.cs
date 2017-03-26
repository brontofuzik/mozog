using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron.ActivationFunctions;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationLayer : LayerBase<IActivationNeuron>, IActivationLayer
    {
        public ActivationLayer(int neuronCount, IActivationFunction activationFunction, INetwork network)
            : base(network)
        {
            neuronCount.Times(() => Neurons.Add(new ActivationNeuron(this)));
            ActivationFunction = activationFunction;
        }

        public IEnumerable<INeuron> Ns => Neurons;

        public IActivationFunction ActivationFunction { get; }

        public void Initialize()
        {
            // Ref
            Neurons.AsEnumerable().ForEach(n => n.Initialize());
        }

        public void Evaluate()
        {
            // Ref
            Neurons.AsEnumerable().ForEach(n => n.Evaluate());
        }

        // Ref
        public double[] GetOutputVector() => Neurons.Select(n => n.Output).ToArray();

        // TODO
        public override string ToString()
        {
            var sb = new StringBuilder("AL\n[\n");

            int neuronIndex = 0;
            foreach (IActivationNeuron neuron in Neurons)
            {
                sb.Append("  " + neuronIndex++ + " : " + neuron + "\n");
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
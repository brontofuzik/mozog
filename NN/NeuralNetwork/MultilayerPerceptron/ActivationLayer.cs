using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Construction;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationLayer : LayerBase<IActivationNeuron>, IActivationLayer
    {
        internal ActivationLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
            ActivationFunction = layer.Activation;
        }

        protected override IActivationNeuron MakeNeuron()
            => new ActivationNeuron();

        public IList<IActivationNeuron> Neurons_Typed => Neurons;

        public IActivationFunction ActivationFunction { get; }

        public override void Initialize()
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
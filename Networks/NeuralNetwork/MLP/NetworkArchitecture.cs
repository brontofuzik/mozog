using System;
using System.Linq;
using NeuralNetwork.MLP.ActivationFunctions;
using NeuralNetwork.MLP.ErrorFunctions;

namespace NeuralNetwork.MLP
{
    // Network topology
    public interface INetworkArchitecture
    {
        NetworkArchitecture.Layer[] Layers { get; set; }

        IErrorFunction ErrorFunction { get; set; }
    }

    public class NetworkArchitecture : INetworkArchitecture
    {
        public Layer[] Layers { get; set; }

        public IErrorFunction ErrorFunction { get; set; }

        public static INetworkArchitecture Feedforward((int neurons, IActivationFunction activation)[] layers, IErrorFunction errorFunction)
            => new NetworkArchitecture
                {
                    Layers = layers.Select((l, i) => new Layer(l.neurons)
                    {
                        SourceLayers = l.activation != null ? new[] {i - 1} : new int[0],
                        Activation = l.activation
                    }).ToArray(),
                    ErrorFunction = errorFunction
                };

        public static INetworkArchitecture Feedforward(int[] neurons, IActivationFunction activation, IErrorFunction errorFunction)
            => Feedforward(neurons.Select((n, i) => (n, i > 0 ? activation : null)).ToArray(), errorFunction);

        public override string ToString() => $"[{String.Join(", ", Layers.AsEnumerable())}]";

        public class Layer
        {
            public Layer(int neurons)
            {
                Neurons = neurons;
            }

            public int Neurons { get; set; }

            // Default is no source layers, indicating an input layer.
            public int[] SourceLayers { get; set; } = new int[0];

            // Default is no activation function, indicating an input layer.
            public IActivationFunction Activation { get; set; }

            public override string ToString() => $"({Neurons}, {Activation})";
        }
    }
}

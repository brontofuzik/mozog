using System;
using System.Linq;
using NeuralNetwork.ActivationFunctions;

namespace NeuralNetwork.Interfaces
{
    // Network topology
    public interface INetworkArchitecture
    {
        NetworkArchitecture.Layer[] Layers { get; set; }
    }

    public class NetworkArchitecture : INetworkArchitecture
    {
        public Layer[] Layers { get; set; }

        public static INetworkArchitecture Feedforward((int neurons, IActivationFunction activation)[] layers)
            => new NetworkArchitecture
                {
                    Layers = layers.Select((l, i) => new Layer(l.neurons)
                    {
                        SourceLayers = l.activation != null ? new[] {i - 1} : new int[0],
                        Activation = l.activation
                    }).ToArray()
                };

        public static INetworkArchitecture Feedforward(int[] neurons, IActivationFunction activation)
            => Feedforward(neurons.Select((n, i) => (n, i > 0 ? activation : null)).ToArray());

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

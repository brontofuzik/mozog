using System.Linq;
using NeuralNetwork.ActivationFunctions;

namespace NeuralNetwork.Interfaces
{
    public interface INetworkArchitecture
    {
        NetworkArchitecture.Layer[] Layers { get; set; }
    }

    public class NetworkArchitecture : INetworkArchitecture
    {
        public Layer[] Layers { get; set; }

        // Shortcut
        public static INetworkArchitecture Feedforward(int[] layers, IActivationFunction activation)
            => new FeedforwardArchitecture(layers, activation);

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
        }
    }

    public class FeedforwardArchitecture : NetworkArchitecture
    {
        public FeedforwardArchitecture((int neurons, IActivationFunction activation)[] layers)
        {
            Layers = layers.Select((l, i) => new Layer(l.neurons)
            {
                SourceLayers = l.activation != null ? new[] {i - 1} : new int[0],
                Activation = l.activation
            }).ToArray();
        }

        public FeedforwardArchitecture(int[] neurons, IActivationFunction activation)
            : this(neurons.Select(n => (n, activation)).ToArray())
        {      
        }
    }
}

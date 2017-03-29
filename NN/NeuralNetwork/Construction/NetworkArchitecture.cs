using System.Linq;
using NeuralNetwork.ActivationFunctions;

namespace NeuralNetwork.Construction
{
    public interface INetworkArchitecture
    {
    }

    public abstract class NetworkArchitecture : INetworkArchitecture
    {
        public Layer[] Layers { get; protected set; }

        //public Connector[] Connectors { get; protected set; }

        // Shortcut
        public static INetworkArchitecture Feedforward(int[] layers, IActivationFunction activation)
            => new FeedforwardArchitecture(layers, activation);

        public class Layer
        {
            public Layer(int neurons, IActivationFunction activation)
            {
                Neurons = neurons;
                Activation = activation;
            }

            public int Neurons { get; private set; }

            public IActivationFunction Activation { get; private set; }
        }

        public class Connector
        {
            public Connector(int source, int target)
            {
                Source = source;
                Target = target;
            }

            public int Source { get; private set; }

            public int Target { get; private set; }
        }
    }

    public class FeedforwardArchitecture : NetworkArchitecture
    {
        public FeedforwardArchitecture(Layer[] layers)
        {
            Layers = layers;
            Connectors = Enumerable.Range(0, layers.Length - 1).Select(l => new Connector(l, l + 1)).ToArray();
        }

        public FeedforwardArchitecture(int[] neurons, IActivationFunction activation)
            : this(neurons.Select(n => new Layer(n, activation)).ToArray())
        {      
        }
    }
}

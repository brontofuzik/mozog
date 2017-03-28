using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Construction
{
    public interface INetworkArchitecture
    {
    }

    public abstract class NetworkArchitecture : INetworkArchitecture
    {
        public static INetworkArchitecture Feedforward => new FeedforwardArchitecture();
    }

    public class FeedforwardArchitecture : NetworkArchitecture
    {
    }
}

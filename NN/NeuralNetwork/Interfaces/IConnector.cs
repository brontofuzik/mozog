using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IConnector
    {
        List<ISynapse> Synapses { get; }

        int SynapseCount { get; }

        ILayer SourceLayer { get; set; }

        ILayer TargetLayer { get; set; }

        INetwork Network { get; set; }

        void Connect();

        void Initialize();

        void Jitter(double jitterNoiseLimit);
    }
}

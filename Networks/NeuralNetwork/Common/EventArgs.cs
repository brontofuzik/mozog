using System;

namespace NeuralNetwork.Common
{
    public class TrainingEventArgs : EventArgs
    {
        public TrainingEventArgs(int iteration)
        {
            Iteration = iteration;
        }

        public int Iteration { get; }
    }

    public class InitializationEventArgs : EventArgs
    {
        public InitializationEventArgs(int neuronIndex)
        {
            NeuronIndex = neuronIndex;
        }

        public int NeuronIndex { get; }
    }
}

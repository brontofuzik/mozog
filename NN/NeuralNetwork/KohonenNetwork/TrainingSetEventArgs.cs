using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public delegate void TrainingSetEventHandler(object sender, TrainingSetEventArgs trainingSetEventArgs);

    public class TrainingSetEventArgs
        : EventArgs
    {
        public TrainingSetEventArgs(TrainingSet trainingSet, int trainingIterationIndex)
        {
            _trainingSet = trainingSet;
            _trainingIterationIndex = trainingIterationIndex;
        }

        public TrainingSet TrainingSet
        {
            get
            {
                return _trainingSet;
            }
        }

        public int TrainingIterationIndex
        {
            get
            {
                return _trainingIterationIndex;
            }
        }

        private TrainingSet _trainingSet;

        private int _trainingIterationIndex;
    }
}

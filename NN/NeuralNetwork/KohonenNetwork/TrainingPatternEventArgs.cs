using NeuralNetwork.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public delegate void TrainingPatternEventhandler(object sender, TrainingPatternEventArgs trainingPatternEventArgs);

    public class TrainingPatternEventArgs
    {
        public TrainingPatternEventArgs(SupervisedTrainingPattern trainingPattern, int trainingIterationIndex)
        {
            _trainingPattern = trainingPattern;
            _trainingIterationIndex = trainingIterationIndex;
        }

        public SupervisedTrainingPattern TrianingPattern
        {
            get
            {
                return _trainingPattern;
            }
        }

        public int TrainingIterationIndex
        {
            get
            {
                return _trainingIterationIndex;
            }
        }

        private SupervisedTrainingPattern _trainingPattern;

        private int _trainingIterationIndex;
    }
}

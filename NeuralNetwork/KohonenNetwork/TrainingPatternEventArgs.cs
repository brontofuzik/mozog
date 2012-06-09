using NeuralNetwork.MultilayerPerceptron.Training;
namespace NeuralNetwork.KohonenNetwork
{
    public delegate void TrainingPatternEventhandler(object sender, TrainingPatternEventArgs trainingPatternEventArgs);

    public class TrainingPatternEventArgs
    {
        #region Public members

        #region Instance constructors

        public TrainingPatternEventArgs(SupervisedTrainingPattern trainingPattern, int trainingIterationIndex)
        {
            _trainingPattern = trainingPattern;
            _trainingIterationIndex = trainingIterationIndex;
        }

        #endregion // Instance constructors

        #region Instance properties

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

        #endregion // Instance properties

        #endregion // Public members


        #region Private members

        #region Instance fields

        private SupervisedTrainingPattern _trainingPattern;

        private int _trainingIterationIndex;

        #endregion // Instance fields

        #endregion // Private members
    }
}

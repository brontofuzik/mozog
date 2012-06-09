using System;

using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public delegate void TrainingSetEventHandler(object sender, TrainingSetEventArgs trainingSetEventArgs);

    public class TrainingSetEventArgs
        : EventArgs
    {
        #region Public members

        #region Instance constructors

        public TrainingSetEventArgs(TrainingSet trainingSet, int trainingIterationIndex)
        {
            _trainingSet = trainingSet;
            _trainingIterationIndex = trainingIterationIndex;
        }

        #endregion // Instance constructors

        #region Instance properties

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

        #endregion // Instance properties

        #endregion // Public members


        #region Private members

        #region Instance fields

        private TrainingSet _trainingSet;

        private int _trainingIterationIndex;

        #endregion // Instance fields 

        #endregion // Private members
    }
}

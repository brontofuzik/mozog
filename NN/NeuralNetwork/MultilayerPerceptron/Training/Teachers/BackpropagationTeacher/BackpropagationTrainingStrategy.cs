using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher.Decorators;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation training strategy.
    /// </summary>
    public class BackpropagationTrainingStrategy
    {
        #region Public members

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxIterationCount"></param>
        /// <param name="maxNetworkError"></param>
        /// <param name="batchLearning"></param>
        /// <param name="synapseLearningRate"></param>
        public BackpropagationTrainingStrategy( int maxIterationCount, double maxNetworkError, bool batchLearning, double synapseLearningRate, double connectorMomentum )
        {
            _maxIterationCount = maxIterationCount;
            _maxNetworkError = maxNetworkError;
            _batchLearning = batchLearning;
            _synapseLearningRate = synapseLearningRate;
            _connectorMomentum = connectorMomentum;
        }

        #endregion // Public instance constructors

        #region Public instance properties

        /// <summary>
        /// 
        /// </summary>
        public int MaxIterationCount
        {
            get
            {
                return _maxIterationCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MaxNetworkError
        {
            get
            {
                return _maxNetworkError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool BatchLearning
        {
            get
            {
                return _batchLearning;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double SynapseLearningRate
        {
            get
            {
                return _synapseLearningRate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double ConnectorMomentum
        {
            get
            {
                return _connectorMomentum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BackpropagationNetwork BackpropagationNetwork
        {
            get
            {
                return _backpropagationNetwork;
            }
            set
            {
                _backpropagationNetwork = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TrainingSet TrainingSet
        {
            get
            {
                return _trainingSet;
            }
            set
            {
                _trainingSet = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable< SupervisedTrainingPattern > TrainingPatterns
        {
            get
            {
                foreach (SupervisedTrainingPattern trainingPattern in _trainingSet)
                {
                    yield return trainingPattern;
                }
            }
        }

        #endregion // Public instance properties

        #region Public instance methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evaluationIterationCount"></param>
        /// <param name="networkError"></param>
        /// <returns></returns>
        public virtual bool IsStoppingCriterionMet( int iterationCount, double networkError )
        {
            return (iterationCount >= _maxIterationCount) || (networkError <= _maxNetworkError);
        }

        #endregion // Public instance methods

        #endregion // Public members

        #region Private members

        #region Private instance fields

        /// <summary>
        /// The maximal number of iterations.
        /// </summary>
        private int _maxIterationCount;

        /// <summary>
        /// The maximal network error.
        /// </summary>
        private double _maxNetworkError;

        /// <summary>
        /// Bacth vs. incremental learning.
        /// </summary>
        private bool _batchLearning;

        /// <summary>
        /// 
        /// </summary>
        private double _synapseLearningRate;

        /// <summary>
        /// 
        /// </summary>
        private double _connectorMomentum;

        /// <summary>
        /// The backpropagation network.
        /// </summary>
        private BackpropagationNetwork _backpropagationNetwork;

        /// <summary>
        /// The training set.
        /// </summary>
        private TrainingSet _trainingSet;

        #endregion // Private instance fields

        #endregion // Private members
    }
}

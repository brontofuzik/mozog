using System;

using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training
{
    /// <summary>
    /// A training log.
    /// </summary>
    public class TrainingLog
    {
        /// <summary>
        /// Creates a new training log.
        /// </summary>
        /// <param name="evaluationIterationCount">The number of iterations used (in the best case).</param>
        /// <param name="networkError">The minim network error achieved (in the best case).</param>
        public TrainingLog(int iterationCount, double networkError)
        {
            _iterationCount = iterationCount;
            _networkError = networkError;
        }

        /// <summary>
        /// Calculates (and logs) the network's measures of fit.
        /// </summary>
        /// <param name="network">The network whose measures of fit are to be calculated (and logged).</param>
        /// <param name="trainingSet">The training set.</param>
        public void CalculateMeasuresOfFit(INetwork network, TrainingSet trainingSet)
        {
            int n = trainingSet.Size;
            int p = network.SynapseCount;

            // Calculate (and log) the residual sum of squares (within-sample).
            _rss_trainingSet = CalculateRSS(network, trainingSet);

            // Calculate (and log) the residual standard deviation (within-sample).
            _rsd_trainingSet = Math.Sqrt(_rss_trainingSet / (double)n);

            // Calculate (and log) the Akaike information criterion.
            _aic = n * Math.Log(_rss_trainingSet / (double)n) + 2 * p;

            // Calcuolate (and log) the bias-corrected Akaike information criterion.
            _aicc = _aic + 2 * (p + 1) * (p + 2) / (n - p - 2);

            // Calculate (and log) the Bayesian information criterion.
            _bic = n * Math.Log(_rss_trainingSet / (double)n) + p + p * Math.Log(n);

            // Calculate (and log) the Schwarz Bayesian criterion.
            sbc = n * Math.Log(_rss_trainingSet / (double)n) + p * Math.Log(n);
        }

        /// <summary>
        /// Calculates (and logs) the network's forecast accuracy.
        /// </summary>
        /// <param name="network">The network whose forecast accuracy is to be calculated (and logged).</param>
        /// <param name="testSet">The test set.</param>
        public void CalculateForecastAccuracy(INetwork network, TrainingSet testSet)
        {
            int n = testSet.Size;
            int p = network.SynapseCount;

            // Calculate the residual sum of squares (out-of-sample).
            rss_testSet = CalculateRSS(network, testSet);

            // Calculate the residual standard deviation (out-of-sample).
            rsd_testSet = Math.Sqrt(rss_testSet / (double)n);
        }

        /// <summary>
        /// Gets or sets the number of iterations used.
        /// </summary>
        /// <value>
        /// The number of iterations used.
        /// </value>
        public int IterationCount
        {
            get
            {
                return _iterationCount;
            }
            set
            {
                _iterationCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum network error achieved.
        /// </summary>
        /// <value>
        /// The minimum network error achieved.
        /// </value>
        public double NetworkError
        {
            get
            {
                return _networkError;
            }
            set
            {
                _networkError = value;
            }
        }

        /// <summary>
        /// Gets the residual sum of squares (within-sample).
        /// </summary>
        /// <value>
        /// The residual sum of squares (within-sample).
        /// </value>
        public double RSS_TrainingSet
        {
            get
            {
                return _rss_trainingSet;
            }
        }

        /// <summary>
        /// Gets the residual standard deviation (within-sample).
        /// </summary>
        /// <value>
        /// The residual standard deviation (within-sample).
        /// </value>
        public double RSD_TrainingSet
        {
            get
            {
                return _rsd_trainingSet;
            }
        }

        /// <summary>
        /// Gets the Akaike information criterion.
        /// </summary>
        /// <value>
        /// The Akaike information criterion.
        /// </value>
        public double AIC
        {
            get
            {
                return _aic;
            }
        }

        /// <summary>
        /// Gets the bias-corrected Akaike information criterion.
        /// </summary>
        /// <value>
        /// The bias-corrected Akaike information criterion.
        /// </value>
        public double AICC
        {
            get
            {
                return _aicc;
            }
        }

        /// <summary>
        /// Gets the Bayesian information criterion.
        /// </summary>
        /// <value>
        /// The Bayesian information criterion.
        /// </value>
        public double BIC
        {
            get
            {
                return _bic;
            }
        }

        /// <summary>
        /// Gets the Schwarz Bayesian criterion.
        /// </summary>
        /// <value>
        /// The Schwarz Bayesian criterion.
        /// </value>
        public double SBC
        {
            get
            {
                return sbc;
            }
        }

        /// <summary>
        /// Gets the residual sum of squares (out-of-sample).
        /// </summary>
        /// <value>
        /// The residual sum of squares (out-of-sample).
        /// </value>
        public double RSS_TestSet
        {
            get
            {
                return rss_testSet;
            }
        }

        /// <summary>
        /// Gets the residual standard deviation (out-of-sample).
        /// </summary>
        /// <value>
        /// The residual standard deviation (out-of-sample).
        /// </value>
        public double RSD_TestSet
        {
            get
            {
                return rsd_testSet;
            }
        }

        /// <summary>
        /// Calculates the residual sum of squares (RSS) of a given network WRT a given training set.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The RSS.
        /// </returns>
        private double CalculateRSS(INetwork network, TrainingSet trainingSet)
        {
            double rss = 0.0;
            foreach (SupervisedTrainingPattern trainingPattern in trainingSet)
            {
                double[] outputVector = network.Evaluate(trainingPattern.InputVector);
                double[] desiredOutputVector = trainingPattern.OutputVector;

                rss += Math.Pow(outputVector[0] - desiredOutputVector[0], 2);
            }
            return rss;
        }

        /// <summary>
        /// The number of iterations used.
        /// </summary>
        private int _iterationCount;

        /// <summary>
        /// The minimum network error achieved.
        /// </summary>
        private double _networkError;

        /// <summary>
        /// The residual sum of squares (within-sample).
        /// </summary>
        private double _rss_trainingSet;

        /// <summary>
        /// The residual standard deviation (within-sample).
        /// </summary>
        private double _rsd_trainingSet;

        /// <summary>
        /// The Akaike information criterion.
        /// </summary>
        private double _aic;

        /// <summary>
        /// The bias-corrected Akaike information criterion.
        /// </summary>
        private double _aicc;

        /// <summary>
        /// The Bayesian information criterion.
        /// </summary>
        private double _bic;

        /// <summary>
        /// The Schwarz Bayesian criterion.
        /// </summary>
        private double sbc;

        /// <summary>
        /// The residual sum of squares (out-of-sample).
        /// </summary>
        private double rss_testSet;

        /// <summary>
        /// The residual standard deviation (out-of-sample).
        /// </summary>
        private double rsd_testSet;
    }
}

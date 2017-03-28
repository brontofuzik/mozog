using System.Collections.Generic;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainingStrategy
    {
        public BackpropagationTrainingStrategy(int maxIterationCount, double maxNetworkError, bool batchLearning, double synapseLearningRate, double connectorMomentum)
        {
            MaxIterationCount = maxIterationCount;
            MaxNetworkError = maxNetworkError;
            BatchLearning = batchLearning;
            SynapseLearningRate = synapseLearningRate;
            ConnectorMomentum = connectorMomentum;
        }

        public int MaxIterationCount { get; }

        public double MaxNetworkError { get; }

        public bool BatchLearning { get; }

        public double SynapseLearningRate { get; }

        public double ConnectorMomentum { get; }

        public BackpropagationNetwork BackpropagationNetwork { get; set; }

        public TrainingSet TrainingSet { get; set; }

        public virtual IEnumerable<SupervisedTrainingPattern> TrainingPatterns
        {
            get
            {
                foreach (var trainingPattern in TrainingSet)
                {
                    yield return trainingPattern;
                }
            }
        }

        public virtual bool IsStoppingCriterionMet(int iterationCount, double networkError)
            => iterationCount >= MaxIterationCount || networkError <= MaxNetworkError;
    }
}

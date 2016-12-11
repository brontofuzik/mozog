using SimulatedAnnealing;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.SimulatedAnnealingTeacher
{
    internal class NetworkObjectiveFunction
        : ObjectiveFunction< double >
    {
        #region Private instance fields

        private INetwork network;

        private TrainingSet trainingSet;

        #endregion // Private insatnce fields

        #region Public instance constructors

        public NetworkObjectiveFunction(INetwork network, TrainingSet trainingSet)
            : base(network.SynapseCount, Objective.MINIMIZE)
        {
            this.network = network;
            this.trainingSet = trainingSet;
        }

        #endregion // Public instance constructors

        #region Public insatnce methods

        public override double Evaluate( double[] weights )
        {
            network.SetWeights( weights );
            return network.CalculateError( trainingSet );
        }

        #endregion // Public instance methods
    }
}
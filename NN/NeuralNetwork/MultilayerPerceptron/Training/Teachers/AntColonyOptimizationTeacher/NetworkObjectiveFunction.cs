using AntColonyOptimization;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.AntColonyOptimizationTeacher
{
    internal class NetworkObjectiveFunction
        : ObjectiveFunction
    {
        private INetwork network;

        private TrainingSet trainingSet;

        public NetworkObjectiveFunction( INetwork network, TrainingSet trainingSet )
            : base( network.SynapseCount, Objective.MINIMIZE )
        {
            this.network = network;
            this.trainingSet = trainingSet;
        }

        public override double Evaluate( double[] weights )
        {
            network.SetWeights( weights );
            return network.CalculateError( trainingSet );    
        }
    }
}

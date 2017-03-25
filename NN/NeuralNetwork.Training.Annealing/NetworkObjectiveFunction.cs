using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training;
using SimulatedAnnealing;

namespace NeuralNetwork.Training.Annealing
{
    internal class NetworkObjectiveFunction : ObjectiveFunction<double>
    {
        private readonly INetwork network;

        private readonly TrainingSet trainingSet;

        public NetworkObjectiveFunction(INetwork network, TrainingSet trainingSet)
            : base(network.SynapseCount, Objective.Minimize)
        {
            this.network = network;
            this.trainingSet = trainingSet;
        }

        public override double Evaluate(double[] weights)
        {
            network.SetWeights(weights);
            return network.CalculateError(trainingSet);
        }
    }
}
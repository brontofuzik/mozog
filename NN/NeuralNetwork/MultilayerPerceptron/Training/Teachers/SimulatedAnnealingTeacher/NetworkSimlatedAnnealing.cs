using System;

using SimulatedAnnealing;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.SimulatedAnnealingTeacher
{
    /// <summary>
    /// A simulated annealing designed to train a neural network.
    /// </summary>
    internal class NetworkSimlatedAnnealing
        : SimulatedAnnealing< double >
    {
        /// <summary>
        /// The generator function.
        /// (= The random weights generator function.)
        /// </summary>
        /// <returns>
        /// A random state of the system.
        /// (= Random weights of the network.)
        /// </returns>
        protected override double[] GeneratorFunction()
        {
            double[] weights = new double[ Dimension ];
            for (int i = 0; i < Dimension; i++)
            {
                weights[ i ] = random.NextDouble() + random.Next( -20, +20 );
            }
            return weights;
        }

        /// <summary>
        /// The perturbation (a.k.a. neighbour) function.
        /// </summary>
        /// <param name="currentWeights">The current state of the system. (= The current weights of the network.)</param>
        /// <returns>
        /// The new state of the system.
        /// (= The new weights of the network.)
        /// </returns>
        protected override double[] PerturbationFunction( double[] currentWeights )
        {
            double[] newWeights = new double[ Dimension ];
            Array.Copy( currentWeights, newWeights, Dimension );

            int index = random.Next( 0, Dimension );
            newWeights[ index ] = (newWeights[ index ] + (random.NextDouble() + random.Next( -20, +20 ))) / 2.0;
            
            return newWeights;
        }
    }
}

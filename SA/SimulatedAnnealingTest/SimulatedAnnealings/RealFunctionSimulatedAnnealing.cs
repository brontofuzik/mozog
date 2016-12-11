using System;

using SimulatedAnnealing;

namespace SimulatedAnnealingTest.SimulatedAnnealings
{
    internal class RealFunctionSimulatedAnnealing
        : SimulatedAnnealing< double >
    {
        #region Protected instance methods

        protected override double[] GeneratorFunction()
        {
            double[] state = new double[ Dimension ];
            for (int i = 0; i < Dimension; i++)
            {
                state[i] = random.NextDouble() + random.Next( -20, +20 );
            }
            return state;
        }

        protected override double[] PerturbationFunction( double[] currentState )
        {
            double[] newState = new double[ Dimension ];
            Array.Copy( currentState, newState, Dimension );

            int index = random.Next( 0, Dimension);
            newState[ index ] = (newState[ index ] + (random.NextDouble() + random.Next( -20, +20 ))) / 2.0;

            return newState;
        }

        #endregion // Protected instance methods
    }
}

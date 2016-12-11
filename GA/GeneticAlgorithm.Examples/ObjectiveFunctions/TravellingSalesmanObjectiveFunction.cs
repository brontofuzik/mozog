using System.Collections.Generic;

using GeneticAlgorithm;

namespace GeneticAlgorithm.Examples.ObjectiveFunctions
{
    internal class TravellingSalesmanObjectiveFunction
        : ObjectiveFunction< char >
    {
        #region Public instance constructors

        public TravellingSalesmanObjectiveFunction()
            :base( 4, Objective.MINIMIZE )
        {
        }

        #endregion // Public instance constructors

        #region Public instance methods

        public override double Evaluate( char[] genes )
        {
            Dictionary< char, Dictionary< char, double > > distances = new Dictionary< char, Dictionary< char, double > >();
            distances[ 'A' ] = new Dictionary< char, double >();
            distances[ 'A' ][ 'A' ] = 0.0;                     distances[ 'A' ][ 'B' ] = 20.0;                    distances[ 'A' ][ 'C' ] = 42.0;                    distances[ 'A' ][ 'D' ] = 35.0;
            distances[ 'B' ] = new Dictionary< char, double >();
            distances[ 'B' ][ 'A' ] = distances[ 'A' ][ 'B' ]; distances[ 'B' ][ 'B' ] =  0.0;                    distances[ 'B' ][ 'C' ] = 30.0;                    distances[ 'B' ][ 'D' ] = 34.0;
            distances[ 'C' ] = new Dictionary< char, double >();
            distances[ 'C' ][ 'A' ] = distances[ 'A' ][ 'C' ]; distances[ 'C' ][ 'B' ] = distances[ 'B' ][ 'C' ]; distances[ 'C' ][ 'C' ] =  0.0;                    distances[ 'C' ][ 'D' ] = 12.0;
            distances[ 'D' ] = new Dictionary< char, double >();
            distances[ 'D' ][ 'A' ] = distances[ 'A' ][ 'D' ]; distances[ 'D' ][ 'B' ] = distances[ 'B' ][ 'D' ]; distances[ 'D' ][ 'C' ] = distances[ 'C' ][ 'D' ]; distances[ 'D' ][ 'D' ] =  0.0;

            double totalDistance = 0.0;
            for (int i = 0; i < 4; i++)
            {
                // The origin city. 
                char originCity = genes[ i ];

                // The destination city.
                int destinationCityIndex = ((i + 1) < Dimension) ? (i + 1) : 0;
                char destinationCity = genes [destinationCityIndex ];

                // Add the distance from the origin to the destination city to the total distance.
                totalDistance += distances[ originCity ][ destinationCity ];
            }

            return totalDistance;
        }

        #endregion // Public instance methods
    }
}
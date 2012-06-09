using GeneticAlgorithm;

namespace GeneticAlgorithmTest.ObjectiveFunctions
{
    internal class KnapsackProblemObjectiveFunction
        : ObjectiveFunction< int >
    {
        #region Public instance constructors

        public KnapsackProblemObjectiveFunction()
            : base( 5, Objective.MAXIMIZE )
        {
        }

        #endregion // Public insatnce constructors

        #region Public instance methods

        public override double Evaluate( int[] genes )
        {
            double[,] itemValuesAndWeights = new double[ 5, 2 ];
            itemValuesAndWeights[ 0, 0 ] =  1.0; itemValuesAndWeights[ 0, 1 ] =  1.0;
            itemValuesAndWeights[ 1, 0 ] =  2.0; itemValuesAndWeights[ 1, 1 ] =  1.0;
            itemValuesAndWeights[ 2, 0 ] =  2.0; itemValuesAndWeights[ 2, 1 ] =  2.0;
            itemValuesAndWeights[ 3, 0 ] =  4.0; itemValuesAndWeights[ 3, 1 ] = 12.0;
            itemValuesAndWeights[ 4, 0 ] = 10.0; itemValuesAndWeights[ 4, 1 ] =  4.0;

            double totalValue = 0.0;
            double totalWeight = 0.0;
            for (int i = 0; i < 5; i++)
            {
                totalValue += (genes[ i ] == 1) ? itemValuesAndWeights[ i, 0 ] : 0.0;
                totalWeight += (genes[ i ] == 1) ? itemValuesAndWeights[ i, 1 ] : 0.0;
            }

            return (totalWeight <= 15.0) ? totalValue : 0.0;
        }

        #endregion // Public instance methods
    }
}

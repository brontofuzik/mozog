using System;

namespace NeuralNetwork.Data
{
    public class DataStatistics
    {
        private readonly int n;
        private readonly int p;

        public DataStatistics(int n, int p)
        {
            this.n = n;
            this.p = p;
        }

        public void AddObservation(double[] output, double[] target, double error)
        {
            TotalError += error;
            RSS += Math.Pow(output[0] - target[0], 2);
        }

        public double TotalError { get; private set; }

        public double AverageError => TotalError / n;

        // Residual sum of squares
        public double RSS { get; private set; }

        // Mean squared error
        public double MSE => RSS / n;

        // RMSE/RMSD a.k.a. RSD
        public double RMSE => Math.Sqrt(MSE);

        // Akaike information criterion
        public double AIC => n * Math.Log(MSE) + 2 * p;

        // Bias-corrected Akaike information criterion
        public double AICC => AIC + 2 * (p + 1) * (p + 2) / (n - p - 2);

        // Bayesian information criterion
        public double BIC => n * Math.Log(MSE) + p * Math.Log(n);

        public override string ToString() => AverageError.ToString("F3");
    }
}

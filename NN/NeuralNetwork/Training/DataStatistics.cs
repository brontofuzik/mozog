using System;

namespace NeuralNetwork.Training
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

        public double Error { get; set; }

        // Residual sum of squares
        public double RSS { get; set; }

        // Residual standard deviation
        public double RSD => Math.Sqrt(RSS / (n - 2));

        // Mean squared error
        public double MSE => RSS / n;

        // Akaike information criterion
        public double AIC => n * Math.Log(MSE) + 2 * p;

        // Bias-corrected Akaike information criterion
        public double AICC => AIC + 2 * (p + 1) * (p + 2) / (n - p - 2);

        // Bayesian information criterion
        public double BIC => n * Math.Log(MSE) + p * Math.Log(n);
    }
}

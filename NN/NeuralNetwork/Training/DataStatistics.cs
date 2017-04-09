using System;

namespace NeuralNetwork.Training
{
    public struct DataStatistics
    {
        private readonly int n;
        private readonly int p;

        public DataStatistics(int n, int p, double error, double rss)
        {
            this.n = n;
            this.p = p;
            Error = error;
            RSS = rss;
        }

        public double Error { get; }

        // Residual sum of squares
        public double RSS { get; }

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

        public override string ToString() => Error.ToString("F3");
    }
}

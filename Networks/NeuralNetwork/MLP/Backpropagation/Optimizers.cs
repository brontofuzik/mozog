using System;

namespace NeuralNetwork.MLP.Backpropagation
{
    public interface IOptimizer
    {
        double AdjustWeight(double weight, double gradient, int iteration);

        void Reset();
    }

    public class Optimizer : IOptimizer
    {
        #region Factories

        public static Func<IOptimizer> Default(double learningRate = 0.05) => () => new Optimizer(learningRate);

        public static Func<IOptimizer> Momentum(double learningRate = 0.05) => () => new Momentum(learningRate);

        public static Func<IOptimizer> AdaptiveLR(double learningRate = 0.05) => () => new AdaptiveLearningRate(learningRate);

        public static Func<IOptimizer> RmsProp(double learningRate = 0.001) => () => new RmsProp(learningRate);

        public static Func<IOptimizer> Adam(double learningRate = 0.001) => () => new Adam(learningRate);

        #endregion // Factories

        // Non-adaptive learning rate
        private readonly double learningRate;

        public Optimizer(double learningRate = 0.05)
        {
            this.learningRate = learningRate;
        }

        public virtual double AdjustWeight(double weight, double gradient, int iteration)
            => weight - learningRate * gradient;

        public virtual void Reset()
        {
            // Do nothing.
        }
    }

    // Classic momentum
    internal class Momentum : IOptimizer
    {
        private readonly double learningRate;
        private readonly double momentum;

        private double previousWeightDelta;
        private double weightDelta;

        public Momentum(double learningRate = 0.05, double momentum = 0.9)
        {
            this.learningRate = learningRate;
            this.momentum = momentum;
        }

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            previousWeightDelta = weightDelta;
            weightDelta = -learningRate * gradient + momentum * previousWeightDelta;
            return weight + weightDelta;
        }

        public void Reset()
        {
            previousWeightDelta = 0.0;
            weightDelta = 0.0;
        }
    }

    // Simple speeding up/slowing down
    internal class AdaptiveLearningRate : IOptimizer
    {
        private readonly double initialLearningRate;

        private double learningRate;
        private double previousWeightDelta;
        private double weightDelta;

        public AdaptiveLearningRate(double initialLearningRate = 0.05)
        {
            this.learningRate = this.initialLearningRate = initialLearningRate;
        }

        private bool SpeedUp => previousWeightDelta * weightDelta >= 0;

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            learningRate = SpeedUp
                ? Math.Min(learningRate * 1.01, 1.0) // Speed up
                : Math.Max(learningRate / 2.0, 0.001);

            previousWeightDelta = weightDelta;
            weightDelta = -learningRate * gradient;
            return weight + weightDelta;
        }

        public void Reset()
        {
            learningRate = initialLearningRate;
            previousWeightDelta = 0.0;
            weightDelta = 0.0;
        }
    }

    // RMSprop (Hinton, Tieleman)
    internal class RmsProp : IOptimizer
    {
        private const double e = 1e-8;

        private readonly double learningRate;

        // gamma
        private readonly double g;

        // Mean-squared gradient
        private double msg;

        public RmsProp(double learningRate = 0.001, double g = 0.9)
        {
            this.learningRate = learningRate;
            this.g = g;
        }

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            msg = g * msg + (1 - g) * Math.Pow(gradient, 2);
            return weight - learningRate * gradient / Math.Sqrt(msg + e);
        }

        public void Reset()
        {
            msg = 0.0;
        }
    }

    // Adam (Kingma, Ba)
    internal class Adam : IOptimizer
    {
        private const double e = 1e-8;

        private readonly double learningRate;

        // First moment decay rate (beta1)
        private readonly double b1;

        //Second moment decat rate (beta2)
        private readonly double b2;

        // Biased first moment estimate (mean)
        private double m;

        // Biased second moment estimate (variance)
        private double v;

        public Adam(double learningRate = 0.001, double b1 = 0.9, double b2 = 0.999)
        {
            this.learningRate = learningRate;
            this.b1 = b1;
            this.b2 = b2;
        }

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            m = b1 * m + (1 - b1) * gradient;
            v = b2 * v + (1 - b2) * Math.Pow(gradient, 2);

            // Bias-corrected first moment estimate
            var mC = m / (1 - Math.Pow(b1, iteration));

            // Biac-corrected second moment estimate
            var vC = v / (1 - Math.Pow(b2, iteration));

            return weight - learningRate * mC / (Math.Sqrt(vC) + e);
        }

        public void Reset()
        {
            m = 0.0;
            v = 0.0;
        }
    }
}
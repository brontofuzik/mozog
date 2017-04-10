using System;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public interface IOptimizer
    {
        double AdjustWeight(double weight, double gradient, double learningRate, int iteration);

        double AdaptLearningRate(double learningRate, int iteration);

        void Reset();
    }

    public class Optimizer : IOptimizer
    {
        public static Func<IOptimizer> Default => () => new Optimizer();

        public static Func<IOptimizer> Momentum => () => new MomentumOptimizer();

        public static Func<IOptimizer> RmsProp => () => new RmsPropOptimizer();

        public static Func<IOptimizer> Adam => () => new AdamOptimizer();

        public virtual double AdjustWeight(double weight, double gradient, double learningRate, int iteration)
            => weight - learningRate * gradient;

        public virtual double AdaptLearningRate(double learningRate, int iteration) => learningRate;

        public virtual void Reset()
        {
            // Do nothing.
        }
    }

    /// <summary>
    /// Momentum & adaptive learning rate
    /// </summary>
    internal class MomentumOptimizer : IOptimizer
    {
        private readonly double momentum;
        private double previousWeightDelta = Double.Epsilon;
        private double weightDelta;

        public MomentumOptimizer(double momentum = 0.9)
        {
            this.momentum = momentum;
        }

        private bool SpeedUp => previousWeightDelta * weightDelta > 0;

        public double AdjustWeight(double weight, double gradient, double learningRate, int iteration)
        {
            previousWeightDelta = weightDelta;
            weightDelta = -learningRate * gradient + momentum * previousWeightDelta;
            return weight + weightDelta;
        }

        public double AdaptLearningRate(double learningRate, int interation)
            => SpeedUp
            ? Math.Min(learningRate * 1.01, 1.0) // Speed up
            : Math.Max(learningRate / 2.0, 0.001);

        public void Reset()
        {
            previousWeightDelta = 0.0;
            weightDelta = 0.0;
        }
    }

    // RMSprop (Hinton)
    internal class RmsPropOptimizer : IOptimizer
    {
        private const double epsilon = 10e-8;

        private readonly double gamma;
        private double meanSquaredGradient;

        public RmsPropOptimizer(double gamma = 0.9)
        {
            this.gamma = gamma;
        }

        public double AdjustWeight(double weight, double gradient, double learningRate, int iteration)
        {
            meanSquaredGradient = gamma * meanSquaredGradient + (1 - gamma) * Math.Pow(gradient, 2);
            return weight - learningRate / Math.Sqrt(meanSquaredGradient + epsilon) * gradient;
        }

        // Dont' adapt
        public double AdaptLearningRate(double learningRate, int iteration) => learningRate;

        public void Reset()
        {
            meanSquaredGradient = 0.0;
        }
    }

    // Adam (Kingma, Ba)
    internal class AdamOptimizer : IOptimizer
    {
        private const double epsilon = 10e-8;

        private readonly double beta1;
        private readonly double beta2;

        private double momentum;
        private double velocity;

        public AdamOptimizer(double beta1 = 0.9, double beta2 = 0.999)
        {
            this.beta1 = beta1;
            this.beta2 = beta2;
        }

        public double AdjustWeight(double weight, double gradient, double learningRate, int iteration)
        {
            momentum = beta1 * momentum + (1 - beta1) * gradient;
            velocity = beta2 * velocity + (1 - beta2) * gradient;

            var momentumHat = momentum / (1 - Math.Pow(beta1, iteration));
            var velocityHat = velocity / (1 - Math.Pow(beta2, iteration));

            return weight - learningRate / (Math.Sqrt(velocityHat) + epsilon) * momentumHat;
        }

        // Don't adapt
        public double AdaptLearningRate(double learningRate, int iteration) => learningRate;

        public void Reset()
        {
            momentum = 0.0;
            velocity = 0.0;
        }
    }
}
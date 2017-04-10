using System;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public interface IOptimizer
    {
        double AdjustWeight(double weight, double gradient, int iteration);

        void Reset();
    }

    public class Optimizer : IOptimizer
    {
        #region Factories

        public static Func<IOptimizer> Default(double learningRate = 0.01) => () => new Optimizer(learningRate);

        public static Func<IOptimizer> Momentum(double learningRate = 0.01) => () => new MomentumOptimizer(learningRate);

        public static Func<IOptimizer> RmsProp(double learningRate = 0.01) => () => new RmsPropOptimizer(learningRate);

        public static Func<IOptimizer> Adam(double learningRate = 0.01) => () => new AdamOptimizer(learningRate);

        #endregion // Factories

        // Non-adaptive learning rate
        private readonly double learningRate;

        public Optimizer(double learningRate = 0.01)
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

    /// <summary>
    /// Momentum & adaptive learning rate
    /// </summary>
    internal class MomentumOptimizer : IOptimizer
    {
        private const double epsilon = 10e-6;

        private readonly double momentum;
        private readonly double initialLearningRate;

        private double learningRate;
        private double previousWeightDelta = epsilon;
        private double weightDelta = epsilon;

        public MomentumOptimizer(double initialLearningRate = 0.01, double momentum = 0.9)
        {
            this.learningRate = this.initialLearningRate = initialLearningRate;
            this.momentum = momentum;
        }

        private bool SpeedUp => previousWeightDelta * weightDelta > 0;

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            learningRate = SpeedUp
                ? Math.Min(learningRate * 1.01, 1.0) // Speed up
                : Math.Max(learningRate / 2.0, 0.001);

            previousWeightDelta = weightDelta;
            weightDelta = -learningRate * gradient + momentum * previousWeightDelta;
            return weight + weightDelta;
        }

        public void Reset()
        {
            learningRate = initialLearningRate;
            previousWeightDelta = 0.0;
            weightDelta = 0.0;
        }
    }

    // RMSprop (Hinton)
    internal class RmsPropOptimizer : IOptimizer
    {
        private const double epsilon = 10e-6;

        private readonly double _learningRate;
        private readonly double gamma;

        private double meanSquaredGradient;

        public RmsPropOptimizer(double learningRate = 0.01, double gamma = 0.9)
        {
            this._learningRate = learningRate;
            this.gamma = gamma;
        }

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            meanSquaredGradient = gamma * meanSquaredGradient + (1 - gamma) * Math.Pow(gradient, 2);
            var effectiveLearningRate = _learningRate / Math.Sqrt(meanSquaredGradient + epsilon);
            return weight - effectiveLearningRate * gradient;
        }

        public void Reset()
        {
            meanSquaredGradient = 0.0;
        }
    }

    // Adam (Kingma, Ba)
    internal class AdamOptimizer : IOptimizer
    {
        private const double epsilon = 10e-6;

        private readonly double learningRate;
        private readonly double beta1;
        private readonly double beta2;

        private double momentum;
        private double velocity;

        public AdamOptimizer(double learningRate = 0.01, double beta1 = 0.9, double beta2 = 0.999)
        {
            this.learningRate = learningRate;
            this.beta1 = beta1;
            this.beta2 = beta2;
        }

        public double AdjustWeight(double weight, double gradient, int iteration)
        {
            momentum = beta1 * momentum + (1 - beta1) * gradient;
            velocity = beta2 * velocity + (1 - beta2) * gradient;

            var momentumHat = momentum / (1 - Math.Pow(beta1, iteration));
            var velocityHat = velocity / (1 - Math.Pow(beta2, iteration));

            var effectiveLearningRate = learningRate / (Math.Sqrt(velocityHat) + epsilon);
            return weight - effectiveLearningRate * momentumHat;
        }

        public void Reset()
        {
            momentum = 0.0;
            velocity = 0.0;
        }
    }
}
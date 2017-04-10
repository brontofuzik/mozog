using System;
using Mozog.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationSynapse : Synapse
    {
        private IOptimizer optimizer;

        private double learningRate;
        private double gradient;

        #region Construction

        internal BackpropagationSynapse()
        {
        }

        #endregion // Construction

        public new BackpropagationNeuron TargetNeuron => base.TargetNeuron as BackpropagationNeuron;

        public void Initialize(BackpropagationArgs args)
        {
            base.Initialize();

            learningRate = args.LearningRate;
            optimizer = new MomentumOptimizer(args.Momentum);
            gradient = 0.0;
        }

        public void ResetGradient()
        {
            gradient = 0.0;
        }

        public void Backpropagate()
        {
            gradient += SourceNeuron.Output * TargetNeuron.Error;
        }

        public void UpdateWeight(int iteration)
        {
            Weight = optimizer.AdjustWeight(Weight, gradient, learningRate, iteration);
            learningRate = optimizer.AdaptLearningRate(learningRate, iteration);
        }

        public override string ToString() => "Bp:" + base.ToString();
    }

    internal interface IOptimizer
    {
        double AdjustWeight(double weight, double gradient, double learningRate, int iteration);

        double AdaptLearningRate(double learningRate, int iteration);

        void Reset();
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
            meanSquaredGradient = gamma * meanSquaredGradient + (1 -  gamma) * Math.Pow(gradient, 2);
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
            throw new NotImplementedException();
        }
    }
}
using System;
using Mozog.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationSynapse : Synapse
    {
        private double learningRate;
        public double momentum;

        // "Gradient"
        private double gradient;
        private double weightChange;
        private double previousWeightChange;

        #region Construction

        internal BackpropagationSynapse()
        {
        }

        #endregion // Construction

        public new BackpropagationNeuron TargetNeuron => base.TargetNeuron as BackpropagationNeuron;

        private double WeightChange
        {
            get { return weightChange; }
            set
            {
                previousWeightChange = weightChange;
                weightChange = value;
            }
        }

        public void Initialize(BackpropagationArgs args)
        {
            Weight = StaticRandom.Double(-1, +1);
            learningRate = args.LearningRate;
            momentum = args.Momentum;

            gradient = 0.0;
            weightChange = 0.0;
            previousWeightChange = 0.0;
        }

        public void ResetGradient()
        {
            gradient = 0.0;
        }

        public void UpdateGradient()
        {
            gradient += SourceNeuron.Output * TargetNeuron.Error;
        }

        public void UpdateWeight()
        {
            WeightChange = -learningRate * gradient;
            Weight += weightChange + momentum * previousWeightChange;
        }

        public void UpdateLearningRate()
        {
            learningRate = previousWeightChange * weightChange > 0
                ? Math.Min(learningRate * 1.01, 1.0) // Speed up
                : Math.Max(learningRate / 2.0, 0.001); // Slow down
        }

        public override string ToString() => "Bp:" + base.ToString();
    }
}
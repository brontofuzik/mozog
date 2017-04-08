using System;
using Mozog.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationSynapse : Synapse
    {
        private double learningRate;
        public double momentum;

        // "Gradient"
        private double partialDerivative;
        private double weightChange;
        private double previousWeightChange;

        #region Construction

        internal BackpropagationSynapse()
        {
        }

        #endregion // Construction

        public new BackpropagationNeuron TargetNeuron => base.TargetNeuron as BackpropagationNeuron;

        public void Initialize(BackpropagationArgs args)
        {
            Weight = StaticRandom.Double(-1, +1);
            learningRate = args.LearningRate;
            momentum = args.Momentum;

            partialDerivative = 0.0;
            weightChange = 0.0;
            previousWeightChange = 0.0;
        }

        public void ResetPartialDerivative()
        {
            partialDerivative = 0.0;
        }

        public void UpdatePartialDerivative()
        {
            partialDerivative += SourceNeuron.Output * TargetNeuron.Error;
        }

        public void UpdateWeight()
        {
            previousWeightChange = weightChange;
            weightChange = -learningRate * partialDerivative;
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
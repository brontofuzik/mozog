namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationSynapse : Synapse
    {
        private double partialDerivative;
        private double weightChange;
        private double previousWeightChange;

        #region Construction

        internal BackpropagationSynapse()
        {
        }

        #endregion // Construction

        public double LearningRate { get; set; }

        public double Momentum { get; set; }

        public new BackpropagationNeuron TargetNeuron => (BackpropagationNeuron)base.TargetNeuron;

        public override void Initialize()
        {
            base.Initialize();

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
            weightChange = -LearningRate * partialDerivative;
            Weight += weightChange + Momentum * previousWeightChange;
        }

        // Only used during batch training
        public void UpdateLearningRate()
        {
            LearningRate = previousWeightChange * weightChange > 0
                ? LearningRate * 1.01 // Speed up
                : LearningRate / 2.0; // Slow down
        }

        public override string ToString() => "Bp:" + base.ToString();
    }
}
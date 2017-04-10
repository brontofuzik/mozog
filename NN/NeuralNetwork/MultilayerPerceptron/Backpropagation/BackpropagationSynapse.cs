namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationSynapse : Synapse
    {
        private IOptimizer optimizer;
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

            optimizer = args.OptimizerFactory();
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
            Weight = optimizer.AdjustWeight(Weight, gradient, iteration);
        }

        public override string ToString() => "Bp:" + base.ToString();
    }
}
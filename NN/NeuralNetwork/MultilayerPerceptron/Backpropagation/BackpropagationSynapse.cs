using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationSynapse : ISynapse
    {
        private double _partialDerivative;
        private double _weightChange;
        private double _previousWeightChange;
        private double _learningRate;
        private double _k;

        public BackpropagationSynapse(ISynapse synapse, IConnector parentConnector)
        {
            _k = 1.01;
        }

        // Factory
        internal BackpropagationSynapse()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _partialDerivative = 0.0;
            _weightChange = 0.0;
            _previousWeightChange = 0.0;
        }

        /// <summary>
        /// Sets the learning rate of the synapse.
        /// </summary>
        /// <param name="learningRate"></param>
        public void SetLearningRate(double learningRate)
        {
            _learningRate = learningRate;
        }

        public void ResetPartialDerivative()
        {
            _partialDerivative = 0.0;
        }

        public void UpdatePartialDerivative()
        {
            BackpropagationNeuron targetNeuron = TargetNeuron as BackpropagationNeuron;
            _partialDerivative += SourceNeuron.Output * targetNeuron.Error;
        }

        public void UpdateWeight()
        {
            // Update the previous weight change and the current weight change.
            _previousWeightChange = _weightChange;
            _weightChange = -_learningRate * _partialDerivative;

            // Update the weight.
            Weight += _weightChange + (ParentConnector as BackpropagationConnector).Momentum * _previousWeightChange;

            // Update the learning rate.

        }

        public void UpdateLearningRate()
        {
            if (_previousWeightChange * _weightChange > 0)
            {
                _learningRate *= _k;
            }
            else
            {
                _learningRate /= 2.0;
            }
        }

        public override string ToString() => "BP" + base.ToString();
    }
}
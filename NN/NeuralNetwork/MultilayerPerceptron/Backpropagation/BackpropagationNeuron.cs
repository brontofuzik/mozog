using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationNeuron : IActivationNeuron
    {
        public BackpropagationNeuron(IActivationNeuron activationNeuron, IActivationLayer parentLayer)
        {
        }

        // Factory
        internal BackpropagationNeuron()
        {
        }

        public double PartialDerivative { get; private set; }

        public double Error { get; private set; }

        public double ActivationFunctionDerivative
        {
            get
            {
                return ((ParentLayer as IActivationLayer).ActivationFunction as IDerivableActivationFunction).EvaluateDerivative(Input);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            PartialDerivative = 0.0;
            Error = 0.0;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double desiredOutput)
        {
            PartialDerivative = Output - desiredOutput;
            Error =  PartialDerivative * ActivationFunctionDerivative;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            PartialDerivative = 0.0;
            foreach (BackpropagationSynapse targetSynapse in TargetSynapses)
            {
                BackpropagationNeuron targetNeuron = targetSynapse.TargetNeuron as BackpropagationNeuron;
                PartialDerivative += targetNeuron.Error * targetSynapse.Weight;
            }

            Error = PartialDerivative * ActivationFunctionDerivative;
        }

        public override string ToString() => "BP" + base.ToString();
    }
}
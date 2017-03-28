using System;
using NeuralNetwork.HopfieldNetwork;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron.ActivationFunctions;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    class BackpropagationLayer
    {
        public BackpropagationLayer(IActivationLayer activationLayer, INetwork parnetNetwork)
        {
            // Ensure the activation function of the neuron is derivable.
            if (!(ActivationFunction is IDerivableActivationFunction))
            {
                // TODO: Throw an exception informing the client that in order for the neuron to undergo training
                // using the error backpropagation algorithm, its activation function has to be derivable
                // (i.e. it has to implement the IDerivableActivationFunction interface)
                throw new Exception();
            }

            // Decorate the neurons.
            for (int i = 0; i < NeuronCount; i++)
            {
                Neurons[i] = new BackpropagationNeuron(Neurons[i],this);
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double[] desiredOutputVector)
        {
            // Validate the arguments.
            if (desiredOutputVector == null)
            {
                throw new ArgumentNullException(nameof(desiredOutputVector));
            }

            // Validate the length of the desired output vector.
            if (desiredOutputVector.Length != NeuronCount)
            {
                throw new ArgumentException("desiredOutputException");
            }

            int i = 0;
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.Backpropagate(desiredOutputVector[i++]);
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.Backpropagate();
            }
        }

        public override string ToString() => "BP" + base.ToString();
    }
}
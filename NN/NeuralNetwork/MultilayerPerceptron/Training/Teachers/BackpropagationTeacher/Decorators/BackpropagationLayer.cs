using System;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher.Decorators
{
    /// <summary>
    /// A backpropagation layer.
    /// </summary>
    internal class BackpropagationLayer
        : ActivationLayerDecorator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer">The (activation) layer to be decorated as backpropagation (activation) layer.</param>
        /// <param name="parnetNetwork">The parnet network.</param>
        public BackpropagationLayer(IActivationLayer activationLayer, INetwork parnetNetwork)
            : base(activationLayer,parnetNetwork)
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
                Neurons[ i ] = new BackpropagationNeuron(Neurons[i],this);
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate( double[] desiredOutputVector )
        {
            // Validate the arguments.
            if (desiredOutputVector == null)
            {
                throw new ArgumentNullException( "desiredOutputVector" );
            }

            // Validate the length of the desired output vector.
            if (desiredOutputVector.Length != NeuronCount)
            {
                throw new ArgumentException( "desiredOutputException" );
            }

            int i = 0;
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.Backpropagate( desiredOutputVector[ i++ ] );
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

        /// <summary>
        /// Returns a string representation of the backpropagation layer.
        /// </summary>
        /// <returns>
        /// A string representation of the backpropagation layer.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }
    }
}
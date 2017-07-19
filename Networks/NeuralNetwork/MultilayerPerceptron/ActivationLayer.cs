using System.Collections.Generic;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationLayer : LayerBase<ActivationNeuron>, IActivationLayer
    {
        protected readonly IActivationFunction activationFunc;

        #region Construction

        internal ActivationLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
            activationFunc = layer.Activation;
        }

        protected override ActivationNeuron MakeNeuron() => new ActivationNeuron();

        #endregion // Construction

        public new IEnumerable<IActivationNeuron> Neurons => base.Neurons;

        // TODO
        public IActivationFunction1 ActivationFunc1 => activationFunc as IActivationFunction1;

        // TODO
        public IActivationFunction2 ActivationFunc2 => activationFunc as IActivationFunction2;

        public void Evaluate()
        {
            if (activationFunc is IActivationFunction1)
            {
                Evaluate1();
            }
            else
            {
                Evaluate2();
            }
        }

        private void Evaluate1()
        {
            Neurons.ForEach(n => n.Evaluate());
        }

        private void Evaluate2()
        {
            Neurons.ForEach(n => n.EvaluateInput());
            Output = ActivationFunc2.Evaluate(Input);
        }

        public override string ToString() => "AL" + base.ToString();
    }
}
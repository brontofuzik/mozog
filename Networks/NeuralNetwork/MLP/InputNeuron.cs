using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MLP
{
    public class InputNeuron : NeuronBase, IInputNeuron
    {
        #region Construction

        internal InputNeuron()
        {
        }

        protected override ISynapse MakeSynapse()
        {
            throw new System.NotImplementedException();
        }

        #endregion // Construction

        public override string ToString() => $"IN({Output:F2})";
    }
}

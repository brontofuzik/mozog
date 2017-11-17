using System.Collections.Generic;

namespace NeuralNetwork.MLP
{
    public class InputLayer : LayerBase<InputNeuron>, IInputLayer
    {
        #region Construction

        internal InputLayer(NetworkArchitecture.Layer layer)
            : base(layer)
        {
        }

        protected override InputNeuron MakeNeuron() => new InputNeuron();

        #endregion // Construction

        public new IEnumerable<IInputNeuron> Neurons => base.Neurons;

        public override string ToString() => "IL" + base.ToString();
    }
}

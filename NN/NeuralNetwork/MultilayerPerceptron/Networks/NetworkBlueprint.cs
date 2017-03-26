using System;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Layers;

namespace NeuralNetwork.MultilayerPerceptron.Networks
{
    /* TODO Remove
    public class NetworkBlueprint
    {
        public NetworkBlueprint(LayerBlueprint inputLayerBlueprint, ActivationLayerBlueprint[] hiddenLayerBlueprints, ActivationLayerBlueprint outputLayerBlueprint)
        {
            Initialize(inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint);
        }

        public NetworkBlueprint(LayerBlueprint inputLayerBlueprint, ActivationLayerBlueprint hiddenLayerBlueprint, ActivationLayerBlueprint outputLayerBlueprint)
            : this(inputLayerBlueprint, new[] { hiddenLayerBlueprint }, outputLayerBlueprint)
        {
        }

        public NetworkBlueprint(params int[] layerNeuronCounts)
        {
            if (layerNeuronCounts.Length < 2)
            {
                throw new ArgumentException("layerNeuronCounts");
            }

            // Create the input layer blueprint.
            int inputLayerNeuronCount = layerNeuronCounts[0];
            LayerBlueprint inputLayerBlueprint = new LayerBlueprint(inputLayerNeuronCount);

            // Create the hidden layer blueprints.
            int hiddenLayerCount = layerNeuronCounts.Length - 2;
            ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[hiddenLayerCount];
            for (int i = 0; i < hiddenLayerCount; i++)
            {
                int hiddenLayerNeuronCount = layerNeuronCounts[1 + i];
                hiddenLayerBlueprints[i] = new ActivationLayerBlueprint(hiddenLayerCount);
            }

            // Create the output layer blueprint.
            int outputLayerNeuronCount = layerNeuronCounts[layerNeuronCounts.Length - 1];
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint(outputLayerNeuronCount);

            Initialize(inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint);
        }

        public void Initialize(LayerBlueprint inputLayerBlueprint, ActivationLayerBlueprint[] hiddenLayerBlueprints, ActivationLayerBlueprint outputLayerBlueprint)
        {
            // 1. Create the layer blueprints.

            BiasLayerBlueprint = new LayerBlueprint(1);

            Require.IsNotNull(inputLayerBlueprint, "inputLayerBlueprint");
            this.InputLayerBlueprint = inputLayerBlueprint;

            Require.IsNotNull(hiddenLayerBlueprints, "hiddenLayerBlueprints");
            foreach (ActivationLayerBlueprint hiddenLayerBlueprint in hiddenLayerBlueprints)
            {
                Require.IsNotNull(hiddenLayerBlueprint, "hiddenLayerBlueprint");
            }
            this.HiddenLayerBlueprints = hiddenLayerBlueprints;

            Require.IsNotNull(outputLayerBlueprint, "outputLayerBlueprint");
            this.OutputLayerBlueprint = outputLayerBlueprint;

            // 2. Create the connector blueprints.

            ConnectorBlueprints = new ConnectorBlueprint[(HiddenLayerCount + 1) * 2];

            int i = 0;
            for (int targetLayerIndex = 1; targetLayerIndex < LayerCount; targetLayerIndex++)
            {
                // 2.3. Create the connector between the source layer (bias) and the target layer.
                int sourceLayerIndex = -1;
                int sourceLayerNeuronCount = 1;
                int targetLayerNeuronCount = GetLayerNeuronCount(targetLayerIndex);
                ConnectorBlueprints[i++] = new ConnectorBlueprint(sourceLayerIndex, sourceLayerNeuronCount, targetLayerIndex, targetLayerNeuronCount);

                // 2.4. Create the connector between the source layer (previous) and the target layer.
                sourceLayerIndex = targetLayerIndex - 1;
                sourceLayerNeuronCount = GetLayerNeuronCount(sourceLayerIndex);
                ConnectorBlueprints[i++] = new ConnectorBlueprint(sourceLayerIndex, sourceLayerNeuronCount, targetLayerIndex, targetLayerNeuronCount);
            }
        }

        public int LayerCount => 1 + HiddenLayerCount + 1;

        public LayerBlueprint BiasLayerBlueprint { get; private set; }

        public int BiasLayerNeuronCount => BiasLayerBlueprint.NeuronCount;

        public LayerBlueprint InputLayerBlueprint { get; private set; }

        public int InputLayerNeuronCount => InputLayerBlueprint.NeuronCount;

        public ActivationLayerBlueprint[] HiddenLayerBlueprints { get; private set; }

        public int HiddenLayerCount => HiddenLayerBlueprints.Length;

        public ActivationLayerBlueprint OutputLayerBlueprint { get; private set; }

        public int OutputLayerNeuronCount => OutputLayerBlueprint.NeuronCount;

        public ConnectorBlueprint[] ConnectorBlueprints { get; private set; }

        public int ConnectorCount => ConnectorBlueprints.Length;

        public int GetLayerNeuronCount(int layerIndex)
        {
            switch (layerIndex)
            {
                case -1:
                    return BiasLayerNeuronCount;
                case 0:
                    return InputLayerNeuronCount;
                default:
                    if (0 < layerIndex && layerIndex < LayerCount - 1)
                    {
                        return HiddenLayerBlueprints[layerIndex - 1].NeuronCount;
                    }
                    else if (layerIndex == LayerCount - 1)
                    {
                        return OutputLayerNeuronCount;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
            }
        }

        public int GetHiddenLayerNeuronCount(int hiddenLayerIndex) => HiddenLayerBlueprints[hiddenLayerIndex].NeuronCount;

        public override string ToString()
        {
            var hiddenLayers = $"[{String.Join(", ", HiddenLayerBlueprints.AsEnumerable())}]";
            return $"MLP({InputLayerBlueprint}, {hiddenLayers}, {OutputLayerBlueprint}";
        }
    }
    */
}
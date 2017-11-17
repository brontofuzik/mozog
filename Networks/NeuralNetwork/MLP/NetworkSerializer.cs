namespace NeuralNetwork.MLP
{
    internal class NetworkSerializer : INetworkSerializer
    {
        public void Serialize(INetwork network, string fileName)
        {
            /* TODO Serialization
            // Open the file for writing.
            TextWriter fileWriter = new StreamWriter(fileName);

            // Write the line containing the numbers of neurons in the layers.
            StringBuilder lineSB = new StringBuilder();
            lineSB.Append(InputLayer.NeuronCount + " ");
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
            {
                lineSB.Append(hiddenLayer.NeuronCount + " ");
            }
            lineSB.Append(OutputLayer.NeuronCount);
            string line = lineSB.ToString();

            fileWriter.WriteLine(line);

            // Write the blank line.
            fileWriter.WriteLine();

            // 1. Save the weights of source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in ActivationLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons_Typed)
                {
                    lineSB = new StringBuilder();
                    for (int i = 0; i < hiddenNeuron.SourceSynapses.Count; i++)
                    {
                        lineSB.Append(hiddenNeuron.SourceSynapses[i].Weight + " ");
                    }
                    if (hiddenNeuron.SourceSynapses.Count != 0)
                    {
                        lineSB.Remove(lineSB.Length - 1, 1);
                    }
                    line = lineSB.ToString();

                    fileWriter.WriteLine(line);
                }

                // Write the blank line.
                fileWriter.WriteLine();
            }

            // 2. Save the weights of source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in OutputLayer.Neurons)
            {
                lineSB = new StringBuilder();
                for (int i = 0; i < outputNeuron.SourceSynapses.Count; i++)
                {
                    lineSB.Append(outputNeuron.SourceSynapses[i].Weight + " ");
                }
                if (outputNeuron.SourceSynapses.Count != 0)
                {
                    lineSB.Remove(lineSB.Length - 1, 1);
                }
                line = lineSB.ToString();

                fileWriter.WriteLine(line);
            }

            // Close the weights file.
            fileWriter.Close();
            */
        }

        public INetwork Deserialize(string fileName)
        {
            return null;

            /* TODO Serialization
            // Open the weights file for reading.
            TextReader fileReader = new StreamReader(fileName);
            const char separator = ' ';

            // Read the line containing the numbers of neurons in the layers.
            string line = fileReader.ReadLine();
            string[] words = line.Split(separator);

            // Read the blank line.
            fileReader.ReadLine();

            //
            // 1. Load the weights of the hidden neurons.
            //

            foreach (IActivationLayer hiddenLayer in ActivationLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons_Typed)
                {
                    line = fileReader.ReadLine();
                    words = line.Split(separator);

                    for (int i = 0; i < hiddenNeuron.SourceSynapses.Count; i++)
                    {
                        hiddenNeuron.SourceSynapses[i].Weight = Double.Parse(words[i]);
                    }
                }

                // Read the blank line.
                fileReader.ReadLine();
            }

            //
            // 2. Load the weights of the output neurons.
            //

            foreach (IActivationNeuron outputNeuron in OutputLayer.Neurons)
            {
                line = fileReader.ReadLine();
                words = line.Split(separator);

                for (int i = 0; i < outputNeuron.SourceSynapses.Count; i++)
                {
                    outputNeuron.SourceSynapses[i].Weight = Double.Parse(words[i]);
                }
            }

            // Close the weights file.
            fileReader.Close();
            */
        }
    }
}

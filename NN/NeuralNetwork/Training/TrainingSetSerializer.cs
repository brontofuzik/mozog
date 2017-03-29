using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    class TrainingSetSerializer : ITrainingSetSerializer
    {
        // TODO Serialization
        public void Serialize(TrainingSet trainingSet, string fileName)
        {
            /*
            TextWriter textWriter = new StreamWriter(fileName);
            const char separator = ' ';

            //
            // 1. Write the input vector length and the input vector length.
            //

            string line = InputVectorLength.ToString() + separator + OutputVectorLength;
            textWriter.WriteLine(line);

            // Write the blank line.
            textWriter.WriteLine();

            //
            // 2. Write the training patterns.
            //

            foreach (SupervisedTrainingPattern trainingPattern in pattern)
            {
                // 2.1. Write the input vector.
                foreach (double d in trainingPattern.InputVector)
                {
                    textWriter.Write(d.ToString() + separator);
                }

                // 2.2. Write the output vector.
                foreach (double d in trainingPattern.OutputVector)
                {
                    textWriter.Write(d.ToString() + separator);
                }

                textWriter.WriteLine();
            }

            textWriter.Close();
            */
        }

        // TODO Serialization
        public TrainingSet Deserialize(string fileName)
        {
            return null;

            /*
            TextReader textReader = new StreamReader(fileName);
            const char separator = ' ';

            //
            // 1. Read the input vector length and the output vector length.
            //

            string line = textReader.ReadLine();
            string[] words = line.Trim().Split(separator);

            // Validate the input vector length.
            int inputVectorLength = Int32.Parse(words[0]);
            Require.IsPositive(inputVectorLength, "inputVectorLength");

            // Validate the output vector length.
            int outputVectorLength = Int32.Parse(words[1]);
            Require.IsPositive(outputVectorLength, "outputVectorLength");

            TrainingSet trainingSet = new TrainingSet(inputVectorLength, outputVectorLength);

            // Read the blank line.
            textReader.ReadLine();

            //
            // 2. Read and create the training patterns.
            //

            while ((line = textReader.ReadLine()) != null)
            {
                if (line.Length == 0)
                {
                    continue;
                }

                words = line.Trim().Split(separator);

                // 2.1. Read and create the input vector.
                double[] inputVector = new double[inputVectorLength];
                for (int i = 0; i < inputVectorLength; i++)
                {
                    inputVector[i] = Double.Parse(words[i]);
                }

                // 2.2. Read and create the output vector.
                double[] outputVector = new double[outputVectorLength];
                for (int i = 0; i < outputVectorLength; i++)
                {
                    outputVector[i] = Double.Parse(words[inputVectorLength + i]);
                }

                // 2.3. Add the training pattern into the training set.
                SupervisedTrainingPattern trainingPattern = new SupervisedTrainingPattern(inputVector, outputVector);
                trainingSet.Add(trainingPattern);
            }

            textReader.Close();
            return trainingSet;
            */
        }
    }

    interface ITrainingSetSerializer
    {
        void Serialize(TrainingSet trainingSet, string fileName);

        TrainingSet Deserialize(string fileName);
    }
}

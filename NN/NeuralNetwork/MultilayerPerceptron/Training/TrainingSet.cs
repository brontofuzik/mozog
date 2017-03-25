using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mozog.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Training
{
    /// <remarks>
    /// A (labeled) training set is a set of (labeled) training patterns.
    /// </remarks>
    public class TrainingSet
    {
        private readonly List<SupervisedTrainingPattern> trainingPatterns = new List<SupervisedTrainingPattern>();

        public TrainingSet(int inputVectorLength, int outputVectorLength, object tag)
        {
            Require.IsPositive(inputVectorLength, nameof(inputVectorLength));
            InputVectorLength = inputVectorLength;

            Require.IsNonNegative(outputVectorLength, nameof(outputVectorLength));
            OutputVectorLength = outputVectorLength;

            Tag = tag;
        }

        public TrainingSet(int inputVectorLength, int outputVectorLength)
            : this(inputVectorLength, outputVectorLength, null)
        {    
        }

        public TrainingSet(int vectorLength)
            : this(vectorLength, 0, null)
        {
        }

        public int InputVectorLength { get; }

        public int OutputVectorLength { get; }

        public object Tag { get; }

        public IEnumerable<SupervisedTrainingPattern> TrainingPatternsRandomOrder
        {
            get
            {
                SupervisedTrainingPattern[] shuffledTrainingPatterns = trainingPatterns.ToArray();
                StaticRandom.Shuffle(shuffledTrainingPatterns);
                foreach (var trainingPattern in shuffledTrainingPatterns)
                {
                    yield return trainingPattern;
                }
            }
        }

        public int Size => trainingPatterns.Count;

        public SupervisedTrainingPattern this[int trainingPatternIndex] => trainingPatterns[trainingPatternIndex];

        public IEnumerator<SupervisedTrainingPattern> GetEnumerator() => trainingPatterns.GetEnumerator();

        public bool Contains(SupervisedTrainingPattern trainingPattern) => trainingPatterns.Contains(trainingPattern);

        public void Add(SupervisedTrainingPattern trainingPattern)
        {
            Require.IsNotNull(trainingPattern, nameof(trainingPattern));
            if (InputVectorLength != trainingPattern.InputVector.Length)
            {
                throw new ArgumentException("The input vector must be of size " + InputVectorLength, nameof(trainingPattern));
            }
              
            if (OutputVectorLength != trainingPattern.OutputVector.Length)
            {
                throw new ArgumentException("The output vector must be of size " + OutputVectorLength, nameof(trainingPattern));
            }

            trainingPatterns.Add(trainingPattern);
        }

        public void Add(TrainingSet trainingSet)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));
            if (InputVectorLength != trainingSet.InputVectorLength || OutputVectorLength != trainingSet.OutputVectorLength)
            {
                // TODO: Incompatible sets.
                throw new ArgumentException();
            }

            trainingPatterns.AddRange(trainingSet.trainingPatterns);
        }

        public bool Remove(SupervisedTrainingPattern trainingPattern) => trainingPatterns.Remove(trainingPattern);

        public void Clear()
        {
            trainingPatterns.Clear();
        }

        public TrainingSet SeparateTestSet(int index, int size)
        {
            TrainingSet testSet = new TrainingSet(InputVectorLength, OutputVectorLength);
            testSet.trainingPatterns.AddRange(trainingPatterns.GetRange(index, size));
            trainingPatterns.RemoveRange(index, size);
            return testSet;
        }

        public void Save(string fileName)
        {
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

            foreach (SupervisedTrainingPattern trainingPattern in trainingPatterns)
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
        }

        public static TrainingSet Load(string fileName)
        {
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
        }

        public override string ToString()
        {
            StringBuilder trainingSetStringBuilder = new StringBuilder();

            //trainingSetStringBuilder.Append(base.ToString());

            trainingSetStringBuilder.Append("{\n");

            int trainingPatternIndex = 0;
            foreach (SupervisedTrainingPattern trainingPattern in trainingPatterns)
            {
                trainingSetStringBuilder.Append("\t" + trainingPatternIndex++ + " : " + trainingPattern + "\n");
            }

            // Remove the trailing "\n" if the training set contained no training patterns.
            if (Size == 0)
            {
                trainingSetStringBuilder.Remove(trainingSetStringBuilder.Length - 1, 1);
            }

            trainingSetStringBuilder.Append("}");

            return trainingSetStringBuilder.ToString();
        }
    }
}

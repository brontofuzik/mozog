using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    /// <remarks>
    /// A (labeled) training set is a set of (labeled) training patterns.
    /// </remarks>
    public class TrainingSet : IEnumerable<SupervisedTrainingPattern>
    {
        private readonly List<SupervisedTrainingPattern> patterns = new List<SupervisedTrainingPattern>();

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

        public IEnumerable<SupervisedTrainingPattern> RandomPatterns
        {
            get
            {
                SupervisedTrainingPattern[] shuffledTrainingPatterns = patterns.ToArray();
                StaticRandom.Shuffle(shuffledTrainingPatterns);
                return shuffledTrainingPatterns.AsEnumerable();
            }
        }

        public int Size => patterns.Count;

        public SupervisedTrainingPattern this[int patternIndex] => patterns[patternIndex];

        public IEnumerator<SupervisedTrainingPattern> GetEnumerator() => patterns.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(SupervisedTrainingPattern pattern)
        {
            Require.IsNotNull(pattern, nameof(pattern));
            if (InputVectorLength != pattern.InputVector.Length)
            {
                throw new ArgumentException("The input vector must be of size " + InputVectorLength, nameof(pattern));
            }
              
            if (OutputVectorLength != pattern.OutputVector.Length)
            {
                throw new ArgumentException("The output vector must be of size " + OutputVectorLength, nameof(pattern));
            }

            this.patterns.Add(pattern);
        }

        public void Add((double[] input, double[] output) pattern)
        {
            Add(new SupervisedTrainingPattern(pattern));
        }

        public void Add(TrainingSet trainingSet)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));
            if (InputVectorLength != trainingSet.InputVectorLength || OutputVectorLength != trainingSet.OutputVectorLength)
            {
                // TODO Incompatible sets.
                throw new ArgumentException();
            }

            patterns.AddRange(trainingSet.patterns);
        }

        public bool Contains(SupervisedTrainingPattern pattern) => patterns.Contains(pattern);

        public bool Remove(SupervisedTrainingPattern pattern)
            => patterns.Remove(pattern);

        public void Clear()
        {
            patterns.Clear();
        }

        // TODO ???
        //public TrainingSet SeparateTestSet(int index, int size)
        //{
        //    TrainingSet testSet = new TrainingSet(InputVectorLength, OutputVectorLength);
        //    testSet.pattern.AddRange(pattern.GetRange(index, size));
        //    pattern.RemoveRange(index, size);
        //    return testSet;
        //}

        public override string ToString()
        {
            StringBuilder trainingSetStringBuilder = new StringBuilder();

            //trainingSetStringBuilder.Append(base.ToString());

            trainingSetStringBuilder.Append("{\n");

            int trainingPatternIndex = 0;
            foreach (SupervisedTrainingPattern trainingPattern in patterns)
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

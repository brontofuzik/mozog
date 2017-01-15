using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeuralNetwork.Utils;
using Random = NeuralNetwork.Utils.Random;

namespace NeuralNetwork.MultilayerPerceptron.Training
{
    /// <remarks>
    /// <para>
    /// A (labeled) training set.
    /// </para>
    /// <para>
    /// Definition: A (labeled) training set is a set of (labeled) training patterns.
    /// </para>
    /// </remarks>
    public class TrainingSet
    {
        /// <summary>
        /// Creates a new (labeled) training set.
        /// </summary>
        /// <param name="inputVectorLength">The length of the input vector.</param>
        /// <param name="outputVectorLength">The length of the output vector.</param>
        /// <param name="tag">The tag.</param>
        /// <exception cref="ArgumentException">
        /// Condition 1: <c>inputVectorLength</c> is less than or equal to zero.
        /// Condition 2: <c>outputVectorLength</c> is less than or equal to zero.
        /// </exception>
        public TrainingSet(int inputVectorLength, int outputVectorLength, object tag)
        {
            Require.IsPositive(inputVectorLength, nameof(inputVectorLength));
            Require.IsNonNegative(outputVectorLength, nameof(outputVectorLength));

            _inputVectorLength = inputVectorLength;
            _outputVectorLength = outputVectorLength;

            _trainingPatterns = new List<SupervisedTrainingPattern>();

            _tag = tag;
        }

        /// <summary>
        /// Creates a new (labeled) training set.
        /// </summary>
        /// <param name="inputVectorLength">The length of the input vector.</param>
        /// <param name="outputVectorLength">The length of the output vector.</param>
        /// <exception cref="ArgumentException">
        /// Condition 1: <c>inputVectorLength</c> is less than or equal to zero.
        /// Condition 2: <c>outputVectorLength</c> is less than zero.
        /// </exception>
        public TrainingSet(int inputVectorLength, int outputVectorLength)
            : this(inputVectorLength, outputVectorLength, null)
        {    
        }

        /// <summary>
        /// Creates a new unlabeled training set.
        /// </summary>
        /// <param name="vectorLength">The length of the vector.</param>
        public TrainingSet(int vectorLength)
            : this(vectorLength, 0, null)
        {
        }

        /// <summary>
        /// Loads a training set from a file.
        /// </summary>
        /// <param name="fileName">The name of the file from which the training set is to be loaded.</param>
        /// <returns>
        /// The training set.
        /// </returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<SupervisedTrainingPattern> GetEnumerator()
        {
            return _trainingPatterns.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the training set contains a training pattern.
        /// </summary>
        /// <param name="trainingPattern">The training sample to check for containment.</param>
        /// <returns>
        /// <c>True</c> if contains, <c>false</c> otherwise.
        /// </returns>
        public bool Contains(SupervisedTrainingPattern trainingPattern)
        {
            return _trainingPatterns.Contains(trainingPattern);
        }

        /// <summary>
        /// <para>
        /// Adds a (supervised) training pattern to the training set.
        /// </para>
        /// <para>
        /// Note that if the training pattern already exists in the set, it will be replaced.
        /// </para>
        /// </summary>
        /// <param name="trainingPattern">The training pattern to add.</param>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>trainingPattern</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Condition: the lengths of input vector or output vector (from the training pattern) differ from their expected lengths (from the training set).
        /// </exception>
        public void Add(SupervisedTrainingPattern trainingPattern)
        {
            // Validatate the arguments.
            Require.IsNotNull(trainingPattern, "trainingPattern");

            // Validate the input vector length.
            if (_inputVectorLength != trainingPattern.InputVector.Length)
            {
                throw new ArgumentException("The input vector must be of size " + _inputVectorLength, nameof(trainingPattern));
            }
            
            // Validate the output vector length.
            if (_outputVectorLength != trainingPattern.OutputVector.Length)
            {
                throw new ArgumentException("The output vector must be of size " + _outputVectorLength, nameof(trainingPattern));
            }

            // Add the training pattern to the training set.
            _trainingPatterns.Add(trainingPattern);
        }

        /// <summary>
        /// Adds a given training set to the training set.
        /// </summary>
        /// <param name="trainingSet">The training set to be added.</param>
        /// <exception cref="ArgumentException">
        /// Condition: <c>trainingSet</c> is not compatible with this training set.
        /// This happens when either their input vector lengths or their output vector lengths differ.
        /// </exception>
        public void Add(TrainingSet trainingSet)
        {
            // Validate the arguments.
            Require.IsNotNull(trainingSet, "trainingSet");

            // Validate the input vector length and the output vector length.
            if (_inputVectorLength != trainingSet.InputVectorLength || _outputVectorLength != trainingSet.OutputVectorLength)
            {
                // TODO: Incompatible sets.
                throw new ArgumentException();
            }

            // Add all the training patterns contained within the given training set to the training set.
            foreach (SupervisedTrainingPattern trainingPattern in trainingSet)
            {
                _trainingPatterns.Add(trainingPattern);
            }
        }

        /// <summary>
        /// Removes a training pattern from the training set.
        /// </summary>
        /// <param name="trainingPattern">The training sample to remove.</param>
        /// <returns>
        /// <c>True</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public bool Remove(SupervisedTrainingPattern trainingPattern)
        {
            return _trainingPatterns.Remove(trainingPattern);
        }

        /// <summary>
        /// Removes all training patterns from the training set.
        /// </summary>
        public void Clear()
        {
            _trainingPatterns.Clear();
        }

        /// <summary>
        /// Separates a test set from the training set.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public TrainingSet SeparateTestSet(int index, int size)
        {
            TrainingSet testSet = new TrainingSet(_inputVectorLength, _outputVectorLength);

            testSet._trainingPatterns.AddRange(_trainingPatterns.GetRange(index, size));
            _trainingPatterns.RemoveRange(index, size);

            return testSet;
        }

        /// <summary>
        /// Save the training set to a file.
        /// </summary>
        /// <param name="fileName">The name of the file to which the training set is to be saved.</param>
        public void Save(string fileName)
        {
            TextWriter textWriter = new StreamWriter(fileName);
            const char separator = ' ';

            //
            // 1. Write the input vector length and the input vector length.
            //

            string line = _inputVectorLength.ToString() + separator + _outputVectorLength;
            textWriter.WriteLine(line);

            // Write the blank line.
            textWriter.WriteLine();

            //
            // 2. Write the training patterns.
            //

            foreach (SupervisedTrainingPattern trainingPattern in _trainingPatterns)
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

        /// <summary>
        /// Returns a string that represents the training set.
        /// </summary>
        /// <returns>
        /// A string that represents the training set.
        /// </returns>
        public override string ToString()
        {
            StringBuilder trainingSetStringBuilder = new StringBuilder();

            //trainingSetStringBuilder.Append(base.ToString());

            trainingSetStringBuilder.Append("{\n");

            int trainingPatternIndex = 0;
            foreach (SupervisedTrainingPattern trainingPattern in _trainingPatterns)
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

        /// <summary>
        /// Gets the length of the input vector.
        /// </summary>
        /// <value>
        /// The length of the input vector.
        /// Note that this is always greater than zero.
        /// </value>
        public int InputVectorLength
        {
            get
            {
                return _inputVectorLength;
            }
        }

        /// <summary>
        /// Gets the length of the output vector.
        /// </summary>
        /// <value>
        /// The length of the output vector.
        /// Note that this is always greater than zero.
        /// </value>
        public int OutputVectorLength
        {
            get
            {
                return _outputVectorLength;
            }
        }

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag
        {
            get
            {
                return _tag;
            }
        }

        /// <summary>
        /// The training set indexer.
        /// </summary>
        /// <param name="trainingPatternIndex">The training pattern index.</param>
        /// <returns>
        /// The training pattern at the given index.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Condition: <c>trainingPatternIndex</c> is out of range.
        /// </exception>
        public SupervisedTrainingPattern this[int trainingPatternIndex]
        {
            get
            {
                return _trainingPatterns[trainingPatternIndex];
            }
        }

        /// <summary>
        /// Gets the size of the training set (i.e. the number of training patterns in the training set).
        /// </summary>
        /// <value>
        /// The size of the training set.
        /// Note that this is always always greater then or equal to zero.
        /// </value>
        public int Size
        {
            get
            {
                return _trainingPatterns.Count;
            }
        }

        public IEnumerable<SupervisedTrainingPattern> TrainingPatternsRandomOrder
        {
            get
            {
                SupervisedTrainingPattern[] _trainingPatternsRandomOrder = _trainingPatterns.ToArray();
                Random.Shuffle<SupervisedTrainingPattern>(_trainingPatternsRandomOrder);
                foreach (SupervisedTrainingPattern trainingPattern in _trainingPatternsRandomOrder)
                {
                    yield return trainingPattern;
                }
            }
        }

        /// <summary>
        /// The input vector length.
        /// </summary>
        private int _inputVectorLength;

        /// <summary>
        /// The output vector length.
        /// </summary>
        private int _outputVectorLength;

        /// <summary>
        /// The training patterns.
        /// </summary>
        private List<SupervisedTrainingPattern> _trainingPatterns;

        /// <summary>
        /// The tag.
        /// </summary>
        private object _tag;
    }
}

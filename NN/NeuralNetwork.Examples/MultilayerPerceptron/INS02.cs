using System;
using System.Collections.Specialized;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron
{
    class INS02
    {
        private static readonly StringCollection keywords = new StringCollection
        {
            "abstract", "as",
            "base", "bool", "break", "byte",
            "case", "catch", "char", "checked", "class", "const", "continue",
            "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach",
            "goto",
            "if", "implicit", "in", "int", "interface", "internal", "is",
            "lock", "long",
            "namespace", "new", "null",
            "object", "operator", "out", "override",
            "params", "private", "protected", "public",
            "readonly", "ref", "return",
            "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
            "this", "throw", "true", "try", "typeof",
            "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
            "virtual", "volatile", "void",
            "while"
        };

        public const int maxKeywordLength = 10;
        public const int keywordCount = 10;

        private static readonly IEncoder<string, int> encoder = new INS02Encoder();
        private static DataSet data;
        private static Network network;

        public static void Run()
        {
            // Step 1: Create the training set.

            data = CreateDataSet();

            // Step 2: Create the network.

            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, 20, data.OutputSize },
                new LogisticActivationFunction());
            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer(data, null, null);
            
            var args = new BackpropagationArgs(
                maxIterations: Int32.MaxValue,
                maxError: 0.01,
                learningRate: 0.05,
                momentum: 0.9,
                batchLearning: false);
            var log = trainer.Train(network, args);

            Console.WriteLine($"Iterations: {log.IterationCount}, Error:{log.NetworkError}");

            // Step 4: Test the network.

            foreach (string keyword in keywords)
            {
                // Original keyword
                var index = network.EvaluateEncoded(keyword, encoder);

                // Mutated keywords
                4.Times(() =>
                {
                    string mutatedKeyword = MutateKeyword(keyword);
                    index = network.EvaluateEncoded(mutatedKeyword, encoder);
                });
            }
        }

        private static DataSet CreateDataSet()
        {
            var dataSet = new DataSet(maxKeywordLength * 5, keywordCount, keywords);
            for (int i = 0; i < keywordCount; i++)
            {
                // Original keyword
                string originalKeyword = keywords[i];
                var input = encoder.EncodeInput(originalKeyword);
                var output = encoder.EncodeOutput(i);
                dataSet.Add((input, output, originalKeyword));

                // Mutated keywords
                4.Times(() =>
                {
                    string mutatedKeyword = MutateKeyword(originalKeyword);
                    input = encoder.EncodeInput(mutatedKeyword);
                    output = encoder.EncodeOutput(-1);
                    dataSet.Add((input, output, mutatedKeyword));
                });
            }
            return dataSet;
        }

        #region Keyword mutation

        private static string MutateKeyword(string keyword)
        {
            string mutatedKeyword;
            do
            {
                mutatedKeyword = NewKeyword(keyword);
            }
            while (keyword.Contains(mutatedKeyword));
            return mutatedKeyword;
        }

        private static string NewKeyword(string keyword)
        {
            int index = StaticRandom.Int(0, keyword.Length);
            return new StringBuilder(keyword) { [index] = MutateCharacter(keyword[index]) }.ToString();
        }

        private static char MutateCharacter(char character)
        {
            char mutatedCharacter;
            do
            {
                mutatedCharacter = RandomLetter();
            }
            while (mutatedCharacter == character);
            return mutatedCharacter;
        }

        private static char RandomLetter() => (char)('a' + StaticRandom.Int(0, 26));

        #endregion // Keyword mutation
    }

    internal class INS02Encoder : IEncoder<string, int>
    {
        public double[] EncodeInput(string keyword)
        {
            keyword = keyword.PadRight(INS02.maxKeywordLength);

            StringBuilder sb = new StringBuilder();
            foreach (char letter in keyword)
            {
                string binary;
                if (letter != ' ')
                {
                    binary = Convert.ToString(letter - 'a', 2).PadLeft(5, '0');
                }
                else
                {
                    binary = new String('1', 5);
                }
                sb.Append(binary);
            }
            string inputVectorString = sb.ToString();

            double[] vector = new double[inputVectorString.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = inputVectorString[i] == '1' ? 1.0 : 0.0;
            }
            return vector;
        }

        public double[] EncodeOutput(int index)
            => Vector.IndexToVector(INS02.keywordCount, index);

        public int DecodeOutput(double[] output)
            => Vector.VectorToIndex(output, 0.5);
    }
}
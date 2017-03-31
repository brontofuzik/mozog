using System;
using System.Collections.Specialized;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.Keywords
{
    static class Data
    {
        public static readonly StringCollection Keywords = new StringCollection
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

        public const int MaxKeywordLength = 10;
        public const int KeywordCount = 10;

        public static readonly IEncoder<string, int> Encoder = new _Encoder();

        public static DataSet Create()
        {
            var dataSet = new EncodedDataSet<string, int>(MaxKeywordLength * 5, KeywordCount, Encoder);
            for (int i = 0; i < KeywordCount; i++)
            {
                // Original keyword
                string originalKeyword = Keywords[i];
                dataSet.Add(originalKeyword, i, originalKeyword);

                // Mutated keywords
                4.Times(() =>
                {
                    string mutatedKeyword = MutateKeyword(originalKeyword);
                    dataSet.Add(mutatedKeyword, -1, mutatedKeyword);
                });
            }
            return dataSet;
        }

        public static string MutateKeyword(string keyword)
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

        private class _Encoder : IEncoder<string, int>
        {
            public double[] EncodeInput(string keyword)
            {
                keyword = keyword.PadRight(MaxKeywordLength);

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
                => Vector.IndexToVector(KeywordCount, index);

            // Index
            public int DecodeOutput(double[] output)
                => Vector.VectorToIndex(output, 0.5);
        }
    }
}

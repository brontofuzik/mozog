using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
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
        private const int Width = 5;

        public static readonly IEncoder<string, int> Encoder = new KeywordsEncoder();

        public static IEncodedData<string, int> Create()
        {
            var data = ClassificationData.New(Encoder, MaxKeywordLength * Width, KeywordCount);
            for (int i = 0; i < KeywordCount; i++)
            {
                // Original keyword
                string originalKeyword = Keywords[i];
                data.Add(originalKeyword, i, tag: originalKeyword);

                // Mutated keywords
                4.Times(() =>
                {
                    string mutatedKeyword = MutateKeyword(originalKeyword);
                    data.Add(mutatedKeyword, i, tag: mutatedKeyword);
                });
            }
            return data;
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

        private class KeywordsEncoder : IEncoder<string, int>
        {
            public double[] EncodeInput(string keyword)
            {
                var sb = new StringBuilder();
                keyword.ForEach(c => sb.Append(EncodeChar(c)));
                string encoding = sb.ToString().PadRight(MaxKeywordLength * Width, '0');
                return encoding.Select(c => c == '1' ? 1.0 : 0.0).ToArray();
            }
   
            public string DecodeInput(double[] input)
            {
                // Not needed
                throw new NotImplementedException();
            }

            public double[] EncodeOutput(int index)
                => Vector.IndexToVector(index, KeywordCount);

            // Index
            public int DecodeOutput(double[] output)
                => Vector.VectorToIndex(output);

            private static string EncodeChar(char c)
                => Convert.ToString(c - 'a', 2).PadLeft(Width, '0');
        }
    }
}

﻿using System;
using System.Collections.Specialized;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.INS02
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

        private const int maxKeywordLength = 10;
        private const int keywordCount = 10;

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

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            foreach (string keyword in keywords)
            {
                Console.WriteLine(keyword + " {");

                TestNetwork(keyword);
                5.Times(() =>
                {
                    string mutatedKeyword = MutateKeyword(keyword);
                    TestNetwork(mutatedKeyword);
                });

                Console.WriteLine("}");
                Console.WriteLine();
            }
        }

        private static DataSet CreateDataSet()
        {
            var dataSet = new DataSet(maxKeywordLength * 5, keywordCount, keywords);
            for (int i = 0; i < keywordCount; i++)
            {
                var inputVector = KeywordToVector(keywords[i]);
                var outputVector = Vector.IndexToVector(keywordCount, i);
                dataSet.Add((inputVector, outputVector, keywords[i]));
            }
            return dataSet;
        }

        // TODO
        public static double[] KeywordToVector(string keyword)
        {
            keyword = keyword.PadRight(maxKeywordLength);

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

        private static string MutateKeyword(string keyword)
        {
            int index = StaticRandom.Int(0, keyword.Length);
            return new StringBuilder(keyword) {[index] = MutateCharacter(keyword[index])}.ToString();
        }

        private static char MutateCharacter(char character)
        {
            int i = character - 'a';
            int mutatedI;
            do
            {
                mutatedI = StaticRandom.Int(0, 26);
            }
            while (mutatedI == i);
            return (char)('a' + mutatedI);
        }
        
        private static void TestNetwork(string keyword)
        {
            var inputVector = KeywordToVector(keyword);
            var outputVector = network.Evaluate(inputVector);
            int keywordIndex = Vector.VectorToIndex(outputVector, 0.5);

            if (keywordIndex != -1)
            {
                Console.WriteLine("\t{0} : {1}", keyword, keywordIndex);
            }
        }
    }
}
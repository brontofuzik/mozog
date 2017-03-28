using System;
using System.Collections.Specialized;
using System.Text;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.INS02
{
    class Program
    {
        static readonly StringCollection keywords = new StringCollection() {
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

        static int maxKeywordLength = 10;

        static int keywordCount = 10;

        static readonly int[] networkTopology = { maxKeywordLength * 5, 20, keywordCount };
        
        static Network network;

        static readonly Random random = new Random();

        /// <summary>
        /// The application's entry point.
        /// </summary>
        /// <param name="args">The comamnd line arguments.</param>
        static void _Main(string[] args)
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            TrainingSet trainingSet = new TrainingSet(networkTopology[0], networkTopology[networkTopology.Length - 1], keywords);
            for (int i = 0; i < keywordCount; i++)
            {
                double[] inputVector = KeywordToVector(keywords[i]);
                double[] outputVector = KeywordIndexToVector(i);
                trainingSet.Add(new SupervisedTrainingPattern(inputVector, outputVector, keywords[i]));
            }

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            // 2.2. Create the network.
            network = new Network(networkTopology, new LogisticActivationFunction());

            /* TODO Backprop
            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            // 3.1. Create the (backpropagation) trainer.
            BackpropagationTrainer trainer = new BackpropagationTrainer(trainingSet, null, null);

            // 3.2. Create the (backpropagation) training strategy.
            int maxIterationCount = Int32.MaxValue;
            double maxNetworkError = 0.01;
            bool batchLearning = false;

            double synapseLearningRate = 0.05;
            double connectorMomentum = 0.9;
            
            INS02BackpropagationTrainingStrategy backpropagationTrainingStrategy = new INS02BackpropagationTrainingStrategy(maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum);

            // 3.3. Train the network.
            TrainingLog trainingLog = trainer.Train(network, backpropagationTrainingStrategy);

            // 3.4. Inspect the training log.
            Console.WriteLine("Number of iterations used : " + trainingLog.IterationCount);
            Console.WriteLine("Minimum network error achieved : " + trainingLog.NetworkError);
            */

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            foreach (string keyword in keywords)
            {
                Console.WriteLine(keyword + " {");

                // 2.1. Test the network on the keyword.
                TestNetwork(keyword);

                // 2.2. Test the netowork on the keyword mutations.
                for (int i = 0; i < 5; ++i)
                {
                    string mutatedKeyword = MutateKeyword(keyword);
                    TestNetwork(mutatedKeyword);
                }

                Console.WriteLine("}");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywordIndex"></param>
        /// <returns></returns>
        public static double[] KeywordIndexToVector(int keywordIndex)
        {
            double[] vector = new double[keywordCount];

            for (int i = 0; i < keywordCount; i++)
            {
                vector[i] = i == keywordIndex ? 1.0 : 0.0;
            }

            return vector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputVector"></param>
        /// <returns></returns>
        static int VectorToKeywordIndex(double[] outputVector)
        {
            int keywordIndex = -1;
            int activeNeuronCount = 0;
            
            for (int i = 0; i < outputVector.Length; i++)
            {
                if (outputVector[i] >= 0.5)
                {
                    keywordIndex = i;
                    activeNeuronCount++;
                }
            }

            return activeNeuronCount == 1 ? keywordIndex : -1; 
        }

        /// <summary>
        /// Mutates a keyword.
        /// </summary>
        /// <param name="keyword">The keyword to mutate.</param>
        /// <returns>
        /// The mutated keyword.
        /// </returns>
        public static string MutateKeyword(string keyword)
        {
            int mutationIndex = random.Next(0, keyword.Length);
            char mutatedCharacter = MutateCharacter(keyword[mutationIndex]);

            StringBuilder sb = new StringBuilder(keyword);
            sb[mutationIndex] = mutatedCharacter;
            return sb.ToString();
        }

        /// <summary>
        /// Mutates a character.
        /// </summary>
        /// <param name="character">The character to mutate.</param>
        /// <returns>
        /// The mutated character.
        /// </returns>
        static char MutateCharacter(char character)
        {
            int i = character - 'a';
            int mutatedI;
            do
            {
                mutatedI = random.Next(0, 26);
            }
            while (mutatedI == i);
            char mutatedCharacter = (char)('a' + mutatedI);

            return mutatedCharacter;
        }
        
        /// <summary>
        /// Tests the network.
        /// </summary>
        /// <param name="keyword"></param>
        static void TestNetwork(string keyword)
        {
            double[] inputVector = KeywordToVector(keyword);
            double[] outputVector = network.Evaluate(inputVector);
            int keywordIndex = VectorToKeywordIndex(outputVector);

            if (keywordIndex != -1)
            {
                Console.WriteLine("\t{0} : {1}", keyword, keywordIndex);
            }
        }

    }
}
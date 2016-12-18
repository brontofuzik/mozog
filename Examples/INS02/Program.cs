using System;
using System.Collections.Specialized;
using System.Text;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training;
using NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher;

namespace INS02
{
    class Program
    {
        /// <summary>
        /// The keywords.
        /// </summary>
        static StringCollection keywords = new StringCollection() {
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

        /// <summary>
        /// The maximum length of a keyword.
        /// </summary>
        static int maxKeywordLength = 10;

        /// <summary>
        /// The number of keywords to recognize.
        /// </summary>
        static int keywordCount = 10;

        /// <summary>
        /// The topology of the network.
        /// </summary>
        static int[] networkTopology = new int[] { maxKeywordLength * 5, 20, keywordCount };
        
        /// <summary>
        /// The network.
        /// </summary>
        static Network network;

        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        static Random random = new Random();

        /// <summary>
        /// The application's entry point.
        /// </summary>
        /// <param name="args">The comamnd line arguments.</param>
        static void Main(string[] args)
        {
            #region Step 1 : Create the training set.

            // Step 1 : Create the training set.
            // ---------------------------------

            // 1.1. Create the trainig set.
            int inputVectorLength = networkTopology[0];
            int outputVectorLength = networkTopology[networkTopology.Length - 1];
            TrainingSet trainingSet = new TrainingSet(inputVectorLength, outputVectorLength, keywords);

            // 1.2. Create the training patterns.
            for (int i = 0; i < keywordCount; i++)
            {
                // Create the input vector.
                double[] inputVector = KeywordToVector(keywords[i]);

                // Create the output vector.
                double[] outputVector = KeywordIndexToVector(i);

                // Create the training pattern, ...
                SupervisedTrainingPattern trainingPattern = new SupervisedTrainingPattern(inputVector, outputVector, keywords[i]);
                
                // ... and add it to the training set.
                trainingSet.Add(trainingPattern);
            }

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            // Step 2 : Create the network.
            // ----------------------------

            // 2.1. Create the blueprint of the network.

            // 2.1.1. Create the blueprint of the input layer.
            LayerBlueprint inputLayerBlueprint = new LayerBlueprint(networkTopology[0]);

            // 2.1.2. Create the blueprints of the hidden layers.
            int hiddenLayerCount = networkTopology.Length - 2;
            ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[hiddenLayerCount];
            for (int i = 0; i < hiddenLayerCount; i++)
            {
                hiddenLayerBlueprints[i] = new ActivationLayerBlueprint(networkTopology[1 + i], new LogisticActivationFunction());
            }

            // 2.1.3. Create the blueprints of the output layer.
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint(networkTopology[networkTopology.Length - 1], new LogisticActivationFunction());

            // 2.1.4. Create the blueprint of the network.
            NetworkBlueprint networkBlueprint = new NetworkBlueprint(inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint);

            // 2.2. Create the network.
            network = new Network(networkBlueprint);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            // Step 3 : Train the network.
            // ---------------------------

            // 3.1. Create the (backpropagation) teacher.
            BackpropagationTeacher teacher = new BackpropagationTeacher(trainingSet, null, null);

            // 3.2. Create the (backpropagation) training strategy.
            int maxIterationCount = Int32.MaxValue;
            double maxNetworkError = 0.01;
            bool batchLearning = false;

            double synapseLearningRate = 0.05;
            double connectorMomentum = 0.9;
            
            INS02BackpropagationTrainingStrategy backpropagationTrainingStrategy = new INS02BackpropagationTrainingStrategy(maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum);

            // 3.3. Train the network.
            TrainingLog trainingLog = teacher.Train(network, backpropagationTrainingStrategy);

            // 3.4. Inspect the training log.
            Console.WriteLine("Number of iterations used : " + trainingLog.IterationCount);
            Console.WriteLine("Minimum network error achieved : " + trainingLog.NetworkError);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            // Step 4 : Test the network.
            // --------------------------

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

            #endregion // Step 4 : Test the network.
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
                vector[i] = (inputVectorString[i] == '1') ? 1.0 : 0.0; 
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
                vector[i] = (i == keywordIndex) ? 1.0 : 0.0;
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

            return (activeNeuronCount == 1) ? keywordIndex : -1; 
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
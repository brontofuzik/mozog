using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.INS02
{
    /* TODO
    class INS02BackpropagationTrainingStrategy : BackpropagationTrainingStrategy
    {
        public override IEnumerable< SupervisedTrainingPattern > TrainingPatterns
        {
            get
            {
                foreach (SupervisedTrainingPattern trainingPattern in TrainingSet)
                {
                    yield return trainingPattern;
                    yield return MutateTrainingPattern(trainingPattern);
                }
            }
        }

        private SupervisedTrainingPattern MutateTrainingPattern(SupervisedTrainingPattern trainigPattern)
        {
            // Modify the keyword.
            string keyword = (string)trainigPattern.Tag;
            string mutatedKeyword;
            do
            {
                mutatedKeyword = Program.MutateKeyword(keyword);
            }
            while (((StringCollection)TrainingSet.Tag).Contains(mutatedKeyword));

            // Create a new training pattern.
            double[] inputVector = Program.KeywordToVector(mutatedKeyword);
            double[] outputVector = Program.KeywordIndexToVector(-1);
            return new SupervisedTrainingPattern(inputVector, outputVector, mutatedKeyword);
        }       
    }
    */
}

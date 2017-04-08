using System;
using System.Text;

namespace NeuralNetwork.Training
{
    public class TrainingLog
    {
        public TrainingLog(int trainingIterations)
        {
            Iterations = trainingIterations;
        }

        public int Iterations { get; }

        public DataStatistics? TrainingSetStats { get; set; }

        public DataStatistics? ValidationSetStats { get; set; }

        public DataStatistics? TestSetStats { get; set; }

        public override string ToString()
        {
            var separator = new String('=', 40);

            var sb = new StringBuilder();

            sb.AppendLine(separator);
            sb.AppendLine("TRAINING LOG");
            sb.AppendLine("------------");
            sb.AppendLine($"Iterations\t{Iterations}");

            if (TrainingSetStats.HasValue)
            {
                sb.AppendLine($"Training set\t{TrainingSetStats.Value}");
            }

            if (ValidationSetStats.HasValue)
            {
                sb.AppendLine($"Validation set\t{ValidationSetStats.Value}");
            }

            if (TestSetStats.HasValue)
            {
                sb.AppendLine($"Test set\t{TestSetStats.Value}");
            }

            sb.Append(separator);

            return sb.ToString();
        }
    }
}

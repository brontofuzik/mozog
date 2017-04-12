using System;
using System.Text;
using NeuralNetwork.Data;

namespace NeuralNetwork.Training
{
    public class TrainingLog
    {
        public TrainingLog(int trainingIterations, double trainingError)
        {
            Iterations = trainingIterations;
            Error = trainingError;
        }

        public int Iterations { get; }

        public double Error { get; }

        public DataStatistics? TrainingSetStats { get; set; }

        public DataStatistics? ValidationSetStats { get; set; }

        public DataStatistics? TestSetStats { get; set; }

        public override string ToString()
        {
            const int width = 23;
            var separator = new String('=', width);

            var sb = new StringBuilder();

            sb.AppendLine(separator);
            sb.AppendLine("TRAINING LOG");
            sb.AppendLine(new String('-', width));
            sb.AppendLine($"Iterations\t{Iterations}");

            if (TrainingSetStats.HasValue)
            {
                sb.AppendLine($"Training err\t{TrainingSetStats.Value}");
            }

            if (ValidationSetStats.HasValue)
            {
                sb.AppendLine($"Validation err\t{ValidationSetStats.Value}");
            }

            if (TestSetStats.HasValue)
            {
                sb.AppendLine($"Test err\t{TestSetStats.Value}");
            }

            sb.Append(separator);

            return sb.ToString();
        }
    }
}

using System;
using System.Text;
using NeuralNetwork.Data;

namespace NeuralNetwork.Training
{
    public class TrainingLog
    {
        public TrainingLog(int trainingIterations)
        {
            Iterations = trainingIterations;
        }

        public int Iterations { get; }

        public DataStatistics TrainingStatistics { get; set; }

        public DataStatistics ValidationSetStats { get; set; }

        public DataStatistics TestSetStats { get; set; }

        public override string ToString()
        {
            const int width = 23;
            var separator = new String('=', width);

            var sb = new StringBuilder();

            sb.AppendLine(separator);
            sb.AppendLine("TRAINING LOG");
            sb.AppendLine(new String('-', width));
            sb.AppendLine($"Iterations\t{Iterations}");

            if (TrainingStatistics != null)
            {
                sb.AppendLine($"Training err\t{TrainingStatistics}");
            }

            if (ValidationSetStats != null)
            {
                sb.AppendLine($"Validation err\t{ValidationSetStats}");
            }

            if (TestSetStats != null)
            {
                sb.AppendLine($"Test err\t{TestSetStats}");
            }

            sb.Append(separator);

            return sb.ToString();
        }
    }
}

﻿using System;
using System.Text;

namespace NeuralNetwork.Training
{
    public class TestingLog
    {
        public DataStatistics DataStats { get; set; }

        public double? Accuracy { get; set; }

        public double? Precision { get; set; }

        public double? Recall { get; set; }

        public override string ToString()
        {
            var separator = new String('=', 40);

            var sb = new StringBuilder();

            sb.AppendLine(separator);
            sb.AppendLine("TESTING LOG");
            sb.AppendLine("-----------");
            sb.AppendLine($"Error\t\t{DataStats}");

            if (Accuracy.HasValue)
                sb.AppendLine($"Accuracy\t{Accuracy.Value:P2}");

            if (Precision.HasValue)
                sb.AppendLine($"Precision\t{Precision.Value:P2}");

            if (Recall.HasValue)
                sb.AppendLine($"Recall\t\t{Recall.Value:P2}");

            sb.Append(separator);

            return sb.ToString();
        }
    }
}
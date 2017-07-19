using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketForecaster
{
    class Log
    {
        private readonly StreamWriter writer;

        private readonly string[] header =
        {
            "Lags",
            "Hidden neurons",

            "# observations (n)",          
            "# parameters (p)",

            "Train:Error",
            "Train:MSE",

            "Test:Error",
            "Test:MSE",

            "Train:AIC",
            "Train:BIC"
        };

        private readonly List<string[]> entries = new List<string[]>();

        public Log(string logFilename)
        {
            writer = new StreamWriter(logFilename);
            entries.Add(header);
        }

        public void Write(params string[] entry)
        {
            entries.Add(entry);
        }

        public void Close()
        {
            var columnWidths = new int[header.Length];
            for (int column = 0; column < header.Length; column++)
            {
                columnWidths[column] = entries.Max(e => e[column].Length);
            }

            foreach (var entry in entries)
            {
                writer.WriteLine(FormatEntry(entry, columnWidths));
            }

            writer.Close();
        }

        private string FormatEntry(string[] entry, int[] columnWidths)
        {
            var sb = new StringBuilder();
            for (int column = 0; column < header.Length; column++)
            {
                sb.Append(entry[column]);

                var spacing = new String(' ', columnWidths[column] - entry[column].Length + 1);
                sb.Append(";" + spacing);
            }
            return sb.ToString();
        }
    }
}

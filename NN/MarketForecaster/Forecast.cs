using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mozog.Utils;

namespace MarketForecaster
{
    class Forecast
    {
        private static readonly char[] separator = { ';' };

        public static IEnumerable<Forecast> FromFile(string filename)
        {
            return File.ReadLines(filename)
                .Where(line => !String.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                .Select(line =>
            {
                var @params = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                var lags = @params[0].Trim().Split(',').Select(Int32.Parse).ToArray();
                var hiddenNeurons = int.Parse(@params[1].Trim());
                return new Forecast(lags, hiddenNeurons);
            });
        }

        public Forecast(int[] lags, int hiddenNeurons)
        {
            Lags = lags;
            HiddenNeurons = hiddenNeurons;
        }

        public int[] Lags { get; }

        public int HiddenNeurons { get; }

        public override string ToString()
        {
            string separator = new String('=', 60);

            var sb = new StringBuilder();
            sb.AppendLine(separator);
            sb.AppendLine($"Lags: {Vector.ToString(Lags)}, Hidden neurons: {HiddenNeurons}");
            sb.Append(separator);
            return sb.ToString();
        }
    }
}

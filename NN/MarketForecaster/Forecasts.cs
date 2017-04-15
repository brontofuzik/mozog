using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarketForecaster
{
    class Forecasts : IEnumerable<Forecast>
    {
        private readonly string filename;

        public Forecasts(string filename)
        {
            this.filename = filename;
        }

        public IEnumerator<Forecast> GetEnumerator()
            => File.ReadLines(filename).Select(l => new Forecast(l)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class Forecast
    {
        private readonly char[] separator = { ';' };

        public Forecast(string line)
        {
            string[] session = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            TestSize = Int32.Parse(session[0].Trim());
            Lags = session[1].Trim().Split(',').Select(Int32.Parse).ToArray();
            Leaps = session[2].Trim().Split(',').Select(Int32.Parse).ToArray();
            HiddenNeurons = int.Parse(session[3].Trim());
        }

        public int TestSize { get; }

        public int[] Lags { get; }

        public int[] Leaps { get; }

        public int HiddenNeurons { get; }
    }
}

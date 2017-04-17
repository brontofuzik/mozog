﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using LineSeries = OxyPlot.Series.LineSeries;

namespace MarketForecaster
{
    class TimeSeries
    {
        private static readonly char[] separator = { ' ' };
        private readonly List<double> dataPoints = new List<double>();

        public static TimeSeries FromFile(string filename)
        {
            var timeSeries = new TimeSeries();
            foreach (var line in File.ReadLines(filename))
            {
                if (line.Trim().Length == 0)
                    continue;

                timeSeries.dataPoints.AddRange(line.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse));
            }
            return timeSeries;
        }

        private int Count => dataPoints.Count;

        public double this[int i] => dataPoints[i];

        public DataSet BuildDataSet(int[] lags)
        {
            var trainingSet = new DataSet(lags.Length, 1);

            int maxLag = lags[lags.Length - 1];
            for (int i = maxLag; i < Count; i++)
            {
                var input = new double[lags.Length];
                for (int j = 0; j < input.Length; j++)
                {
                    input[j] = dataPoints[i - lags[j]];
                }
                var output = new[] {dataPoints[i]};

                trainingSet.Add(new LabeledDataPoint(input, output));
            }

            return trainingSet;
        }

        public void AddDataPoint(double dataPoint)
        {
            dataPoints.Add(dataPoint);
        }

        public void Plot(int[] lags, int hiddenNeurons)
        {
            var name = $"Airline.{Vector.ToString(lags)}.{hiddenNeurons}";
            var plotModel = CreatePlotModel(name);

            var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
            pngExporter.ExportToFile(plotModel, $"data\\Charts\\{name}.png");
        }

        private PlotModel CreatePlotModel(string name)
        {
            var plotModel = new PlotModel { Title = name };

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = dataPoints.Max() });

            var series = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            for (var i = 0; i < Count; i++)
            {
                series.Points.Add(new OxyPlot.DataPoint(i, dataPoints[i]));
            }

            plotModel.Series.Add(series);

            return plotModel;
        }
    }
}

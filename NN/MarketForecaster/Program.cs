using System;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.ErrorFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;

namespace MarketForecaster
{
    class Program
    {
        private static string timeSeriesFilename;       
        private static Log log;

        [STAThread]
        public static void Main(string[] args)
        {        
            try
            {
                var forecasts = Forecast.FromFile(args[0]);
                timeSeriesFilename = args[1];             
                log = new Log(args[2]);

                foreach (var forecast in forecasts)
                {
                    DoForecast(forecast);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Usage: MarketForecaster .forecast .timeSeries .log");
            }
            finally
            {
                log?.Close();
            }
        }

        private static void DoForecast(Forecast forecast)
        {
            Console.WriteLine(forecast);

            // Step 1: Training & test data

            var timeSeries = TimeSeries.FromFile(timeSeriesFilename);
            var trainingData = timeSeries.BuildDataSet(forecast.Lags);
            //var testData = trainingData.Split(size: 12, random: false);

            // Step 2: Network
           
            var architecture = NetworkArchitecture.Feedforward(new[]
            {
                (forecast.Lags.Length, null),
                (forecast.HiddenNeurons, Activation.Sigmoid),
                (1, Activation.Softplus)
            }, Error.MSE);
            var network = new Network(architecture);

            // Step 3: Train

            var trainer = new BackpropagationTrainer();
            trainer.WeightsUpdated += Trainer_WeightsUpdated;

            var args = BackpropagationArgs.Batch(Optimizer.RmsProp(0.01),
                maxError: 0.0005,
                maxIterations: 5_000);
            var trainingLog = trainer.Train(network, trainingData, args);

            // Step 4: Test

            //var testLog = trainer.Test(network, testData);

            log.Write(
                Vector.ToString(forecast.Lags),
                forecast.HiddenNeurons.ToString(),

                trainingData.Size.ToString(), // n
                network.SynapseCount.ToString(), // p

                trainingLog.TrainingStatistics?.AverageError.ToString(),
                trainingLog.TrainingStatistics?.MSE.ToString(),

                "N/A", //testLog.Statistics.AverageError.ToString(),
                "N/A", //testLog.Statistics.RMSE.ToString(),

                trainingLog.TrainingStatistics?.AIC.ToString(),
                trainingLog.TrainingStatistics?.BIC.ToString());

            // Step 5: Extrapolate

            foreach (var i in timeSeries.ForecastedIndices)
            {
                var input = new double[forecast.Lags.Length];
                for (int j = 0; j < input.Length; j++)
                {
                    input[j] = timeSeries[i - forecast.Lags[j]];
                }
                var output = network.EvaluateUnlabeled(input)[0];

                timeSeries.AddDataPoint(output);
            }

            timeSeries.Plot(forecast.Lags, forecast.HiddenNeurons);
        }

        private static void Trainer_WeightsUpdated(object sender, NeuralNetwork.Training.TrainingStatus e)
        {
            if (e.Iterations % 100 == 0)
            {
                Console.WriteLine($"Iterations: {e.Iterations}, Error: {e.Error}");
            }
        }
    }
}

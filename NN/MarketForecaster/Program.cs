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
        private static TimeSeries timeSeries;
        private static Forecasts forecasts;
        private static Log log;

        public static void Main(string[] args)
        {
            try
            {
                timeSeries = new TimeSeries(args[0]);
                forecasts = new Forecasts(args[1]);
                log = new Log(args[2]);

                foreach (var forecast in forecasts)
                {
                    Forecast(forecast);
                }
            }
            catch
            {
                Console.WriteLine("Usage: MarketForecaster .timeSeries .forecast .log");
            }
            finally
            {
                log?.Close();
            }
        }

        private static void Forecast(Forecast forecast)
        {
            // Step 1: Training & test data

            var trainingData = timeSeries.BuildTrainingSet(forecast.Lags, forecast.Leaps);
            var testData = trainingData.Split(forecast.TestSize, random: false);

            // Step 2: Network
           
            var architecture = NetworkArchitecture.Feedforward(new[]
            {
                (forecast.Lags.Length, null),
                (forecast.HiddenNeurons, Activation.Sigmoid),
                (forecast.Leaps.Length, Activation.Softplus)
            }, Error.MSE);
            var network = new Network(architecture);

            // Step 3: Train

            var trainer = new BackpropagationTrainer();
            trainer.WeightsUpdated += Trainer_WeightsUpdated;

            var args = BackpropagationArgs.Batch(Optimizer.RmsProp(0.01),
                maxError: 0.001,
                maxIterations: 5_000);
            var trainingLog = trainer.Train(network, trainingData, args);

            // Step 4: Test

            var testLog = trainer.Test(network, testData);

            log.Write(
                Vector.ToString(forecast.Lags),
                forecast.HiddenNeurons.ToString(),

                trainingData.Size.ToString(), // n
                network.SynapseCount.ToString(), // p

                trainingLog.TrainingStatistics?.Error.ToString(),
                trainingLog.TrainingStatistics?.MSE.ToString(),

                testLog.Statistics.Error.ToString(),
                testLog.Statistics.RSD.ToString(),

                trainingLog.TrainingStatistics?.AIC.ToString(),
                trainingLog.TrainingStatistics?.BIC.ToString());
        }

        private static void Trainer_WeightsUpdated(object sender, NeuralNetwork.Training.TrainingStatus e)
        {
            Console.WriteLine($"Iterations: {e.Iterations}, Error: {e.Error}");
        }
    }
}

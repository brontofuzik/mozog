﻿namespace NeuralNetwork.Interfaces
{
    public interface ISynapse
    {
        double Weight { get; set; }

        INeuron SourceNeuron { get; set; }

        INeuron TargetNeuron { get; set; }

        void Initialize();

        // TODO Jitter
        //void Jitter(double noiseLimit);
    }
}

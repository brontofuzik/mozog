﻿namespace NeuralNetwork.MLP
{
    public interface ISynapse
    {
        double Weight { get; set; }

        INeuron SourceNeuron { get; set; }

        INeuron TargetNeuron { get; set; }
    }
}

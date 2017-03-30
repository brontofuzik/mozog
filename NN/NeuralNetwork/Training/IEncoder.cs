using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Training
{
    public interface IEncoder<TInput, TOutput>
    {
        double[] EncodeInput(TInput input);

        TInput DecodeInput(double[] input);

        double[] EncodeOutput(TOutput output);

        TOutput DecodeOutput(double[] output);
    }
}

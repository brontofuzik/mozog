namespace NeuralNetwork.Interfaces
{
    public interface IEncoder<TInput, TOutput>
    {
        double[] EncodeInput(TInput input);

        TInput DecodeInput(double[] input);

        double[] EncodeOutput(TOutput output);

        TOutput DecodeOutput(double[] output);
    }
}

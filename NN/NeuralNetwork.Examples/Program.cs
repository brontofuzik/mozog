namespace NeuralNetwork.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            MultilayerPerceptron.Xor.Run();

            HopfieldNetwork.Examples.Run();

            KohonenNetwork.Examples.Run();
        }
    }
}

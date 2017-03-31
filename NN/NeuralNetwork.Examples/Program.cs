using NeuralNetwork.Examples.MultilayerPerceptron.LogicGates;

namespace NeuralNetwork.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Example.Run();

            HopfieldNetwork.Examples.Run();

            KohonenNetwork.Examples.Run();
        }
    }
}

namespace NeuralNetwork.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Multilayer perceptron
            //

            MultilayerPerceptron.LogicGates.Example.Run();
            //MultilayerPerceptron.Keywords.Example.Run();
            //MultilayerPerceptron.Iris.Example.Run();
            //MultilayerPerceptron.Tiling.Example.Run();

            //
            // Hopfield network
            //

            HopfieldNetwork.Simple.Run();
            HopfieldNetwork.EightRooks.Run();
            HopfieldNetwork.EightQueens.Run();
            HopfieldNetwork.Multiflop.Run();
            HopfieldNetwork.INS03.Example.Run();
            HopfieldNetwork.INS04.Example.Run();
        }
    }
}

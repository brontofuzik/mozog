namespace NeuralNetwork.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Multilayer perceptron
            //

            //MLP.LogicGates.Example.Run();
            //MLP.Keywords.Example.Run();
            //MLP.Iris.Example.Run();
            //MLP.Tiling.Example.Run();

            //
            // Hopfield network
            //

            //Hopfield.Simple.Run();
            //Hopfield.Multiflop.Run();
            //Hopfield.EightRooks.Run();
            //Hopfield.EightQueens.Run();
            //Hopfield.GrayscaleDithering.Run();
            //Hopfield.INS04.Example.Run();

            //
            // Kohonen network
            //

            Kohonen.Mapping.Test1DTo1D();
            Kohonen.Mapping.Test1DTo2D();
            Kohonen.Mapping.Test2DTo1D();
            Kohonen.Mapping.Test2DTo2D();
        }
    }
}

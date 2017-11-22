namespace NeuralNetwork.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //MLP();
            Hopfield();
            //Kohonen();
        }

        private static void MLP()
        {
            Examples.MLP.LogicGates.Example.Run();
            Examples.MLP.LogicGates.Example.Run();
            Examples.MLP.Keywords.Example.Run();
            Examples.MLP.Iris.Example.Run();
            Examples.MLP.Tiling.Example.Run();
        }

        private static void Hopfield()
        {
            Examples.Hopfield.Simple.Run();
            Examples.Hopfield.Multiflop.Run();
            Examples.Hopfield.EightRooks.Run();
            Examples.Hopfield.EightQueens.Run();
            Examples.Hopfield.GrayscaleDithering.Run();
            Examples.Hopfield.ColourDithering.Run();
        }

        private static void Kohonen()
        {
            Examples.Kohonen.Mapping.Map_1D_with_1D();
            Examples.Kohonen.Mapping.Map_2D_with_1D();
            Examples.Kohonen.Mapping.Map_2D_with_2D();
        }
    }
}

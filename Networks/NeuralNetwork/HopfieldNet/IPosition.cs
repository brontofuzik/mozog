namespace NeuralNetwork.HopfieldNet
{
    public interface IPosition
    {
        int Index { get; }
    }

    public struct Position1D : IPosition
    {
        private int index;

        public Position1D(int index)
        {
            this.index = index;
        }

        public int Index => index;
    }

    public struct Position2D : IPosition
    {
        private int width;

        public Position2D(int row, int col, int width)
        {
            Row = row;
            Col = col;
            this.width = width;
        }

        public int Row { get; }

        public int Col { get; }

        public int Index => Row * width + Col;
    }
}

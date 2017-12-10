using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Hopfield.HopfieldNetworkImps
{
    interface IMatrix
    {
        int Rows { get; }

        int Cols { get; }

        int Size { get; }

        double this[int row, int col] { get; set; }

        IEnumerable<int> GetSourceNeurons(int neuron);
    }

    abstract class MatrixBase
    {
        protected MatrixBase(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }

        public int Rows { get; }

        public int Cols { get; }

        public int Size => Rows * Cols;
    }

    class DenseMatrix : MatrixBase, IMatrix
    {
        private readonly double[,] elements;

        public DenseMatrix(int rows, int cols)
            : base(rows, cols)
        {
            elements = new double[rows, cols];
        }

        public double this[int row, int col]
        {
            get => elements[row, col];
            set => elements[row, col] = value;
        }

        public IEnumerable<int> GetSourceNeurons(int neuron)
        {
            for (int i = 0; i < Cols; i++)
                if (elements[neuron, i] != 0)
                    yield return i;
        }
    }

    // Inefficient
    class SparseMatrix : MatrixBase, IMatrix
    {
        private readonly IDictionary<int, IDictionary<int, double>> elements;

        public SparseMatrix(int rows, int cols)
            : base(rows, cols)
        {
            elements = new Dictionary<int, IDictionary<int, double>>(rows);
            for (int r = 0; r < rows; r++)
                elements[r] = new Dictionary<int, double>();
        }

        public double this[int row, int col]
        {
            get => elements[row].ContainsKey(col) ? elements[row][col] : 0.0;
            set => elements[row][col] = value;
        }

        public IEnumerable<int> GetSourceNeurons(int neuron)
            => elements[neuron].Where(kvp => kvp.Value != 0.0).Select(kvp => kvp.Key);
    }
}

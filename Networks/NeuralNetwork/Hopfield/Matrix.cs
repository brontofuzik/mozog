using System;
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
        public int Rows { get; }

        public int Cols { get; }

        protected MatrixBase(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }
    }

    class FullMatrix : MatrixBase, IMatrix
    {
        private readonly double[,] weights;

        public FullMatrix(int rows, int cols)
            : base(rows, cols)
        {
            weights = new double[rows, cols];
        }

        public int Size => weights.Length;

        public double this[int row, int col]
        {
            get => weights[row, col];
            set => weights[row, col] = value;
        }

        public IEnumerable<int> GetSourceNeurons(int neuron)
        {
            for (int i = 0; i < Cols; i++)
                if (weights[neuron, i] != 0)
                    yield return i;
        }
    }

    class SparseMatrix : MatrixBase, IMatrix
    {
        private readonly IDictionary<int, IDictionary<int, double>> weights;

        public SparseMatrix(int rows, int cols)
            : base(rows, cols)
        {
            weights = new Dictionary<int, IDictionary<int, double>>(rows);
            for (int r = 0; r < rows; r++)
                weights[r] = new Dictionary<int, double>();
        }

        public int Size => throw new NotImplementedException();

        public double this[int row, int col]
        {
            get => weights[row].ContainsKey(col) ? weights[row][col] : 0.0;
            set => weights[row][col] = value;
        }

        public IEnumerable<int> GetSourceNeurons(int neuron)
            => weights[neuron].Where(kvp => kvp.Value != 0.0).Select(kvp => kvp.Key);
    }
}

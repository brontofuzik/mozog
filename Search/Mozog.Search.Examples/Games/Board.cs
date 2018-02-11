using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozog.Search.Examples.Games
{
    public class Board
    {
        private readonly string[,] board;

        public Board(string[,] board)
        {
            this.board = board;
        }

        public int Rows => board.GetLength(0);

        public int Cols => board.GetLength(1);

        public IEnumerable<Square> Squares
        {
            get
            {
                for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    yield return new Square(col: c, row0: r, piece: board[r, c]);
            }
        }

        public string Debug => String.Concat(board.Cast<string>());

        public string this[int row, int col] => board[row, col];

        public string GetSquare(Square square)
            => IsWithinBoard(square.Row0, square.ColInt) ? board[square.Row0, square.ColInt] : null;

        public void SetSquare(Square square, string value)
        {
            if (IsWithinBoard(square.Row0, square.ColInt))
                board[square.Row0, square.ColInt] = value;
            else
                throw new ArgumentException(nameof(square));
        }

        public int SquareToIndex(Square square)
            => square.Row0 * Cols + square.ColInt;

        private bool IsWithinBoard(int row, int col)
            => 0 <= row && row < Rows && 0 <= col && col < Cols;

        public Board Clone() => new Board((string[,])board.Clone());

        public static char IntToChar(int i) => (char)('a' + i);

        public static int CharToInt(char c) => c - 'a';
    }

    public struct Square : IEquatable<Square>
    {
        public Square(int col, int row0, string piece = null)
        {
            ColInt = col;
            Row0 = row0;
            Piece = piece;
        }

        public Square(char col, int row1, string piece = null)
            : this(Board.CharToInt(col), row1 - 1, piece)
        {
        }

        public int ColInt { get; }

        public char ColChar => Board.IntToChar(ColInt);

        public int Row0 { get; }

        public int Row1 => Row0 + 1;

        public string Piece { get; }

        public bool Equals(Square other)
            => ColInt == other.ColInt && Row0 == other.Row0;

        public override string ToString() => $"{ColChar}{Row1}";
    }
}

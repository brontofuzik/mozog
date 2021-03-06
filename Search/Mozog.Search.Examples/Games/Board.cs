﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Search.Adversarial;
using Mozog.Utils;

namespace Mozog.Search.Examples.Games
{
    public class Board
    {
        private readonly string[,] board;

        // New board
        public Board(int rows, int cols)
        {
            this.board = new string[rows, cols];
        }

        // Existing board
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
                    yield return new Square(col0: c, row0: r, piece: board[r, c]);
            }
        }

        public string Debug => String.Concat(board.Cast<string>());

        public string this[int row, int col] => board[row, col];

        public string this[Square square] => board[square.Row0, square.Col0];

        public Board Initialize(Func<Square, string> initializer)
        {
            board.Initialize2D((r, c) => initializer(new Square(col0: c, row0: r)));
            return this;
        }

        public Board MakeMove(IChessboardMove move)
            => Clone().SetSquare(move.From, move.Empty).SetSquare(move.To, move.Piece.ToString());

        public string GetSquare(Square square)
            => IsWithinBoard(square.Row0, square.Col0) ? board[square.Row0, square.Col0] : null;

        public Board SetSquare(Square square, string value)
        {
            if (IsWithinBoard(square.Row0, square.Col0))
                board[square.Row0, square.Col0] = value;
            else
                throw new ArgumentException(nameof(square));
            return this;
        }

        public int SquareToIndex(Square square)
            => square.Row0 * Cols + square.Col0;

        public bool IsWithinBoard(int row, int col)
            => 0 <= row && row < Rows && 0 <= col && col < Cols;

        public Square? FindPiece(string piece)
            => Squares.FirstOrDefault(s => s.Piece == piece);

        public IEnumerable<Square> FindPieces(string piece)
            => Squares.Where(s => s.Piece == piece);

        public Board Clone() => new Board((string[,])board.Clone());

        public static char IntToChar(int i) => (char)('a' + i);

        public static int CharToInt(char c) => c - 'a';
    }

    public struct Square : IEquatable<Square>
    {
        public Square(int col0, int row0, string piece = null)
        {
            Col0 = col0;
            Row0 = row0;
            Piece = piece;
        }

        public Square(char col, int row1, string piece = null)
            : this(Board.CharToInt(col), row1 - 1, piece)
        {
        }

        public static Square Parse(string squareStr)
        {
            if (squareStr.Length != 2)
                throw new ArgumentException(nameof(squareStr));
            return new Square(squareStr[0], (int)Char.GetNumericValue(squareStr[1]));
        }

        public int Col0 { get; }

        public char ColChar => Board.IntToChar(Col0);

        public int Row0 { get; }

        public int Row1 => Row0 + 1;

        public char RowChar => Row1.ToString()[0];

        public string Piece { get; }

        public bool Equals(Square other)
            => Col0 == other.Col0 && Row0 == other.Row0;

        public bool Is(string pattern)
        {
            bool colMatch = pattern[0] == '*' || pattern[0] == ColChar;
            bool rowMatch = pattern[1] == '*' || pattern[1] == RowChar;
            return colMatch && rowMatch;
        }

        public override string ToString() => $"{ColChar}{Row1}";
    }

    public interface IChessboardMove : IAction
    {
        char Piece { get; }

        string Empty { get; }

        Square From { get; }

        Square To { get; }
    }
}

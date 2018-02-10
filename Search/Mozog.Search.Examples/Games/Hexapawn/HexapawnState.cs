﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mozog.Search.Adversarial;
using Mozog.Utils;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class HexapawnState : State
    {
        private readonly string[,] board;
        private readonly int movesPlayed;
        private /*readonly*/ Hexapawn game; // TODO Make readonly

        public HexapawnState(string[,] board, string playerToMove, int movesPlayed, Hexapawn game)
            : base(playerToMove)
        {
            this.board = board;
            this.movesPlayed = movesPlayed;
            this.game = game;
        }

        internal Hexapawn Game_DEBUG { set => game = value; }

        private bool WhiteToMove => PlayerToMove == Hexapawn.White;

        private bool BlackToMove => PlayerToMove == Hexapawn.Black;

        private string Opponent => WhiteToMove ? Hexapawn.Black : Hexapawn.White;

        public static IState CreateInitial(int rows, int cols, Hexapawn game)
            => new HexapawnState(InitialBoard(rows, cols), Hexapawn.White, 0, game);

        private static string[,] InitialBoard(int rows, int cols)
            => new string[rows, cols].Initialize2D((r, c) => Fn.Switch(r, Hexapawn.Empty,
                (r1 => r1 == 0, Hexapawn.White),
                (r2 => r2 == rows - 1, Hexapawn.Black)));

        #region Move

        public override IEnumerable<IAction> GetLegalMoves()
            => board.Squares().Where(s => s.Piece == PlayerToMove)
                .SelectMany(s => GetLegalMovesForPiece(s.Row0, s.ColInt));

        private IEnumerable<IAction> GetLegalMovesForPiece(int row, int col)
        {
            var current = new HexapawnSquare(col, row);
            int newRow = WhiteToMove ? row + 1 : row - 1;

            var moveForward = new HexapawnSquare(col, newRow);
            if (board.GetSquare(moveForward) == Hexapawn.Empty)
                yield return new HexapawnMove(current, moveForward);

            var captureRight = new HexapawnSquare(WhiteToMove ? col + 1 : col - 1, newRow);
            if (board.GetSquare(captureRight) == Opponent)
                yield return new HexapawnMove(current, captureRight);

            var captureLeft = new HexapawnSquare(WhiteToMove ? col - 1 : col + 1, newRow);
            if (board.GetSquare(captureLeft) == Opponent)
                yield return new HexapawnMove(current, captureLeft);
        }

        public override IState MakeMove(IAction action)
            => new HexapawnState(NewBoard((HexapawnMove)action), Opponent, movesPlayed + 1, game);

        private string[,] NewBoard(HexapawnMove action)
        {
            var newBoard = (string[,])board.Clone();
            newBoard.SetSquare(action.From, Hexapawn.Empty);
            newBoard.SetSquare(action.To, PlayerToMove);
            return newBoard;
        }

        #endregion // Move

        #region Evaluate

        // No draws
        protected override GameResult GetResult()
        {
            if (WhiteWon) return GameResult.Win1;
            if (BlackWon) return GameResult.Win2;
            return GameResult.InProgress;
        }

        protected override double? EvaluateTerminal()
        {
            switch (Result)
            {
                case GameResult.Win1: return +10.0 + (10.0 / movesPlayed);
                case GameResult.Win2: return -10.0 - (10.0 / movesPlayed);
                default: return null; // GameResult.InProgress
            }
        }

        protected override double Evaluate() => EvaluateTerminal() ?? EvaluateNonTerminal();

        private double EvaluateNonTerminal()
        {
            // Pawn balance
            int whitePawns = board.Squares().Count(s => s.Piece == Hexapawn.White);
            int blackPawns = board.Squares().Count(s => s.Piece == Hexapawn.Black);
            return whitePawns - blackPawns;
        }

        // Either White has queened or Black has no legal moves
        public bool WhiteWon
            => board.Squares().Where(s => s.Row0 == board.Rows() - 1).Any(s => s.Piece == Hexapawn.White)
            || BlackToMove && GetLegalMoves().None();

        // Either Black has queened or White has no legal moves
        public bool BlackWon
            => board.Squares().Where(s => s.Row0 == 0).Any(s => s.Piece == Hexapawn.Black)
            || WhiteToMove && GetLegalMoves().None();

        #endregion // Evaluate

        #region Transposition table

        private int? hash;
        public override int Hash => hash ?? (hash = CalculateHash()) ?? 0;

        //// Simple hashing
        //private int CalculateHash()
        //{
        //    const int prime = 31;

        //    int h = 1;
        //    foreach (var square in board)
        //        h = h * prime + square.GetHashCode();
        //    return h * prime + PlayerToMove.GetHashCode();
        //}

        // Zobrist hashing (new)
        private int CalculateHash()
        {
            int SquareHash(HexapawnSquare square)
                => game.Table[board.SquareToIndex(square), board.GetSquare(square) == Hexapawn.White ? 0 : 1];

            var boardHash = board.Squares().Where(s => s.Piece != Hexapawn.Empty)
                .Aggregate(0, (h, s) => h ^ SquareHash(s));

            var playerHash = WhiteToMove ? game.Table_WhiteToMove : game.Table_BlackToMove;

            return boardHash ^ playerHash;
        }

        #endregion // Transposition table

        public override string ToString()
        {
            string PrintRow(int row)
                => $"│{string.Join("│", Enumerable.Range(0, board.Cols()).Select(c => board[row, c]))}│";

            var Bar = $"{new String('─', 2 * board.Cols() + 1)}";

            // Board
            var sb = new StringBuilder();
            sb.Append(Bar).Append(Environment.NewLine);
            for (int r = board.Rows() - 1; r >= 0; r--)
                sb.Append(PrintRow(r)).Append(Environment.NewLine)
                    .Append(Bar).Append(Environment.NewLine);

            // Player to move
            var player = PlayerToMove == Hexapawn.White ? "White" : "Black";
            sb.Append($"{player} to move").Append(Environment.NewLine);

            sb.Append(Bar);

            return sb.ToString();
        }

        public override string Debug => $"{string.Concat(board.Cast<string>())}|{PlayerToMove}";
    }

    internal static class BoardUtils
    {
        public static int Rows(this string[,] board) => board.GetLength(0);

        public static int Cols(this string[,] board) => board.GetLength(1);

        // Current board by default
        public static string GetSquare(this string[,] board, HexapawnSquare square)
            => board.IsWithinBoard(square.Row0, square.ColInt) ? board[square.Row0, square.ColInt] : null;

        // Current board by default
        public static void SetSquare(this string[,] board, HexapawnSquare square, string value)
        {
            if (board.IsWithinBoard(square.Row0, square.ColInt))
                board[square.Row0, square.ColInt] = value;
            else
                throw new ArgumentException(nameof(square));
        }

        public static int SquareToIndex(this string[,] board, HexapawnSquare square)
            => square.Row0 * board.Cols() + square.ColInt;

        private static bool IsWithinBoard(this string[,] board, int row, int col)
            => 0 <= row && row < board.Rows() && 0 <= col && col < board.Cols();

        public static IEnumerable<HexapawnSquare> Squares(this string[,] board)
        {
            for (int r = 0; r < board.Rows(); r++)
            for (int c = 0; c < board.Cols(); c++)
                yield return new HexapawnSquare(col: c, row0: r, piece: board[r, c]);
        }

        public static char IntToChar(int i) => (char)('a' + i);

        public static int CharToInt(char c) => c - 'a';
    }
}

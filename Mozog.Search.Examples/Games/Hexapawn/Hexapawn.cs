﻿using Mozog.Search.Adversarial;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace Mozog.Search.Examples.Games.Hexapawn
{
    public class Hexapawn : Game
    {
        public static void Play_Minimax(int cols, int rows, bool tt)
        {
            var hexapawn = new Hexapawn(rows: rows, cols: cols);
            var engine = GameEngine.Minimax(hexapawn, tt: tt);
            engine.Play();
        }

        public static void Play_Minimax_DEBUG(int cols, int rows, bool tt, IState initialState)
        {
            var hexapawn = new Hexapawn(rows: rows, cols: cols);
            var engine = GameEngine.Minimax(hexapawn, tt: tt);

            // DEBUG
            if (initialState is HexapawnState state)
                state.Game_DEBUG = hexapawn;

            engine.Play_DEBUG(initialState);
        }

        public static void Play_AlphaBeta(int cols, int rows, bool tt)
        {
            var hexapawn = new Hexapawn(rows: rows, cols: cols);
            var engine = GameEngine.AlphaBeta(hexapawn, tt: tt);
            engine.Play();
        }

        public static void Play_AlphaBeta_DEBUG(int cols, int rows, bool tt, IState initialState)
        {
            var hexapawn = new Hexapawn(rows: rows, cols: cols);
            var engine = GameEngine.AlphaBeta(hexapawn, tt: tt);

            // DEBUG
            if (initialState is HexapawnState state)
                state.Game_DEBUG = hexapawn;

            engine.Play_DEBUG(initialState);
        }

        public const string White = "W";
        public const string Black = "B";
        public const string Empty = " ";

        private readonly int rows;
        private readonly int cols;

        // Transposition table
        internal int[,] Table { get; private set; }
        internal int Table_WhiteToMove { get; private set; }
        internal int Table_BlackToMove { get; private set; }

        public Hexapawn(int rows = 3, int cols = 3)
        {
            this.rows = rows;
            this.cols = cols;

            InitializeTranspositionTable();
        }

        private void InitializeTranspositionTable()
        {
            Table = new int[rows * cols, 2];
            Table.Initialize2D((i1, i2) => StaticRandom.Int());
            Table_WhiteToMove = StaticRandom.Int();
            Table_BlackToMove = StaticRandom.Int();
        }

        internal int Rows_DEBUG => rows;

        internal int Cols_DEBUG => cols;

        public override string[] Players { get; } = { White, Black };

        public override IState InitialState => HexapawnState.CreateInitial(rows, cols, this);

        public override Objective GetObjective(string player)
            => player == White ? Objective.Max : Objective.Min;

        public override IAction ParseMove(string moveStr, string player)
            => HexapawnMove.Parse(moveStr, player);
    }
}

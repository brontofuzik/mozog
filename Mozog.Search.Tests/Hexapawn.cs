﻿using System.Collections.Generic;
using System.Linq;
using Mozog.Search.Adversarial;
using Mozog.Search.Examples.Games.Hexapawn;
using HexapawnGame = Mozog.Search.Examples.Games.Hexapawn.Hexapawn;
using Mozog.Utils;
using Mozog.Utils.Math;
using NUnit.Framework;

namespace Mozog.Search.Tests
{
    [TestFixture]
    public class Hexapawn
    {
        private readonly HexapawnGame game;

        public Hexapawn()
        {
            //StaticRandom.Seed = 42;
            game = new HexapawnGame(4, 4);
        }

        [Test]
        public void All_states_should_have_unique_hash()
        {
            var transTable = new TranspositionTable();

            var allStates = GenerateAllStates();
            foreach (var state in allStates)
            {
                var eval = transTable.RetrieveEvaluation(state);
                Assert.That(eval, Is.Null);

                transTable.StoreEvaluation(state, 0.0);
            }
        }

        private IEnumerable<IState> GenerateAllStates()
            => GenerateAllStatesRecursive(new string[4, 4], 0);

        private IEnumerable<IState> GenerateAllStatesRecursive(string [,] board, int index)
        {
            if (index == board.Length)
            {
                yield return new HexapawnState(board, HexapawnGame.White, 0, game);
                yield return new HexapawnState(board, HexapawnGame.Black, 0, game);
            }
            else
            {
                var emptyBoard = (string[,])board.Clone();
                emptyBoard.Set(index, HexapawnGame.Empty);
                var empty = GenerateAllStatesRecursive(emptyBoard, index + 1);

                var whiteBoard = (string[,])board.Clone();
                whiteBoard.Set(index, HexapawnGame.White);
                var white = GenerateAllStatesRecursive(whiteBoard, index + 1);

                var blackBoard = (string[,])board.Clone();
                blackBoard.Set(index, HexapawnGame.Black);
                var black = GenerateAllStatesRecursive(blackBoard, index + 1);

                foreach (var s in empty.Concat(white).Concat(black))
                    yield return s;
            }
        }
    }
}

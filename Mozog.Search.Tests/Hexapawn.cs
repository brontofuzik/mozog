using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Search.Adversarial;
using Mozog.Search.Examples.Games.Hexapawn;
using HexapawnGame = Mozog.Search.Examples.Games.Hexapawn.Hexapawn;
using Mozog.Utils;
using Mozog.Utils.Math;
using NUnit.Framework;
using Math = System.Math;

namespace Mozog.Search.Tests
{
    [TestFixture]
    public class Hexapawn
    {
        private readonly HexapawnGame game;
        private readonly MinimaxSearch<(double alpha, double beta)> minimax;
        private readonly MinimaxSearch<(double alpha, double beta)> minimax_tt;

        public Hexapawn()
        {
            //StaticRandom.Seed = 42;
            game = new HexapawnGame(cols: 4, rows: 4);
            minimax = MinimaxSearch.AlphaBeta(game, tt: false);
            minimax_tt = MinimaxSearch.AlphaBeta(game, tt: true);
        }

        // 1. b2 (b3/d3)
        [Test]
        public void Check_difference_between_wtt_and_wott1()
        {
            var state = new HexapawnState(new[,]
            {
                { HexapawnGame.White, HexapawnGame.Empty, HexapawnGame.White, HexapawnGame.White },
                { HexapawnGame.Empty, HexapawnGame.White, HexapawnGame.Empty, HexapawnGame.Empty },
                { HexapawnGame.Empty, HexapawnGame.Empty, HexapawnGame.Empty, HexapawnGame.Empty },
                { HexapawnGame.Black, HexapawnGame.Black, HexapawnGame.Black, HexapawnGame.Black }
            }, HexapawnGame.Black, 0, game);

            var (move, eval, nodes) = minimax.MakeDecision_DEBUG(state);
            var (move_tt, eval_tt, nodes_tt) = minimax_tt.MakeDecision_DEBUG(state);

            Assert.That(move.ToString() == move_tt.ToString(), $"W/o tt: {move}\nWith tt: {move_tt}");
            Assert.That(Math.Sign(eval) == Math.Sign(eval_tt));
            Assert.That(nodes >= nodes_tt);
        }

        // 1. d2 a3 2. c2 (a2/d3)
        [Test]
        public void Check_difference_between_wtt_and_wott2()
        {
            var state = new HexapawnState(new [,]
            {
                { HexapawnGame.White, HexapawnGame.White, HexapawnGame.Empty, HexapawnGame.Empty },
                { HexapawnGame.Empty, HexapawnGame.Empty, HexapawnGame.White, HexapawnGame.White },
                { HexapawnGame.Black, HexapawnGame.Empty, HexapawnGame.Empty, HexapawnGame.Empty },
                { HexapawnGame.Empty, HexapawnGame.Black, HexapawnGame.Black, HexapawnGame.Black }
            }, HexapawnGame.Black, 0, game);

            var (move, eval, nodes) = minimax.MakeDecision_DEBUG(state);
            var (move_tt, eval_tt, nodes_tt) = minimax_tt.MakeDecision_DEBUG(state);

            Assert.That(move.ToString() == move_tt.ToString(), $"W/o tt: {move}\nWith tt: {move_tt}");
            Assert.That(Math.Sign(eval) == Math.Sign(eval_tt));
            Assert.That(nodes >= nodes_tt);
        }

        [Test]
        public void Find_differences_between_wtt_and_wott()
        {
            const string enginePlayer = HexapawnGame.Black;

            var record = new GameRecord();
            
            var state = game.InitialState;
            while (!state.IsTerminal)
            {
                var move = game.GetPlayer(state) == enginePlayer
                    ? GetEngineMove(state, record)
                    : GetRandomMove(state);
                state = game.GetResult(state, move);
                record.AddMove(move);
            }
        }

        private IAction GetEngineMove(IState state, GameRecord record)
        {
            var (move, eval, nodes) = minimax.MakeDecision_DEBUG(state);
            var (move_tt, eval_tt, nodes_tt) = minimax_tt.MakeDecision_DEBUG(state);

            Assert.That(move.ToString() == move_tt.ToString(), $"{record}\n{state}\nW/o tt: {move}\nWith tt: {move_tt}");
            Assert.That(Math.Sign(eval) == Math.Sign(eval_tt));
            Assert.That(nodes >= nodes_tt);

            return move;
        }

        private IAction GetRandomMove(IState state)
        {
            var moves = game.GetActions(state);
            return StaticRandom.Pick(moves.ToList());
        }

        [Test]
        public void All_reachable_states_should_have_unique_hash()
        {
            var transTable = new TranspositionTable();

            var allReachableStates = GenerateAllReachableStates();
            foreach (var state in allReachableStates)
            {
                var eval = transTable.RetrieveEvaluation(state);
                //Assert.That(eval, Is.Null);

                transTable.StoreEvaluation(state, 0.0);
            }
        }

        private IEnumerable<IState> GenerateAllReachableStates()
            => GenerateAllReachableStatesRecursive(game.InitialState);

        private IEnumerable<IState> GenerateAllReachableStatesRecursive(IState state)
        {
            if (state.IsTerminal)
            {
                yield return state;
            }
            else
            {
                var states = new List<IState> { state };
                foreach (var (_, nextState) in game.GetActionsAndResults(state))
                    states.AddRange(GenerateAllReachableStatesRecursive(nextState));

                // yield return IEnumerable
                foreach (var s in states)
                    yield return s;
            }
        }

        //[Test]
        //public void All_states_should_have_unique_hash()
        //{
        //    var transTable = new TranspositionTable();

        //    var allStates = GenerateAllStates();
        //    foreach (var state in allStates)
        //    {
        //        var eval = transTable.RetrieveEvaluation(state);
        //        //Assert.That(eval, Is.Null);

        //        transTable.StoreEvaluation(state, 0.0);
        //    }
        //}

        //private IEnumerable<IState> GenerateAllStates()
        //    => GenerateAllStatesRecursive(new string[game.Rows_DEBUG, game.Cols_DEBUG], 0);

        //private IEnumerable<IState> GenerateAllStatesRecursive(string [,] board, int index)
        //{
        //    if (index == board.Length)
        //    {
        //        yield return new HexapawnState(board, HexapawnGame.White, 0, game);
        //        yield return new HexapawnState(board, HexapawnGame.Black, 0, game);
        //    }
        //    else
        //    {
        //        var emptyBoard = (string[,])board.Clone();
        //        emptyBoard.Set(index, HexapawnGame.Empty);
        //        var empty = GenerateAllStatesRecursive(emptyBoard, index + 1);

        //        var whiteBoard = (string[,])board.Clone();
        //        whiteBoard.Set(index, HexapawnGame.White);
        //        var white = GenerateAllStatesRecursive(whiteBoard, index + 1);

        //        var blackBoard = (string[,])board.Clone();
        //        blackBoard.Set(index, HexapawnGame.Black);
        //        var black = GenerateAllStatesRecursive(blackBoard, index + 1);

        //        // yield return IEnumerable
        //        foreach (var s in empty.Concat(white).Concat(black))
        //            yield return s;
        //    }
        //}
    }

    internal class GameRecord
    {
        private readonly IList<IAction> moves = new List<IAction>();

        public void AddMove(IAction move)
        {
            moves.Add(move);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < moves.Count; i++)
            {
                if (i % 2 == 0)
                {
                    sb.Append($" {i/2 + 1}.");
                }
                sb.Append($" {moves[i]}");
            }

            return sb.ToString();
        }
    }
}

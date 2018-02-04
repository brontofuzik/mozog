using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Search.Adversarial;
using Mozog.Search.Examples.Games.Hexapawn;
using Mozog.Utils;
using HexapawnGame = Mozog.Search.Examples.Games.Hexapawn.Hexapawn;
using Mozog.Utils.Math;
using NUnit.Framework;
using Math = System.Math;

namespace Mozog.Search.Tests
{
    [TestFixture]
    public class Hexapawn
    {
        private const string _ = HexapawnGame.Empty;
        private const string W = HexapawnGame.White;
        private const string B = HexapawnGame.Black;

        private readonly HexapawnGame game;

        // Minimax
        private readonly MinimaxSearch minimax;
        private readonly MinimaxSearch minimax_tt;
        private readonly MinimaxSearch minimax_ab;
        private readonly MinimaxSearch minimax_ab_tt;

        // Negamax
        private readonly NegamaxSearch negamax;
        private readonly NegamaxSearch negamax_ab;
        private readonly NegamaxSearch negamax_ab_tt;

        public Hexapawn()
        {
            StaticRandom.Seed = 42;
            game = new HexapawnGame(cols: 4, rows: 5);

            minimax = new MinimaxSearch(game, prune: false, tt: false);
            //minimax_tt = new MinimaxSearch(game, prune: false, tt: true);;
            minimax_ab = new MinimaxSearch(game, prune: true, tt: false);
            minimax_ab_tt = new MinimaxSearch(game, prune: true, tt: true);

            negamax = new NegamaxSearch(game, prune: false, tt: false);
            negamax_ab = new NegamaxSearch(game, prune: true, tt: false);
            negamax_ab_tt = new NegamaxSearch(game, prune: true, tt: true);
        }

        [Test]
        public void Minimax_check_difference()
        {
            var state = CreateState(HexapawnGame.White, new[,]
            {
                { _, B, _, B },
                { _, _, B, _ },
                { W, _, _, _ },
                { W, _, _, _ },
                { _, _, W, W }
            });

            var mm = minimax.MakeDecision_DEBUG(state);
            //var mm_tt = minimax_tt.MakeDecision_DEBUG(state);
            var mm_ab = minimax_ab.MakeDecision_DEBUG(state);
            var mm_ab_tt = minimax_ab_tt.MakeDecision_DEBUG(state);

            Assert.That(mm_ab.move.ToString() == mm_ab_tt.move.ToString(), $"W/o tt: {mm_ab.move}\nWith tt: {mm_ab_tt.move}");
            Assert.That(Math.Sign(mm_ab.eval) == Math.Sign(mm_ab_tt.eval));
            Assert.That(mm_ab.nodes >= mm_ab_tt.nodes);
        }

        [Test]
        public void Negamax_check_difference()
        {
            var state = CreateState(HexapawnGame.White, new[,]
            {
                { _, B, _, B },
                { _, _, B, _ },
                { W, _, _, _ },
                { W, _, _, _ },
                { _, _, W, W }
            });

            var nm = negamax.MakeDecision_DEBUG(state);
            var nm_ab = negamax_ab.MakeDecision_DEBUG(state);
            var nm_ab_tt = negamax_ab_tt.MakeDecision_DEBUG(state);

            Assert.That(nm_ab.move.ToString() == nm_ab_tt.move.ToString(), $"W/o tt: {nm_ab.move}\nWith tt: {nm_ab_tt.move}");
            Assert.That(Math.Sign(nm_ab.eval) == Math.Sign(nm_ab_tt.eval));
            Assert.That(nm_ab.nodes >= nm_ab_tt.nodes);
        }

        private HexapawnState CreateState(string playerToMove, string[,] board)
        {
            var boardReversed = board.ToJaggedArray().Reverse().ToArray().ToMultidimArray();
            return new HexapawnState(boardReversed, playerToMove, 0, game);
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
            var (move, eval, nodes) = minimax_ab.MakeDecision_DEBUG(state);
            var (move_tt, eval_tt, nodes_tt) = minimax_ab_tt.MakeDecision_DEBUG(state);

            Assert.That(move.ToString() == move_tt.ToString(), $"{record}\n{state}\nW/o tt: {move}\nWith tt: {move_tt}");
            Assert.That(Math.Sign(eval) == Math.Sign(eval_tt));
            //Assert.That(nodes >= nodes_tt);

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
                var eval = transTable.Lookup(state);
                Assert.That(eval, Is.Null);

                var entry = new TTEntry { Eval = 0.0, Action = null, Flag = TTFlag.Exact };
                transTable.Store(state, entry);
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

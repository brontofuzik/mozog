using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozog.Search
{
    public class MinimaxSearch : ISearch
    {
        private readonly IGame game;

        public MinimaxSearch(IGame game)
        {
            this.game = game;
        }

        public IAction MakeDecision(IState state)
        {
            IAction result = null;
            double resultValue = Double.MinValue;

            string player = game.GetPlayer(state);

            foreach (var action in game.GetActions(state))
            {
                double value = MinValue(game.GetResult(state, action), player);
                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }

            return result;
        }

        private double MinValue(IState state, string player)
        {
            if (game.IsTerminal(state))
                return game.GetUtility(state, player).Value;

            double value = Double.MaxValue;
            foreach (IAction action in game.GetActions(state))
                value = Math.Min(value, MaxValue(game.GetResult(state, action), player));

            return value;
        }

        private double MaxValue(IState state, string player)
        {
            if (game.IsTerminal(state))
                return game.GetUtility(state, player).Value;

            double value = Double.MinValue;
            foreach (IAction action in game.GetActions(state))
                value = Math.Max(value, MinValue(game.GetResult(state, action), player));
            return value;
        }
    }
}

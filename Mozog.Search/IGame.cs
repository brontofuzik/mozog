using System.Collections.Generic;

namespace Mozog.Search
{
    interface IGame<TState, TAction, TPlayer>
    {
        TState InitialState { get; }

        IList<TPlayer> Players { get; }

        TPlayer GetPlayer(TState state);

        IList<TAction> GetActions(TState state);

        bool IsTerminal(TState state);

        TState GetReult(TState state, TAction action);

        double? GetUtility(TState state, TPlayer player);
    }
}

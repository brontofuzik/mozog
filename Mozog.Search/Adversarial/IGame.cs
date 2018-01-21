using System;
using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public interface IGame
    {
        string[] Players { get; }

        IState InitialState { get; }

        Objective GetObjective(string player);

        // Minimax
        string GetPlayer(IState state);

        // Minimax
        IEnumerable<IAction> GetActions(IState state);

        // Minimax
        bool IsTerminal(IState state);

        bool IsLegalMove(IState state, IAction move);

        // Minimax
        IState GetResult(IState state, IAction action);

        double? GetUtility(IState state);

        string PrintState(IState state);

        IAction ParseMove(string moveStr, string player);
    }

    public interface IAction : IEquatable<IAction>
    {
    }
}

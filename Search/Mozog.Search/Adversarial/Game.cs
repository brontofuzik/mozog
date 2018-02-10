using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozog.Search.Adversarial
{
    public abstract class Game : IGame
    {
        public abstract string[] Players { get; }

        public abstract IState InitialState { get; }

        public abstract Objective GetObjective(string player);
  
        public virtual string GetPlayer(IState state)
            => state.PlayerToMove;

        public virtual IEnumerable<IAction> GetActions(IState state)
            => state.GetLegalMoves();

        public virtual IState GetResult(IState state, IAction action)
            => state.MakeMove(action);

        public IEnumerable<(IAction, IState)> GetActionsAndResults(IState state)
            => GetActions(state).Select(a => (a, GetResult(state, a)));

        public virtual bool IsTerminal(IState state)
            => state.IsTerminal;

        public virtual bool IsLegalMove(IState state, IAction move)
            => state.IsLegalMove(move);

        public virtual double? GetUtility(IState state)
            => state.Evaluation;

        public virtual string PrintState(IState state)
            => state.ToString();

        public abstract IAction ParseMove(string moveStr, string player);
    }

    public interface IGame
    {
        string[] Players { get; }

        IState InitialState { get; }

        Objective GetObjective(string player);

        string GetPlayer(IState state);

        IEnumerable<IAction> GetActions(IState state);

        IState GetResult(IState state, IAction action);

        IEnumerable<(IAction, IState)> GetActionsAndResults(IState state);

        bool IsLegalMove(IState state, IAction move);

        bool IsTerminal(IState state);

        double? GetUtility(IState state);

        #region UI

        string PrintState(IState state);

        IAction ParseMove(string moveStr, string player);

        #endregion // UI
    }

    public interface IAction : IEquatable<IAction>
    {
    }
}

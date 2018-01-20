using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public abstract class GameBase : IGame
    {
        public abstract string[] Players { get; }

        public abstract IState InitialState { get; }

        public abstract Objective GetObjective(string player);
  
        public virtual string GetPlayer(IState state)
            => state.PlayerToMove;

        public virtual IList<IAction> GetActions(IState state)
            => state.GetLegalMoves();

        public virtual IState GetResult(IState state, IAction action)
            => state.MakeMove(action);

        public virtual double? GetUtility(IState state)
            => state.Evaluation;

        public virtual bool IsTerminal(IState state)
            => state.IsTerminal;

        public virtual string PrintState(IState state)
            => state.ToString();

        public abstract IAction ParseMove(string moveStr);
    }
}

using System.Linq;
using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public abstract class State : IState
    {
        protected State(string playerToMove)
        {
            PlayerToMove = playerToMove;
        }

        public string PlayerToMove { get; }

        public bool IsTerminal => Evaluation.HasValue;

        private double? evaluation = null;
        public virtual double? Evaluation => evaluation ?? (evaluation = Evaluate());

        protected abstract double? Evaluate();

        public abstract IEnumerable<IAction> GetLegalMoves();

        public virtual bool IsLegalMove(IAction move)
            => GetLegalMoves().Contains(move);

        public abstract IState MakeMove(IAction action);

        public virtual int Hash => 0;

        public abstract string Debug { get; }
    }

    public interface IState
    {
        bool IsTerminal { get; }

        string PlayerToMove { get; }

        double? Evaluation { get; }

        IEnumerable<IAction> GetLegalMoves();

        bool IsLegalMove(IAction move);

        IState MakeMove(IAction action);

        int Hash { get; }

        string Debug { get; }
    }
}

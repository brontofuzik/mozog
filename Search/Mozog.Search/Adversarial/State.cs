using System;
using System.Linq;
using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public abstract class State : IState
    {
        private readonly Lazy<GameResult> result;

        // Null evaluation means the state hasn't been evaluated yet.
        private double? evaluation;

        protected State(string playerToMove)
        {
            PlayerToMove = playerToMove;
            result = new Lazy<GameResult>(GetResult);
        }

        public virtual string PlayerToMove { get; }

        protected GameResult Result => result.Value;

        public virtual bool IsTerminal => Result != GameResult.InProgress;

        public virtual double? Evaluation => evaluation ?? (evaluation = EvaluateTerminal());

        public virtual double Evaluation_NEW => evaluation ?? (evaluation = Evaluate()).Value;

        private int? hash;
        public virtual int Hash => hash ?? (int)(hash = CalculateHash());

        public abstract IEnumerable<IAction> GetLegalMoves();

        public virtual bool IsLegalMove(IAction move)
            => GetLegalMoves().Contains(move);

        public abstract IState MakeMove(IAction action);

        // Guaranteed to be called once
        protected abstract GameResult GetResult();

        // Guaranteed to be called once
        protected virtual double Evaluate() => EvaluateTerminal() ?? EvaluateNonTerminal();

        // Guaranteed to be called once
        protected abstract double? EvaluateTerminal();

        protected abstract double EvaluateNonTerminal();

        // Guaranteed to be called once
        protected virtual int CalculateHash() => 0;

        public abstract string Debug { get; }
    }

    public interface IState
    {
        string PlayerToMove { get; }

        bool IsTerminal { get; }

        double? Evaluation { get; }

        double Evaluation_NEW { get; }

        IEnumerable<IAction> GetLegalMoves();

        bool IsLegalMove(IAction move);

        IState MakeMove(IAction action);

        int Hash { get; }

        string Debug { get; }
    }
}

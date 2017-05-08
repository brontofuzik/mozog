using System;
using System.Linq;

namespace SimulatedAnnealing
{
    public struct State<T>
    {
        private readonly SimulatedAnnealing<T> algo;

        public State(SimulatedAnnealing<T> algo)
            : this(algo, algo.Initializer.Initialize(algo.Dimension))
        {
        }

        private State(SimulatedAnnealing<T> algo, T[] state)
        {
            this.algo = algo;
            S = state;
            E = algo.Objective.Evaluate(S);
        }

        public T[] S { get; }

        public double E { get; }

        public State<T> Perturb() => new State<T>(algo, algo.Perturbator.Perturb(S));

        public override string ToString()
        {
            var state = S.Select(s => s is double ? Convert.ToDouble(s).ToString("F2") : s.ToString());
            return $"[{String.Join(", ", state)}] = {E:F2}";
        }
    }
}
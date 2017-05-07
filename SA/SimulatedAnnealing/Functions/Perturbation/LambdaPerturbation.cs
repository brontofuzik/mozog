using System;

namespace SimulatedAnnealing.Functions.Perturbation
{
    public class LambdaPerturbation<T> : PerturbationFunction<T>
    {
        private readonly Func<T[], T[]> perturb;

        public LambdaPerturbation(Func<T[], T[]> perturb)
        {
            this.perturb = perturb;
        }

        public override T[] Perturb(T[] state) => perturb(state);
    }
}

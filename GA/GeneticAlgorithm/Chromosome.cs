using System;
using System.Collections.Generic;
using Random = Mozog.Utils.Random;

namespace GeneticAlgorithm
{
    /// <summary>
    /// A chromosome.
    /// </summary>
    /// <typeparam name="TGene">The type of gene.</typeparam>
    public class Chromosome<TGene> : IComparable<Chromosome<TGene>>
    {
        private readonly GeneticAlgorithm<TGene> args;

        /// <summary>
        /// Creates a new chromosome.
        /// </summary>
        /// <param name="chromosomeSize">The size of the chromosome (i.e. the numebr of genes in the chromosome).</param>
        public Chromosome(GeneticAlgorithm<TGene> args)
        {
            this.args = args;
            Genes = args.InitializationFunction(args);
        }

        private Chromosome(GeneticAlgorithm<TGene> args, TGene[] genes)
        {
            this.args = args;
            Genes = genes;
        }

        /// <summary>
        /// Gets or sets the genes.
        /// </summary>
        /// <value>
        /// The genes.
        /// </value>
        public TGene[] Genes { get; set; }

        public TGene this[int index]
        {
            get { return Genes[index]; }
            set { Genes[index] = value; }
        }

        /// <summary>
        /// Gets the size of the chromosome (i.e. the number of genes in the chromosome).
        /// </summary>
        /// <value>
        /// The size of the chromosome.
        /// </value>
        public int Size => Genes.Length;

        /// <summary>
        /// Gets or sets the evaluation.
        /// </summary>
        /// <value>
        /// The evaluation.
        /// </value>
        public double Evaluation { get; set; }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>
        /// The fitness.
        /// </value>
        public double Fitness { get; set; }

        public double Evaluate() => Evaluation = args.ObjectiveFunction.Evaluate(Genes);

        public double EvaluateFitness(double averageEvaluation) => Fitness = args.FitnessFunction(Evaluation, averageEvaluation, args.ObjectiveFunction.Objective);

        public IEnumerable<Chromosome<TGene>> Mate(Chromosome<TGene> partner)
        {
            Chromosome<TGene> offspring1 = Clone();
            Chromosome<TGene> offspring2 = partner.Clone();

            // Cross-over
            if (Random.Double() < args.CrossoverRate)
            {
                args.CrossoverOperator(offspring1.Genes, offspring2.Genes, args);
            }

            // Mutate
            if (Random.Double() < args.MutationRate)
            {
                args.MutationOperator(offspring1.Genes, args);
            }
            if (Random.Double() < args.MutationRate)
            {
                args.MutationOperator(offspring2.Genes, args);
            }

            yield return offspring1;
            yield return offspring2;
        }

        /// <summary>
        /// Clones the chromosome.
        /// </summary>
        /// <returns>
        /// A clone of the chromosome.
        /// </returns>
        public Chromosome<TGene > Clone()
        {
            Chromosome<TGene> clone = new Chromosome<TGene>(args, new TGene[Size]);
            Array.Copy(Genes, clone.Genes, Size);
            return clone;
        }

        /// <summary>
        /// Compares the current chromosome with another chromosome.
        /// </summary>
        /// <param name="otherChromosome">A chromosome to compare with this chromosome.</param>
        /// <returns>
        /// Less than zero if this chromosome is less than the <c>otherChromosome</c> (this chromosome is less fit than the <c>otherChromosome</c>),
        /// zero if this chromosome equals the <c>otherChromosome</c> (this chromosome and the <c>otherChromosome</c> are equally fit), and
        /// greater than zero if this chromosome is more than the <c>otherChromosome</c> (this chromosome is fitter than the <c>otherChromosome</c>).
        /// </returns>
        public int CompareTo(Chromosome<TGene > otherChromosome)
            => Evaluation.CompareTo(otherChromosome.Evaluation);

        /// <summary>
        /// Converts a chromosome into its string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the chromosome.
        /// </returns>
        public override string ToString()
            => $"Genes: {Print(Genes)}, Evaluation: {Evaluation}, Fitness: {Fitness}";

        public static string Print(TGene[] genes) => $"[{String.Join(", ", genes)}]";
    }
}

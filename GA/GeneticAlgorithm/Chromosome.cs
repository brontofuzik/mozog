using System;
using System.Text;

namespace GeneticAlgorithm
{
    /// <summary>
    /// A chromosome.
    /// </summary>
    /// <typeparam name="TGene">The type of gene.</typeparam>
    public class Chromosome< TGene >
        : IComparable< Chromosome< TGene > >
    {
        /// <summary>
        /// The genes of the chromosome.
        /// </summary>
        private TGene[] genes;

        /// <summary>
        /// The evaluation of the chromosome (the value of objective function for the chromoosme).
        /// </summary>
        private double evaluation;

        /// <summary>
        /// The fitness of the chromosome (the value of fitness function for the chromosome).
        /// </summary>
        private double fitness;

        /// <summary>
        /// Creates a new chromosome.
        /// </summary>
        /// <param name="chromosomeSize">The size of the chromosome (i.e. the numebr of genes in the chromosome).</param>
        public Chromosome( int chromosomeSize )
        {
            genes = new TGene[ chromosomeSize ];
        }

        /// <summary>
        /// Gets or sets the genes.
        /// </summary>
        /// <value>
        /// The genes.
        /// </value>
        public TGene[] Genes
        {
            get
            {
                return genes;
            }
            set
            {
                genes = value;
            }
        }

        /// <summary>
        /// Gets the size of the chromosome (i.e. the number of genes in the chromosome).
        /// </summary>
        /// <value>
        /// The size of the chromosome.
        /// </value>
        public int Size
        {
            get
            {
                return genes.Length;
            }
        }

        /// <summary>
        /// Gets or sets the evaluation.
        /// </summary>
        /// <value>
        /// The evaluation.
        /// </value>
        public double Evaluation
        {
            get
            {
                return evaluation;
            }
            set
            {
                evaluation = value;
            }
        }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>
        /// The fitness.
        /// </value>
        public double Fitness
        {
            get
            {
                return fitness;
            }
            set
            {
                fitness = value;
            }
        }

        /// <summary>
        /// Clones the chromosome.
        /// </summary>
        /// <returns>
        /// A clone of the chromosome.
        /// </returns>
        public Chromosome< TGene > Clone()
        {
            Chromosome< TGene > clone = new Chromosome< TGene >( Size );
            Array.Copy( genes, clone.genes, Size );
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
        public int CompareTo( Chromosome< TGene > otherChromosome )
        {
            return evaluation.CompareTo( otherChromosome.Evaluation );
        }

        /// <summary>
        /// Converts a chromosome into its string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the chromosome.
        /// </returns>
        public override string ToString()
        {
            StringBuilder chromosomeSB = new StringBuilder();

            chromosomeSB.Append( "Genes: [" );
            foreach (TGene gene in genes)
            {
                chromosomeSB.Append( gene );
                chromosomeSB.Append( ", " );
            }
            chromosomeSB.Remove( chromosomeSB.Length - 2, 2 );
            chromosomeSB.Append( "]; Evaluation: " + evaluation + "; Fitness: " + fitness );

            return chromosomeSB.ToString();
        }
    }
}

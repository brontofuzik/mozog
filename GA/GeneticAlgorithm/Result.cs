namespace GeneticAlgorithm
{
    public class Result<TGene>
    {
        public TGene[] Solution { get; set; }

        public double Evaluation { get; set; }

        public int Generations { get; set; }
    }
}

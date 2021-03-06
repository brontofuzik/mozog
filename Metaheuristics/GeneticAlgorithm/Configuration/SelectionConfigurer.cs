﻿using GeneticAlgorithm.Functions.Selection;

namespace GeneticAlgorithm.Configuration
{
    public class SelectionConfigurer<TGene> : FunctionConfigurer<TGene>
    {
        public SelectionConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer)
            : base(algo, algoConfigurer)
        {
        }

        public AlgorithmConfigurer<TGene> RouletteWheel()
            => SetSelection(new RouletteWheelSelection<TGene>());

        public AlgorithmConfigurer<TGene> RankBased()
            => SetSelection(new RankBasedSelection<TGene>());

        private AlgorithmConfigurer<TGene> SetSelection(ISelectionFunction<TGene> selection)
        {
            Algo.Selector = selection;
            return AlgoConfigurer;
        }
    }
}

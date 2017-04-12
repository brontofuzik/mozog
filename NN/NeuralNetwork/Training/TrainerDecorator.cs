using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public abstract class TrainerDecorator<TTrainingArgs> : TrainerBase<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        protected readonly ITrainer<TTrainingArgs> innerTrainer;

        protected TrainerDecorator(ITrainer<TTrainingArgs> innerTrainer)
        {
            this.innerTrainer = innerTrainer;
        }

        public override event EventHandler<TrainingStatus> WeightsUpdated
        {
            add { innerTrainer.WeightsUpdated += value; }
            remove { innerTrainer.WeightsUpdated -= value; }
        }

        public override event EventHandler WeightsReset
        {
            add { innerTrainer.WeightsReset += value; }
            remove { innerTrainer.WeightsReset -= value; }
        }
    }
}

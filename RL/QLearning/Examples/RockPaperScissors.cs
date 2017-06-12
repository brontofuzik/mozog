using System;
using System.Collections.Generic;
using System.Diagnostics;
using QLearning.Lib;
using Action = QLearning.Lib.Action;

namespace QLearning.Examples
{
    class RockPaperScissors
    {
        internal enum State
        {
            Begin,
            Rock,
            Paper,
            Scissors
        }
 
        static void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            RunInternal();

            stopwatch.Stop();

            Console.WriteLine($"\n{stopwatch.Elapsed.TotalSeconds.Pretty()} sec. press a key ...");
            Console.ReadKey();
        }
 
        static void RunInternal()
        {
            var opponents = new List<double[]>
            {
                new[] {0.33, 0.33, 0.33},
                new[] {0.5, 0.3, 0.2},
                new[] {0.2, 0.5, 0.3},
                new[] {0.1, 0.1, 0.8}
            };

            var opponent = opponents[new Random().Next(opponents.Count)];

            double rockProb = opponent[0];
            double paperProb = opponent[1];
            double scissorsProb = opponent[2];

            Console.WriteLine("\n** Opponent style **");
            Console.WriteLine($"style is rock {rockProb.Pretty()} paper {paperProb.Pretty()} scissor {scissorsProb.Pretty()}");

            var q = new Lib.QLearning
            {
                Episodes = 1000,
                Alpha = 0.1,
                Gamma = 0.9,
                MaxExploreStepsWithinOneEpisode = 1000
            };

            //
            // Setup
            //

            // States
            var beginState = new Lib.State(State.Begin);
            q.AddState(beginState);
            q.AddState(new Lib.State(State.Rock), isEndState: true);
            q.AddState(new Lib.State(State.Paper), isEndState: true);
            q.AddState(new Lib.State(State.Scissors), isEndState: true);

            // "Rock" action
            beginState.AddAction(new Action(State.Rock))
                .AddResult(State.Rock, rockProb, 0)
                .AddResult(State.Paper, paperProb, -10)
                .AddResult(State.Scissors, scissorsProb, 100);

            // "Paper" action
            beginState.AddAction(new Action( State.Paper))
                .AddResult(State.Rock, rockProb, 100)
                .AddResult(State.Paper, paperProb, 0)
                .AddResult(State.Scissors, scissorsProb, -10);

            // "Scissors" action
            beginState.AddAction(new Action(State.Scissors))
                .AddResult(State.Rock, rockProb, -10)
                .AddResult(State.Paper, paperProb, 100)
                .AddResult(State.Scissors, scissorsProb, 0);

            //
            // Learning
            //

            q.RunTraining();
            q.PrintQLearningStructure();
            q.ShowPolicy();
         }        
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QLearning.Examples
{
    class RockPaperScissors
    {
        internal enum S
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

            var q = new QLearning
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
            var beginState = new State(S.Begin);
            q.AddState(beginState);
            q.AddState(new State(S.Rock), isEndState: true);
            q.AddState(new State(S.Paper), isEndState: true);
            q.AddState(new State(S.Scissors), isEndState: true);

            // "Rock" action
            beginState.AddAction(new Action(S.Rock))
                .AddResult(S.Rock, rockProb, 0)
                .AddResult(S.Paper, paperProb, -10)
                .AddResult(S.Scissors, scissorsProb, 100);

            // "Paper" action
            beginState.AddAction(new Action(S.Paper))
                .AddResult(S.Rock, rockProb, 100)
                .AddResult(S.Paper, paperProb, 0)
                .AddResult(S.Scissors, scissorsProb, -10);

            // "Scissors" action
            beginState.AddAction(new Action(S.Scissors))
                .AddResult(S.Rock, rockProb, -10)
                .AddResult(S.Paper, paperProb, 100)
                .AddResult(S.Scissors, scissorsProb, 0);

            //
            // Learning
            //

            q.RunTraining();
            q.PrintQLearningStructure();
            q.ShowPolicy();
         }        
    }
}
using Mozog.Search.Examples.Games.Hexapawn;
using Mozog.Search.Examples.Games.TicTacToe;
using Mozog.Utils.Math;

namespace Mozog.Search.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            StaticRandom.Seed = 42;

            //PlayTicTacToe();
            PlayHexapawn();
        }

        private static void PlayTicTacToe()
        {
            TicTacToe.Play_Minimax(prune: false);
            TicTacToe.Play_Minimax(prune: true);
        }

        private static void PlayHexapawn()
        {
            // 3 cols
            //Hexapawn.Play_Minimax(cols: 3, rows: 3); // Black wins
            //Hexapawn.Play_Minimax(cols: 3, rows: 4); // Black wins

            // 4 cols
            //Hexapawn.Play_Minimax(cols: 4, rows: 4); // White wins
            //Hexapawn.Play_Minimax(cols: 4, rows: 5); // Black wins
            Hexapawn.Play_Minimax(cols: 4, rows: 5); // Black wins

            // 5 cols
            //Hexapawn.Play_Minimax(cols: 5, rows: 5); // White wins
        }
    }
}

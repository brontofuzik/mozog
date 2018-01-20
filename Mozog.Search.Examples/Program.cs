namespace Mozog.Search.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Adversarial.TicTacToe.Play_Minimax();
            //Adversarial.TicTacToe.Play_AlphaBeta();
            Adversarial.Hexapawn.Play_Minimax();
        }
    }
}

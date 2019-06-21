using System;

namespace Tetris.Model.EventArg
{
    public class GameOverArgs : EventArgs
    {
        public int Score { get; private set; }

        public GameOverArgs(int score) { Score = score; }
    }
}

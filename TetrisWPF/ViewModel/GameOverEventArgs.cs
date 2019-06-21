using System;

namespace Tetris.ViewModel
{
    public class GameOverEventArgs : EventArgs
    {
        public int Score { get; set; }
    }
}

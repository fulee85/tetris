using System;

namespace Tetris.Model.EventArg
{
    public class ScoreChangedArgs : EventArgs
    {
        public int GameScore { get; private set; }

        public ScoreChangedArgs(int score) { GameScore = score; }
    }
}

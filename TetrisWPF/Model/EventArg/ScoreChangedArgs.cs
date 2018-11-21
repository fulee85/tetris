using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model.EventArg
{
    public class ScoreChangedArgs : EventArgs
    {
        public int GameScore { get; private set; }

        public ScoreChangedArgs(int score) { GameScore = score; }
    }
}

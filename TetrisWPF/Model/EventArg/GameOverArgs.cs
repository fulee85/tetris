using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model.EventArg
{
    public class GameOverArgs: EventArgs    
    {
        public int GameTime { get; private set; }

        public GameOverArgs(int time) { GameTime = time; }
    }
}

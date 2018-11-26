using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;

namespace Tetris.Model.EventArg
{
    public class TetrisFieldChangedAgrs : EventArgs
    {
        private Coordinate position;
        private readonly FieldStatus fieldStatus;

        public int XPosition { get { return position.x; } }
        public int YPosition { get { return position.y; } }
        public bool IsFieldEmpty { get { return FieldStatus.FREE == fieldStatus; } }

        public TetrisFieldChangedAgrs(Coordinate position, FieldStatus newFieldStatus)
        {
            this.position = position;
            this.fieldStatus = newFieldStatus;
        }
    }
}

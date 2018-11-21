﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;

namespace Tetris.Model.EventArg
{
    public class TetrisFieldChangedAgrs : EventArgs
    {
        private Coordinates position;
        private FieldStatus fieldStatus;

        public int XPosition { get { return position.x; } }
        public int YPosition { get { return position.y; } }
        public bool IsEmptyField { get { return FieldStatus.FREE == fieldStatus; } }

        public TetrisFieldChangedAgrs(Coordinates position, FieldStatus newFieldStatus)
        {
            this.position = position;
            this.fieldStatus = newFieldStatus;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model.Structs
{
    public enum Directions { LEFT, RIGHT, DOWN}
    public enum FieldStatus { FREE, NOT_FREE }
    public enum ShapeTypes { CUBE, STRAIGHT, RIGHT_L, LEFT_L, T_SHAPE, Z_SHAPE, S_SHAPE}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class ZShape : Shape
    {
        public ZShape(int playFieldHorizontalSize) : base(playFieldHorizontalSize)
        {
            center = new Coordinates(playFieldHorizontalSize / 2, 1);
            shapeCoordinates[0] = new Coordinates(center.x - 1, center.y);
            shapeCoordinates[1] = center;
            shapeCoordinates[2] = new Coordinates(center.x, center.y + 1);
            shapeCoordinates[3] = new Coordinates(center.x + 1, center.y + 1);
        }
    }
}

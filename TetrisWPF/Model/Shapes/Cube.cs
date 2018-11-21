using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class Cube : Shape
    {
        public Cube(int playFieldHorizontalSize) : base(playFieldHorizontalSize)
        {

            center = new Coordinates(playFieldHorizontalSize / 2 -1, 0);

            shapePosition[0] = center;
            shapePosition[1] = new Coordinates(center.x + 1, center.y);
            shapePosition[2] = new Coordinates(center.x + 1, center.y + 1);
            shapePosition[3] = new Coordinates(center.x, center.y + 1);
        }

        public override Coordinates[] getShapePositionAfterRotation()
        {
            return shapePosition;
        }
    }
}

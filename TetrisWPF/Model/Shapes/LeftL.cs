using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class LeftL : Shape
    {
        public LeftL(int tableCenter) : base(tableCenter)
        {
            center = new Coordinate(tableCenter, 1);

            shapeCoordinates[0] = new Coordinate(center.x, center.y - 1);
            shapeCoordinates[1] = center;
            shapeCoordinates[2] = new Coordinate(center.x, center.y + 1);
            shapeCoordinates[3] = new Coordinate(center.x - 1, center.y + 1);
        }
    }
}

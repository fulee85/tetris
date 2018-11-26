using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class Cube : Shape
    {
        public Cube(int tableCenter) : base(tableCenter)
        {
            center = new Coordinate(tableCenter, 0);

            shapeCoordinates[0] = center;
            shapeCoordinates[1] = new Coordinate(center.x - 1, center.y);
            shapeCoordinates[2] = new Coordinate(center.x - 1, center.y + 1);
            shapeCoordinates[3] = new Coordinate(center.x, center.y + 1);
        }

        public override Coordinate[] GetShapePositionAfterRotation()
        {
            return shapeCoordinates;
        }
    }
}

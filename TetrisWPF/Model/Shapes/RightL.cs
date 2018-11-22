using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class RightL : Shape
    {
        public RightL(int tableCenter) : base(tableCenter)
        {
            center = new Coordinates(tableCenter, 1);

            shapeCoordinates[0] = new Coordinates(center.x, center.y - 1);
            shapeCoordinates[1] = center;
            shapeCoordinates[2] = new Coordinates(center.x, center.y + 1);
            shapeCoordinates[3] = new Coordinates(center.x + 1, center.y + 1);
        }
    }
}

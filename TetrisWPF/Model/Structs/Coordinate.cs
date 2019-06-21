namespace Tetris.Model.Structs
{
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Step(Directions direction)
        {
            switch (direction)
            {
                case Directions.LEFT:
                    x--;
                    break;
                case Directions.RIGHT:
                    x++;
                    break;
                case Directions.DOWN:
                    y++;
                    break;
            }
        }

        /// <summary>
        /// Rotates a point 90° Clockwise around the center
        /// </summary>
        public static Coordinate Rotate90Degrees(Coordinate center, Coordinate pointToRotate)
        {
            Coordinate directionVector = pointToRotate - center;
            int helper = directionVector.x;
            directionVector.x = -directionVector.y;
            directionVector.y = helper;
            return directionVector + center;
        }

        public static Coordinate operator +(Coordinate i, Coordinate j)
        {
            Coordinate k;
            k.x = i.x + j.x;
            k.y = i.y + j.y;
            return k;
        }

        public static Coordinate operator -(Coordinate i, Coordinate j)
        {
            Coordinate k;
            k.x = i.x - j.x;
            k.y = i.y - j.y;
            return k;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;

namespace Tetris.Model.Structs
{
    public struct Coordinates
    {
        public int x;
        public int y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void step(Directions direction)
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

        public static Coordinates rotate90Degrees(Coordinates center, Coordinates pointToRotate)
        {
            Coordinates directionVector = pointToRotate - center;
            int helper = directionVector.x;
            directionVector.x = -directionVector.y;
            directionVector.y = helper;
            return directionVector + center;
        }

        public static Coordinates operator+( Coordinates i, Coordinates j)
        {
            Coordinates k;
            k.x = i.x + j.x;
            k.y = i.y + j.y;
            return k;
        }

        public static Coordinates operator -(Coordinates i, Coordinates j)
        {
            Coordinates k;
            k.x = i.x - j.x;
            k.y = i.y - j.y;
            return k;
        }
    }

}

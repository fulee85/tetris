using System;
using Tetris.Model.Structs;


namespace Tetris.Model.Shapes
{

    abstract public class Shape
    {
        protected Coordinate center;
        protected Coordinate[] shapeCoordinates;
        protected readonly static Random randomGenerator = new Random();

        protected Shape(int tableCenter)
        {
            if (tableCenter < 2)
            {
                throw new Exception("Too small playfield!");
            }
            shapeCoordinates = new Coordinate[4];
        }

        public Coordinate[] PartsCoordinates
        {
            get { return shapeCoordinates; }
            set { value.CopyTo(shapeCoordinates, 0); center = shapeCoordinates[1]; }
        }

        virtual public Coordinate[] GetShapePositionAfterRotation()
        {
            Coordinate[] positionAfterMove = new Coordinate[shapeCoordinates.Length];
            shapeCoordinates.CopyTo(positionAfterMove, 0);
            for (int i = 0; i < positionAfterMove.Length; i++)
            {
                positionAfterMove[i] = Coordinate.Rotate90Degrees(center, positionAfterMove[i]);
            }
            return positionAfterMove;
        }

        virtual public Coordinate[] GetShapePositionAfterMove(Directions direction)
        {
            Coordinate[] positionAfterMove = new Coordinate[shapeCoordinates.Length];
            shapeCoordinates.CopyTo(positionAfterMove, 0);
            for (int i = 0; i < shapeCoordinates.Length; i++)
            {
                positionAfterMove[i].Step(direction);
            }
            return positionAfterMove;
        }
    }
}

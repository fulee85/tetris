using System;
using Tetris.Model.Structs;


namespace Tetris.Model.Shapes
{

    abstract public class Shape
    {
        protected Coordinates center;
        protected Coordinates[] shapeCoordinates;
        protected static Random randomGenerator = new Random();

        protected Shape(int tableCenter)
        {
            if (tableCenter < 2)
            {
                throw new Exception("Too small playfield!");
            }
            shapeCoordinates = new Coordinates[4];
        }

        public Coordinates[] PartsCoordinates
        {
            get { return shapeCoordinates; }
            set { value.CopyTo(shapeCoordinates, 0); center = shapeCoordinates[1]; }
        }

        virtual public Coordinates[] GetShapePositionAfterRotation()
        {
            Coordinates[] positionAfterMove = new Coordinates[shapeCoordinates.Length];
            shapeCoordinates.CopyTo(positionAfterMove, 0);
            for (int i = 0; i < positionAfterMove.Length; i++)
            {
                positionAfterMove[i] = Coordinates.Rotate90Degrees(center, positionAfterMove[i]);
            }
            return positionAfterMove;
        }

        virtual public Coordinates[] GetShapePositionAfterMove(Directions direction)
        {
            Coordinates[] positionAfterMove = new Coordinates[shapeCoordinates.Length];
            shapeCoordinates.CopyTo(positionAfterMove, 0);
            for (int i = 0; i < shapeCoordinates.Length; i++)
            {
                positionAfterMove[i].Step(direction);
            }
            return positionAfterMove;
        }
    }
}

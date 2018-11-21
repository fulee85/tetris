using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;


namespace Tetris.Model.Shapes
{

    abstract public class Shape
    {
        protected Coordinates center;
        protected Coordinates[] shapePosition;
        protected static Random randomGenerator = new Random();

        protected Shape(int playFieldHorizontalSize)
        { 
            if (playFieldHorizontalSize < 4)
            {
                throw new Exception("Too small playfield!");
            }
            shapePosition = new Coordinates[4];
        }

        public Coordinates[] PartsCoordinates {
            get { return shapePosition; }
            set { value.CopyTo(shapePosition, 0); center = shapePosition[1]; }
        }

        virtual public Coordinates[] getShapePositionAfterRotation()
        {
            Coordinates[] positionAfterMove = new Coordinates[shapePosition.Length];
            shapePosition.CopyTo(positionAfterMove, 0);
            for (int i = 0; i < positionAfterMove.Length; i++)
            {
                positionAfterMove[i] = Coordinates.rotate90Degrees(center, positionAfterMove[i]); 
            }
            return positionAfterMove;
        }

        virtual public Coordinates[] getShapePositionAfterMove(Directions direction)
        {
            Coordinates[] positionAfterMove = new Coordinates[shapePosition.Length];
            shapePosition.CopyTo(positionAfterMove, 0);
            for (int i = 0; i < shapePosition.Length; i++)
            {
                positionAfterMove[i].step(direction);
            }
            return positionAfterMove;
        }
    }
}

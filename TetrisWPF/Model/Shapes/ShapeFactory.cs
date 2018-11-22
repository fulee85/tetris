using System;
using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class ShapeFactory : IShapeFactory
    {
        private Random randomGenerator;
        private readonly int shapesNumber = 7;

        public ShapeFactory()
        {
            randomGenerator = new Random();
        }

        public Shape GetNewShape(int tableCenter)
        {
            ShapeTypes shapeType = (ShapeTypes)randomGenerator.Next(shapesNumber);
            int initRotationNum = randomGenerator.Next(4);
            Shape returnShape;
            switch (shapeType)
            {
                case ShapeTypes.CUBE:
                    returnShape = new Cube(tableCenter);
                    initRotationNum = 0;
                    break;
                case ShapeTypes.STRAIGHT:
                    returnShape = new Straight(tableCenter);
                    initRotationNum %= 2;
                    break;
                case ShapeTypes.RIGHT_L:
                    returnShape = new RightL(tableCenter);
                    break;
                case ShapeTypes.LEFT_L:
                    returnShape = new LeftL(tableCenter);
                    break;
                case ShapeTypes.T_SHAPE:
                    returnShape = new TShape(tableCenter);
                    break;
                case ShapeTypes.Z_SHAPE:
                    returnShape = new ZShape(tableCenter);
                    initRotationNum %= 2;
                    break;
                case ShapeTypes.S_SHAPE:
                    returnShape = new SShape(tableCenter);
                    initRotationNum %= 2;
                    break;
                default:
                    throw new Exception("Unknown Shape");
            }
            for (int i = 0; i < initRotationNum; i++)
            {
                returnShape.PartsCoordinates = returnShape.GetShapePositionAfterRotation();
            }
            return returnShape;
        }
    }
}

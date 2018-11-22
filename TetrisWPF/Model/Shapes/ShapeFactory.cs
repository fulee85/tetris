using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Shape GetNewShape(int playFieldHorizontalSize)
        {
            ShapeTypes shapeType = (ShapeTypes)randomGenerator.Next(shapesNumber);
            int initRotationNum = randomGenerator.Next(4);
            Shape returnShape;
            switch (shapeType)
            {
                case ShapeTypes.CUBE:
                    returnShape = new Cube(playFieldHorizontalSize);
                    initRotationNum = 0;
                    break;
                case ShapeTypes.STRAIGHT:
                    returnShape = new Straight(playFieldHorizontalSize);
                    initRotationNum %= 2;
                    break;
                case ShapeTypes.RIGHT_L:
                    returnShape = new RightL(playFieldHorizontalSize);
                    break;
                case ShapeTypes.LEFT_L:
                    returnShape = new LeftL(playFieldHorizontalSize);
                    break;
                case ShapeTypes.T_SHAPE:
                    returnShape = new TShape(playFieldHorizontalSize);
                    break;
                case ShapeTypes.Z_SHAPE:
                    returnShape = new ZShape(playFieldHorizontalSize);
                    initRotationNum %= 2;
                    break;
                case ShapeTypes.S_SHAPE:
                    returnShape = new SShape(playFieldHorizontalSize);
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

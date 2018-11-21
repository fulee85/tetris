using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Model.Shapes;
using Tetris.Model.Structs;

namespace TetrisWPFTest
{
    /// <summary>
    /// Summary description for TestShape
    /// </summary>
    [TestClass]
    public class TestShape
    {
        [TestMethod]
        public void Test_getShapePositionAfterRotation()
        {
            Shape testShape = new RightL(16);
            Coordinates[] startCoordinates = new Coordinates[4];
            startCoordinates[0] = new Coordinates(3, 3);
            startCoordinates[1] = new Coordinates(3, 4);
            startCoordinates[2] = new Coordinates(3, 5);
            startCoordinates[3] = new Coordinates(4, 5);
            testShape.PartsCoordinates = startCoordinates;

            Coordinates[] expectedCoordinatesAfterRotation = new Coordinates[4];
            expectedCoordinatesAfterRotation[0] = new Coordinates(4, 4);
            expectedCoordinatesAfterRotation[1] = new Coordinates(3, 4);
            expectedCoordinatesAfterRotation[2] = new Coordinates(2, 4);
            expectedCoordinatesAfterRotation[3] = new Coordinates(2, 5);

            Coordinates[] coordinatesAfterRotation = testShape.getShapePositionAfterRotation();

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterRotation[i], coordinatesAfterRotation[i]);
            }
        }

        [TestMethod]
        public void Test_getShapePositionAfterMoveDown()
        {
            Shape testShape = new SShape(16);
            Coordinates[] startCoordinates = new Coordinates[4];
            startCoordinates[0] = new Coordinates(3, 3);
            startCoordinates[1] = new Coordinates(3, 4);
            startCoordinates[2] = new Coordinates(4, 4);
            startCoordinates[3] = new Coordinates(4, 5);
            testShape.PartsCoordinates = startCoordinates;

            Coordinates[] expectedCoordinatesAfterMoveDown = new Coordinates[4];
            expectedCoordinatesAfterMoveDown[0] = new Coordinates(3, 4);
            expectedCoordinatesAfterMoveDown[1] = new Coordinates(3, 5);
            expectedCoordinatesAfterMoveDown[2] = new Coordinates(4, 5);
            expectedCoordinatesAfterMoveDown[3] = new Coordinates(4, 6);

            Coordinates[] coordinatesAfterMoveDown = testShape.getShapePositionAfterMove(Directions.DOWN);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterMoveDown[i], coordinatesAfterMoveDown[i]);
            }
        }

        [TestMethod]
        public void Test_getShapePositionAfterMoveLeft()
        {
            Shape testShape = new ZShape(16);
            Coordinates[] startCoordinates = new Coordinates[4];
            startCoordinates[0] = new Coordinates(3, 3);
            startCoordinates[1] = new Coordinates(3, 4);
            startCoordinates[2] = new Coordinates(2, 4);
            startCoordinates[3] = new Coordinates(2, 5);
            testShape.PartsCoordinates = startCoordinates;

            Coordinates[] expectedCoordinatesAfterMoveLeft = new Coordinates[4];
            expectedCoordinatesAfterMoveLeft[0] = new Coordinates(2, 3);
            expectedCoordinatesAfterMoveLeft[1] = new Coordinates(2, 4);
            expectedCoordinatesAfterMoveLeft[2] = new Coordinates(1, 4);
            expectedCoordinatesAfterMoveLeft[3] = new Coordinates(1, 5);

            Coordinates[] coordinatesAfterMoveLeft = testShape.getShapePositionAfterMove(Directions.LEFT);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterMoveLeft[i], coordinatesAfterMoveLeft[i]);
            }
        }

        [TestMethod]
        public void Test_getShapePositionAfterMoveRight()
        {
            Shape testShape = new Straight(16);
            Coordinates[] startCoordinates = new Coordinates[4];
            startCoordinates[0] = new Coordinates(3, 3);
            startCoordinates[1] = new Coordinates(3, 4);
            startCoordinates[2] = new Coordinates(3, 5);
            startCoordinates[3] = new Coordinates(3, 6);
            testShape.PartsCoordinates = startCoordinates;

            Coordinates[] expectedCoordinatesAfterMoveRight = new Coordinates[4];
            expectedCoordinatesAfterMoveRight[0] = new Coordinates(4, 3);
            expectedCoordinatesAfterMoveRight[1] = new Coordinates(4, 4);
            expectedCoordinatesAfterMoveRight[2] = new Coordinates(4, 5);
            expectedCoordinatesAfterMoveRight[3] = new Coordinates(4, 6);

            Coordinates[] coordinatesAfterMoveRight = testShape.getShapePositionAfterMove(Directions.RIGHT);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterMoveRight[i], coordinatesAfterMoveRight[i]);
            }
        }
    }
}

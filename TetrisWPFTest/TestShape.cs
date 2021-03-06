﻿using System;
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
            Coordinate[] startCoordinates = new Coordinate[4];
            startCoordinates[0] = new Coordinate(3, 3);
            startCoordinates[1] = new Coordinate(3, 4);
            startCoordinates[2] = new Coordinate(3, 5);
            startCoordinates[3] = new Coordinate(4, 5);
            testShape.PartsCoordinates = startCoordinates;

            Coordinate[] expectedCoordinatesAfterRotation = new Coordinate[4];
            expectedCoordinatesAfterRotation[0] = new Coordinate(4, 4);
            expectedCoordinatesAfterRotation[1] = new Coordinate(3, 4);
            expectedCoordinatesAfterRotation[2] = new Coordinate(2, 4);
            expectedCoordinatesAfterRotation[3] = new Coordinate(2, 5);

            Coordinate[] coordinatesAfterRotation = testShape.GetShapePositionAfterRotation();

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterRotation[i], coordinatesAfterRotation[i]);
            }
        }

        [TestMethod]
        public void Test_getShapePositionAfterMoveDown()
        {
            Shape testShape = new SShape(16);
            Coordinate[] startCoordinates = new Coordinate[4];
            startCoordinates[0] = new Coordinate(3, 3);
            startCoordinates[1] = new Coordinate(3, 4);
            startCoordinates[2] = new Coordinate(4, 4);
            startCoordinates[3] = new Coordinate(4, 5);
            testShape.PartsCoordinates = startCoordinates;

            Coordinate[] expectedCoordinatesAfterMoveDown = new Coordinate[4];
            expectedCoordinatesAfterMoveDown[0] = new Coordinate(3, 4);
            expectedCoordinatesAfterMoveDown[1] = new Coordinate(3, 5);
            expectedCoordinatesAfterMoveDown[2] = new Coordinate(4, 5);
            expectedCoordinatesAfterMoveDown[3] = new Coordinate(4, 6);

            Coordinate[] coordinatesAfterMoveDown = testShape.GetShapePositionAfterMove(Directions.DOWN);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterMoveDown[i], coordinatesAfterMoveDown[i]);
            }
        }

        [TestMethod]
        public void Test_getShapePositionAfterMoveLeft()
        {
            Shape testShape = new ZShape(16);
            Coordinate[] startCoordinates = new Coordinate[4];
            startCoordinates[0] = new Coordinate(3, 3);
            startCoordinates[1] = new Coordinate(3, 4);
            startCoordinates[2] = new Coordinate(2, 4);
            startCoordinates[3] = new Coordinate(2, 5);
            testShape.PartsCoordinates = startCoordinates;

            Coordinate[] expectedCoordinatesAfterMoveLeft = new Coordinate[4];
            expectedCoordinatesAfterMoveLeft[0] = new Coordinate(2, 3);
            expectedCoordinatesAfterMoveLeft[1] = new Coordinate(2, 4);
            expectedCoordinatesAfterMoveLeft[2] = new Coordinate(1, 4);
            expectedCoordinatesAfterMoveLeft[3] = new Coordinate(1, 5);

            Coordinate[] coordinatesAfterMoveLeft = testShape.GetShapePositionAfterMove(Directions.LEFT);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterMoveLeft[i], coordinatesAfterMoveLeft[i]);
            }
        }

        [TestMethod]
        public void Test_getShapePositionAfterMoveRight()
        {
            Shape testShape = new Straight(16);
            Coordinate[] startCoordinates = new Coordinate[4];
            startCoordinates[0] = new Coordinate(3, 3);
            startCoordinates[1] = new Coordinate(3, 4);
            startCoordinates[2] = new Coordinate(3, 5);
            startCoordinates[3] = new Coordinate(3, 6);
            testShape.PartsCoordinates = startCoordinates;

            Coordinate[] expectedCoordinatesAfterMoveRight = new Coordinate[4];
            expectedCoordinatesAfterMoveRight[0] = new Coordinate(4, 3);
            expectedCoordinatesAfterMoveRight[1] = new Coordinate(4, 4);
            expectedCoordinatesAfterMoveRight[2] = new Coordinate(4, 5);
            expectedCoordinatesAfterMoveRight[3] = new Coordinate(4, 6);

            Coordinate[] coordinatesAfterMoveRight = testShape.GetShapePositionAfterMove(Directions.RIGHT);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedCoordinatesAfterMoveRight[i], coordinatesAfterMoveRight[i]);
            }
        }
    }
}

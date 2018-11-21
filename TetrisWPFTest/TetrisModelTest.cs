using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tetris.Model;
using Tetris.Model.Structs;
using Tetris.Model.Shapes;
using System.Timers;
using System.Linq;

namespace TetrisWPFTest
{
    [TestClass]
    public class TetrisModelTest
    {

        [TestMethod]
        public void Test_TetrisModel_startNewGame()
        {

            TetrisModel model = new TetrisModel();

            int expectedXSize = 8;
            int expectedYSize = 16;
            model.startNewGame(expectedXSize, expectedYSize);

            Assert.AreEqual(expectedXSize, model.XSize);
            Assert.AreEqual(expectedYSize, model.YSize);
            Assert.AreEqual(expectedXSize, model.GameTable[0].Length);
            Assert.AreEqual(expectedYSize, model.GameTable.Length);

            foreach (var row in model.GameTable)
            {
                foreach (var element in row)
                {
                    Assert.AreEqual(FieldStatus.FREE, element);
                }
            }
        }

        [TestMethod]
        public void Test_TetrisModel_StepGame_GameTime()
        {
            int expectedXSize = 8;
            int expectedYSize = 16;                  
            TetrisModel model = new TetrisModel();
            model.startNewGame(expectedXSize, expectedYSize);

            model.stepGame();
            Assert.AreEqual(1, model.GameTime);
            model.stepGame();
            Assert.AreEqual(2, model.GameTime);
        }

        [TestMethod]
        public void Test_TetrisModel_StepGame_CheckShapeMove()
        {
            int expectedXSize = 4;
            int expectedYSize = 16;
            var shapeFactory = new Mock<IShapeFactory>();
            shapeFactory.Setup(sf => sf.getNewShape(expectedXSize)).Returns(new LeftL(expectedXSize));
            TetrisModel model = new TetrisModel(shapeFactory.Object);
            model.startNewGame(expectedXSize, expectedYSize);

            Shape expectedShape = new LeftL(expectedXSize);
            var partsCoordinates = expectedShape.PartsCoordinates;
            foreach (var part in partsCoordinates)
            {
                Assert.IsTrue(model.ActualShapePosition.Contains(part),"Before move is not OK");
            }

            model.stepGame();
            for (int i = 0; i < partsCoordinates.Length; i++)
            {
                partsCoordinates[i].y++;
            }

            foreach (var part in partsCoordinates)
            {
                Assert.IsTrue(model.ActualShapePosition.Contains(part),"After move is not OK");
            }
        }
    }
}

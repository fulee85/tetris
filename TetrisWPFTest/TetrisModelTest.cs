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
        public void Test_TetrisModel_StepGame_CheckShapeMove()
        {
            int expectedXSize = 4;
            int expectedYSize = 16;
            var shapeFactory = new Mock<IShapeFactory>();
            shapeFactory.Setup(sf => sf.GetNewShape(expectedXSize / 2)).Returns(new LeftL(expectedXSize / 2));
            TetrisModel model = new TetrisModel(shapeFactory.Object);
            model.StartNewGame(expectedXSize, expectedYSize);

            Shape expectedShape = new LeftL(expectedXSize / 2);
            var partsCoordinates = expectedShape.PartsCoordinates;
            foreach (var part in partsCoordinates)
            {
                Assert.IsTrue(model.ActualShapePosition.Contains(part),"Before move is not OK");
            }

            model.StepGame();
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

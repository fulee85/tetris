using System;
using System.Linq;
using Tetris.Model.Shapes;
using Tetris.Model.Structs;

namespace Tetris.Model
{
    public class TetrisTable
    {
        public FieldStatus[][] GameTable { get; private set; }
        public int XSize { get; private set; }
        public int YSize { get; private set; }

        public TetrisTable(int xSize, int ySize)
        {
            if (xSize < 4 || ySize < 10) throw new Exception("Game size should be bigger.");
            this.XSize = xSize;
            this.YSize = ySize;
            GenerateNewGameTable();
        }

        private void GenerateNewGameTable()
        {
            GameTable = new FieldStatus[YSize][];
            for (int i = 0; i < GameTable.Length; i++)
            {
                GameTable[i] = GetEmptyRow();
            }
        }

        private FieldStatus[] GetEmptyRow()
        {
            return new FieldStatus[XSize];
        }

        public void ShapeLanded(Shape shape)
        {
            foreach (Coordinate part in shape.PartsCoordinates)
            {
                GameTable[part.y][part.x] = FieldStatus.NOT_FREE;
            }
        }

        public (int lastErasedRow, int linesErased) EraseFullLines()
        {
            int lastErasedRow = -1;
            int linesErased = 0;
            for (int y = 0; y < GameTable.Length; y++)
            {
                if (GameTable[y].All(p => p == FieldStatus.NOT_FREE))
                {
                    linesErased++;
                    for (int i = y; i > 0; i--)
                    {
                        GameTable[i] = GameTable[i - 1];
                    }
                    GameTable[0] = GetEmptyRow();
                    lastErasedRow = y;
                }
            }
            return (lastErasedRow, linesErased);
        }

        public bool IsPositionCoordinatesFree(Coordinate[] shapePositionCoordinates)
        {
            return shapePositionCoordinates.All(PositionValid);

            bool PositionValid(Coordinate position)
            {
                return 0 <= position.x && position.x < XSize &&
                       0 <= position.y && position.y < YSize &&
                       GameTable[position.y][position.x] == FieldStatus.FREE;
            }
        }

        public FieldStatus GetFieldStatus(Coordinate coordinate)
        {
            return GameTable[coordinate.y][coordinate.x];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tetris.Model.EventArg;
using Tetris.Model.Shapes;
using Tetris.Model.Structs;

namespace Tetris.Model
{
    public class TetrisModel
    {
        private Shape actualShape;
        private Shape nextShape;
        private IShapeFactory shapeFactory;
        private int gameScore;
        private bool isGamePaused;

        private int TableHorizontalCenter { get { return XSize / 2; } }
        public int GameTime { get; private set; }
        public FieldStatus[][] GameTable { get; private set; }
        public int XSize { get; private set; }
        public int YSize { get; private set; }
        public Coordinates[] ActualShapePosition { get { return actualShape.PartsCoordinates; } }

        public event EventHandler<TetrisFieldChangedAgrs> FieldStatusChanged;
        public event EventHandler<TetrisFieldChangedAgrs> NextShapeStatusChanged;
        public event EventHandler<GameOverArgs> GameOver;
        public event EventHandler<ScoreChangedArgs> ScoreChanged;

        public TetrisModel( IShapeFactory shapeFact = null)
        {           
            shapeFactory = shapeFact ?? new ShapeFactory();
        }

        public void StartNewGame(int xSize, int ySize)
        {
            if (xSize < 4 || ySize < 10) throw new Exception("Game size should be bigger.");
            this.XSize = xSize;
            this.YSize = ySize;

            GenerateNewGameTable();
            nextShape = shapeFactory.GetNewShape(TableHorizontalCenter);
            SetShapes();
            GameTime = 0;
            gameScore = 0;
            isGamePaused = false;
            ScoreChanged?.Invoke(this, new ScoreChangedArgs(gameScore));
        }

        private void SetShapes()
        {
            actualShape = nextShape;
            SetViewTablePositionsNotFree(actualShape.PartsCoordinates);
            SetViewNextShapePositionsFree(nextShape.PartsCoordinates);
            nextShape = shapeFactory.GetNewShape(TableHorizontalCenter);
            SetViewNextShapePositionsNotFree(nextShape.PartsCoordinates);
        }

        public void PauseGame()
        {
            isGamePaused = (isGamePaused) ? false : true;
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
            var emptyRow = new FieldStatus[XSize];
            for (int i = 0; i < XSize; i++)
            {
                emptyRow[i] = FieldStatus.FREE;
            }
            return emptyRow;
        }

        public void StepGame()
        {
            GameTime++;
            StepShapeDown();
        }

        private void ShapeLanded()
        {
            foreach (Coordinates part in actualShape.PartsCoordinates)
            {
                GameTable[part.y][part.x] = FieldStatus.NOT_FREE;
            }
            MaintainGameTable();
            SetShapes();
            if (!IsPositionCoordinatesFree(actualShape.PartsCoordinates))
            {
                GameOver?.Invoke(this, new GameOverArgs(gameScore));
            }
        }       

        private void MaintainGameTable()
        {
            int lastChangedRow = -1;
            for (int y = 0; y < GameTable.Length; y++)
            {
                if(GameTable[y].All(p => p == FieldStatus.NOT_FREE))
                {
                    gameScore++;
                    for (int i = y; i > 0; i--)
                    {
                        GameTable[i] = GameTable[i - 1];
                    }
                    GameTable[0] = GetEmptyRow();
                    lastChangedRow = y;
                }
            }
            if (lastChangedRow != -1)
            {
                RefreshTableViewFromRow(lastChangedRow);
                ScoreChanged?.Invoke(this, new ScoreChangedArgs(gameScore));
            }

        }

        private void RefreshTableViewFromRow(int lastChangedRow)
        {
            for (int yIndex = lastChangedRow; yIndex >=0; yIndex--)
            {
                for (int xIndex = 0; xIndex < XSize; xIndex++)
                {
                    FieldStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(new Coordinates(xIndex, yIndex), GameTable[yIndex][xIndex]));
                }
            }
        }   

        public void StepShapeRight()
        {
            StepShape(Directions.RIGHT);
        }

        public void StepShapeLeft()
        {
            StepShape(Directions.LEFT);
        }

        public void StepShapeDown()
        {
            if (StepShape(Directions.DOWN) == false)
            {
                ShapeLanded();
            }
        }

        private bool StepShape(Directions direction)
        {
            if (actualShape == null || isGamePaused) return true;
            var nextShapePositionCoordinates = actualShape.GetShapePositionAfterMove(direction);

            return TryChangeShapePosition(nextShapePositionCoordinates);
        }

        public void RotateShape()
        {
            if (actualShape == null || isGamePaused) return;        
            var nextShapePositionCoordinates = actualShape.GetShapePositionAfterRotation();

            TryChangeShapePosition(nextShapePositionCoordinates);
        }

        private bool TryChangeShapePosition(Coordinates[] nextShapePositionCoordinates)
        {
            if (IsPositionCoordinatesFree(nextShapePositionCoordinates))
            {
                SetViewTablePositionsFree(actualShape.PartsCoordinates.Except(nextShapePositionCoordinates));
                SetViewTablePositionsNotFree(nextShapePositionCoordinates.Except(actualShape.PartsCoordinates));
                actualShape.PartsCoordinates = nextShapePositionCoordinates;
                return true;
            }
            return false;
        }

        private bool IsPositionCoordinatesFree(Coordinates[] shapePositionCoordinates)
        {
            return shapePositionCoordinates.All(PositionValid);

            bool PositionValid(Coordinates position)
            {
                return 0 <= position.x && position.x < XSize && 0 <= position.y && position.y < YSize &&
                    GameTable[position.y][position.x] == FieldStatus.FREE; 
            }
        }


        private void SetViewTablePositionsFree(IEnumerable<Coordinates> freePositions)
        {
            foreach (Coordinates position in freePositions)
            {
                FieldStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(position, FieldStatus.FREE));
            }
        }

        private void SetViewTablePositionsNotFree(IEnumerable<Coordinates> notFreePositions)
        {
            foreach (Coordinates position in notFreePositions)
            {
                FieldStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(position, FieldStatus.NOT_FREE));
            }
        }

        private void SetViewNextShapePositionsFree(IEnumerable<Coordinates> freePositions)
        {
            foreach (Coordinates position in freePositions)
            {
                Coordinates shiftedCoordinates = new Coordinates(position.x - TableHorizontalCenter + 2, position.y);
                NextShapeStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(shiftedCoordinates, FieldStatus.FREE));
            }
        }

        private void SetViewNextShapePositionsNotFree(IEnumerable<Coordinates> notFreePositions)
        {
            foreach (Coordinates position in notFreePositions)
            {
                Coordinates shiftedCoordinates = new Coordinates(position.x - TableHorizontalCenter + 2, position.y);
                NextShapeStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(shiftedCoordinates, FieldStatus.NOT_FREE));
            }
        }
    }
}

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

        public void startNewGame(int xSize, int ySize)
        {
            if (xSize < 4 || ySize < 10) throw new Exception("Game size should be bigger.");
            this.XSize = xSize;
            this.YSize = ySize;

            generateNewGameTable();
            nextShape = shapeFactory.getNewShape(xSize);
            setShapes();
            GameTime = 0;
            gameScore = 0;
            isGamePaused = false;
            if (ScoreChanged != null)
                ScoreChanged(this, new ScoreChangedArgs(gameScore));
        }

        private void setShapes()
        {
            actualShape = nextShape;
            setViewTablePositionsNotFree(actualShape.PartsCoordinates);
            setViewNextShapePositionsFree(nextShape.PartsCoordinates);
            nextShape = shapeFactory.getNewShape(XSize);
            setViewNextShapePositionsNotFree(nextShape.PartsCoordinates);
        }

        public void pauseGame()
        {
            isGamePaused = (isGamePaused) ? false : true;
        }

        private void generateNewGameTable()
        {
            GameTable = new FieldStatus[YSize][];
            for (int i = 0; i < GameTable.Length; i++)
            {
                GameTable[i] = getEmptyRow();
            }
        }

        private FieldStatus[] getEmptyRow()
        {
            FieldStatus[] emptyRow = new FieldStatus[XSize];
            for (int i = 0; i < XSize; i++)
            {
                emptyRow[i] = FieldStatus.FREE;
            }
            return emptyRow;
        }

        public void stepGame()
        {
            GameTime++;
            if(stepShape(Directions.DOWN) == false)
            {
                shapeLanded();
            }
        }

        private void shapeLanded()
        {
            foreach (Coordinates part in actualShape.PartsCoordinates)
            {
                GameTable[part.y][part.x] = FieldStatus.NOT_FREE;
            }
            maintainGameTable();
            setShapes();
            if (!isPositionCoordinatesFree(actualShape.PartsCoordinates))
            {
                GameOver?.Invoke(this, new GameOverArgs(GameTime));
            }
        }       

        private void maintainGameTable()
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
                    GameTable[0] = getEmptyRow();
                    lastChangedRow = y;
                }
            }
            if (lastChangedRow != -1)
            {
                refreshTableViewFromRow(lastChangedRow);
                if (ScoreChanged != null)
                    ScoreChanged(this, new ScoreChangedArgs(gameScore));
            }

        }

        private void refreshTableViewFromRow(int lastChangedRow)
        {
            for (int yIndex = lastChangedRow; yIndex >=0; yIndex--)
            {
                for (int xIndex = 0; xIndex < XSize; xIndex++)
                {
                    FieldStatusChanged(this, new TetrisFieldChangedAgrs(new Coordinates(xIndex, yIndex), GameTable[yIndex][xIndex]));
                }
            }
        }   

        public void stepShapeRight()
        {
            stepShape(Directions.RIGHT);
        }

        public void stepShapeLeft()
        {
            stepShape(Directions.LEFT);
        }

        public void stepShapeDown()
        {
            if (stepShape(Directions.DOWN) == false)
            {
                shapeLanded();
            }
        }

        private bool stepShape(Directions direction)
        {
            if (actualShape == null || isGamePaused) return true;
            Coordinates[] nextShapePositionCoordinates = actualShape.getShapePositionAfterMove(direction);
            if (isPositionCoordinatesFree(nextShapePositionCoordinates))
            {
                setViewTablePositionsFree(actualShape.PartsCoordinates.Except(nextShapePositionCoordinates));
                setViewTablePositionsNotFree(nextShapePositionCoordinates.Except(actualShape.PartsCoordinates));
                actualShape.PartsCoordinates = nextShapePositionCoordinates;
                return true;
            }
            return false;
        }

        public void rotateShape()
        {
            if (actualShape != null && !isGamePaused)
            {
                Coordinates[] nextShapePositionCoordinates = actualShape.getShapePositionAfterRotation();
                if (isPositionCoordinatesFree(nextShapePositionCoordinates))
                {
                    setViewTablePositionsFree(actualShape.PartsCoordinates.Except(nextShapePositionCoordinates));
                    setViewTablePositionsNotFree(nextShapePositionCoordinates.Except(actualShape.PartsCoordinates));
                    actualShape.PartsCoordinates = nextShapePositionCoordinates;
                }
            }
        }

        private bool isPositionCoordinatesFree(Coordinates[] shapePositionCoordinates)
        {
            return shapePositionCoordinates.All(isPositionValid);
        }

        private bool isPositionValid(Coordinates position)
        {
            return 0 <= position.x && position.x < XSize && 0 <= position.y && position.y < YSize &&
                GameTable[position.y][position.x] == FieldStatus.FREE; 
        }

        private void setViewTablePositionsFree(IEnumerable<Coordinates> freePositions)
        {
            foreach (Coordinates position in freePositions)
            {
                if (FieldStatusChanged != null)
                    FieldStatusChanged(this, new TetrisFieldChangedAgrs(position, FieldStatus.FREE));
            }
        }

        private void setViewTablePositionsNotFree(IEnumerable<Coordinates> notFreePositions)
        {
            foreach (Coordinates position in notFreePositions)
            {
                if (FieldStatusChanged != null)
                    FieldStatusChanged(this, new TetrisFieldChangedAgrs(position, FieldStatus.NOT_FREE));
            }
        }

        private void setViewNextShapePositionsFree(IEnumerable<Coordinates> freePositions)
        {
            foreach (Coordinates position in freePositions)
            {
                Coordinates shiftedCoordinates = new Coordinates(position.x - XSize / 2 + 2, position.y);
                if (NextShapeStatusChanged != null)
                    NextShapeStatusChanged(this, new TetrisFieldChangedAgrs(shiftedCoordinates, FieldStatus.FREE));
            }
        }

        private void setViewNextShapePositionsNotFree(IEnumerable<Coordinates> notFreePositions)
        {
            foreach (Coordinates position in notFreePositions)
            {
                Coordinates shiftedCoordinates = new Coordinates(position.x - XSize / 2 + 2, position.y);
                if (NextShapeStatusChanged != null)
                    NextShapeStatusChanged(this, new TetrisFieldChangedAgrs(shiftedCoordinates, FieldStatus.NOT_FREE));
            }
        }
    }
}

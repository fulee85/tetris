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
        private FieldStatus[][] gameTable;
        private int xSize, ySize;
        private Shape actualShape;
        private Shape nextShape;
        private IShapeFactory shapeFactory;
        private int gameTime;
        private int gameScore;
        private bool isGamePaused;

        public int GameTime { get { return gameTime; } }
        public FieldStatus[][] GameTable { get { return gameTable; } }
        public int XSize { get { return xSize; } }
        public int YSize { get { return ySize; } }
        public Coordinates[] ActualShapePosition { get { return actualShape.PartsCoordinates; } }

        public event EventHandler<TetrisFieldChangedAgrs> FieldStatusChanged;
        public event EventHandler<TetrisFieldChangedAgrs> NextShapeStatusChanged;
        public event EventHandler<GameOverArgs> GameOver;
        public event EventHandler<ScoreChangedArgs> ScoreChanged;

        public TetrisModel( IShapeFactory shapeFact = null)
        {           
            shapeFactory = (shapeFact == null) ? new ShapeFactory() : shapeFact;
        }

        public void startNewGame(int xSize, int ySize)
        {
            if (xSize < 4 || ySize < 10) throw new Exception("Game size should be bigger.");
            this.xSize = xSize;
            this.ySize = ySize;

            generateNewGameTable();
            nextShape = shapeFactory.getNewShape(xSize);
            setShapes();
            gameTime = 0;
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
            nextShape = shapeFactory.getNewShape(xSize);
            setViewNextShapePositionsNotFree(nextShape.PartsCoordinates);
        }

        public void pauseGame()
        {
            isGamePaused = (isGamePaused) ? false : true;
        }

        private void generateNewGameTable()
        {
            gameTable = new FieldStatus[ySize][];
            for (int i = 0; i < gameTable.Length; i++)
            {
                gameTable[i] = getEmptyRow();
            }
        }

        private FieldStatus[] getEmptyRow()
        {
            FieldStatus[] emptyRow = new FieldStatus[xSize];
            for (int i = 0; i < xSize; i++)
            {
                emptyRow[i] = FieldStatus.FREE;
            }
            return emptyRow;
        }

        public void stepGame()
        {
            gameTime++;
            if(stepShape(Directions.DOWN) == false)
            {
                shapeLanded();
            }
        }

        private void shapeLanded()
        {
            foreach (Coordinates part in actualShape.PartsCoordinates)
            {
                gameTable[part.y][part.x] = FieldStatus.NOT_FREE;
            }
            maintainGameTable();
            setShapes();
            if (!isPositionCoordinatesFree(actualShape.PartsCoordinates))
            {
                if (GameOver != null)
                    GameOver(this, new GameOverArgs(gameTime));
            }
        }       

        private void maintainGameTable()
        {
            int lastChangedRow = -1;
            for (int y = 0; y < gameTable.Length; y++)
            {
                if(gameTable[y].All(p => p == FieldStatus.NOT_FREE))
                {
                    gameScore++;
                    for (int i = y; i > 0; i--)
                    {
                        gameTable[i] = gameTable[i - 1];
                    }
                    gameTable[0] = getEmptyRow();
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
                for (int xIndex = 0; xIndex < xSize; xIndex++)
                {
                    FieldStatusChanged(this, new TetrisFieldChangedAgrs(new Coordinates(xIndex, yIndex), gameTable[yIndex][xIndex]));
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
            return 0 <= position.x && position.x < xSize && 0 <= position.y && position.y < ySize &&
                gameTable[position.y][position.x] == FieldStatus.FREE; 
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
                Coordinates shiftedCoordinates = new Coordinates(position.x - xSize / 2 + 2, position.y);
                if (NextShapeStatusChanged != null)
                    NextShapeStatusChanged(this, new TetrisFieldChangedAgrs(shiftedCoordinates, FieldStatus.FREE));
            }
        }

        private void setViewNextShapePositionsNotFree(IEnumerable<Coordinates> notFreePositions)
        {
            foreach (Coordinates position in notFreePositions)
            {
                Coordinates shiftedCoordinates = new Coordinates(position.x - xSize / 2 + 2, position.y);
                if (NextShapeStatusChanged != null)
                    NextShapeStatusChanged(this, new TetrisFieldChangedAgrs(shiftedCoordinates, FieldStatus.NOT_FREE));
            }
        }
    }
}

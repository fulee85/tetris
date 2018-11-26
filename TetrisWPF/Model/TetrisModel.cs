using System;
using System.Collections.Generic;
using System.Linq;
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
        private TetrisTable tetrisTable;

        private int TableHorizontalCenter { get { return TableHozizontalSize / 2; } }
        public int GameTime { get; private set; }
        public int TableHozizontalSize { get; private set; }
        private int GameScore {
            get
            {
                return gameScore;
            }
            set
            {
                gameScore = value;
                ScoreChanged?.Invoke(this, new ScoreChangedArgs(gameScore));
            }
        }

        public Coordinate[] ActualShapePosition { get { return actualShape.PartsCoordinates; } }

        public event EventHandler<TetrisFieldChangedAgrs> FieldStatusChanged;
        public event EventHandler<TetrisFieldChangedAgrs> NextShapeStatusChanged;
        public event EventHandler<GameOverArgs> GameOver;
        public event EventHandler<ScoreChangedArgs> ScoreChanged;

        public TetrisModel(IShapeFactory shapeFact = null)
        {
            shapeFactory = shapeFact ?? new ShapeFactory();
        }

        public void StartNewGame(int xSize, int ySize)
        {
            tetrisTable = new TetrisTable(xSize, ySize, this);
            TableHozizontalSize = xSize;
            nextShape = shapeFactory.GetNewShape(TableHorizontalCenter);
            SetShapes();
            GameTime = 0;
            gameScore = 0;
            isGamePaused = false;
            ScoreChanged?.Invoke(this, new ScoreChangedArgs(gameScore));
        }

        private void ShapeLanded()
        {
            tetrisTable.ShapeLanded(actualShape);
            var (lastErasedRow, linesErased) = tetrisTable.EraseFullLines();
            if (linesErased != 0)
            {
                RefreshTableViewFromRow(lastErasedRow);
                GameScore += linesErased;
            }
            SetShapes();
            if (!tetrisTable.IsPositionCoordinatesFree(actualShape.PartsCoordinates))
            {
                GameOver?.Invoke(this, new GameOverArgs(gameScore));
            }
        }
        
        private void RefreshTableViewFromRow(int lastErasedRow)
        {
            for (int yIndex = lastErasedRow; yIndex >= 0; yIndex--)
            {
                for (int xIndex = 0; xIndex < TableHozizontalSize; xIndex++)
                {
                    var coordinate = new Coordinate(xIndex, yIndex);
                    var fieldStatus = tetrisTable.GetFieldStatus(coordinate);
                    FieldStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(coordinate, fieldStatus));
                }
            }
        }

        private void SetShapes()
        {
            actualShape = nextShape;
            SetViewTableCoordinates(actualShape.PartsCoordinates, FieldStatus.NOT_FREE);
            SetNextShapeViewCoordinates(nextShape.PartsCoordinates, FieldStatus.FREE);
            nextShape = shapeFactory.GetNewShape(TableHorizontalCenter);
            SetNextShapeViewCoordinates(nextShape.PartsCoordinates, FieldStatus.NOT_FREE);
        }

        public void PauseGame()
        {
            isGamePaused = (isGamePaused) ? false : true;
        }
        
        public void StepGame()
        {
            GameTime++;
            StepShapeDown();
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
            var shapeNextPosition = actualShape.GetShapePositionAfterMove(direction);

            return TryChangeShapePosition(shapeNextPosition);
        }

        public void RotateShape()
        {
            if (actualShape == null || isGamePaused) return;
            var shapeNextPosition = actualShape.GetShapePositionAfterRotation();

            TryChangeShapePosition(shapeNextPosition);
        }

        private bool TryChangeShapePosition(Coordinate[] shapeNextPositionCoordinates)
        {
            if (tetrisTable.IsPositionCoordinatesFree(shapeNextPositionCoordinates))
            {
                SetViewTableCoordinates(actualShape.PartsCoordinates.Except(shapeNextPositionCoordinates), FieldStatus.FREE);
                SetViewTableCoordinates(shapeNextPositionCoordinates.Except(actualShape.PartsCoordinates), FieldStatus.NOT_FREE);
                actualShape.PartsCoordinates = shapeNextPositionCoordinates;
                return true;
            }
            return false;
        }

        private void SetViewTableCoordinates(IEnumerable<Coordinate> coordinates, FieldStatus status)
        {
            foreach (Coordinate coordinate in coordinates)
            {
                FieldStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(coordinate, status));
            }
        }

        private void SetNextShapeViewCoordinates(IEnumerable<Coordinate> coordinates, FieldStatus status)
        {
            foreach (Coordinate coordinate in coordinates)
            {
                Coordinate shiftedCoordinates = new Coordinate(coordinate.x - TableHorizontalCenter + 2, coordinate.y);
                NextShapeStatusChanged?.Invoke(this, new TetrisFieldChangedAgrs(shiftedCoordinates, status));
            }
        }
    }
}

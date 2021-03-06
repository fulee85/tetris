﻿using System;
using System.Collections.ObjectModel;
using Tetris.Model;
using Tetris.Model.EventArg;


namespace Tetris.ViewModel
{
    public class TetrisViewModel : ViewModelBase
    {
        public ObservableCollection<TetrisField> Fields { get; set; }
        public ObservableCollection<TetrisField> NextShapeFields { get; set; }

        private readonly int _rowNumber = 16;
        private int _columnNumber = 8;
        private int _score;
        private double _windowWidth;

        public DelegateCommand TablesizeChangeCommand { get; private set; }
        public DelegateCommand SizeDialogOK { get; private set; }
        public DelegateCommand RotateShape { get; private set; }
        public DelegateCommand MoveShape { get; private set; }
        public DelegateCommand PauseGame { get; private set; }
        public DelegateCommand ExitGame { get; private set; }
        public DelegateCommand StartNewGame { get; private set; }

        public event EventHandler TableSizeDialogClosed;
        public event EventHandler Exit;
        public event EventHandler NewGame;
        public event EventHandler<GameOverEventArgs> GameOver;

        private readonly TetrisModel _tetrisModel;

        public int ColumnNumber
        {
            get { return _columnNumber; }
            set
            {
                if (_columnNumber != value)
                {
                    _columnNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                OnPropertyChanged();
            }
        }

        public TetrisViewModel(TetrisModel tetrisModel)
        {
            _tetrisModel = tetrisModel;
            _tetrisModel.FieldStatusChanged += _tetrisModel_FieldStatusChanged;
            _tetrisModel.NextShapeStatusChanged += _tetrisModel_NextShapeStatusChanged;
            _tetrisModel.GameOver += _tetrisModel_GameOver;
            _tetrisModel.ScoreChanged += _tetrisModel_ScoreChanged;

            Fields = new ObservableCollection<TetrisField>();
            NextShapeFields = new ObservableCollection<TetrisField>();
            for (int i = 0; i < 16; i++)
            {
                NextShapeFields.Add(new TetrisField
                {
                    IsFree = true
                });
            }

            PauseGame = new DelegateCommand(param => _tetrisModel.PauseGame());
            RotateShape = new DelegateCommand(param => _tetrisModel.RotateShape());
            TablesizeChangeCommand = new DelegateCommand(ChangeTableSizeAction);
            SizeDialogOK = new DelegateCommand(param => OnOKButtonPressed());
            MoveShape = new DelegateCommand(MoveShapeAction);
            ExitGame = new DelegateCommand(param => Exit?.Invoke(this, EventArgs.Empty));
            StartNewGame = new DelegateCommand(param => NewGame?.Invoke(this, EventArgs.Empty));
        }

        private void ChangeTableSizeAction(object size)
        {
            switch (size.ToString())
            {
                case "Small":
                    ColumnNumber = 4;
                    break;
                case "Medium":
                    ColumnNumber = 8;
                    break;
                case "Large":
                    ColumnNumber = 12;
                    break;
            }
        }

        private void MoveShapeAction(object direction)
        {
            switch (direction.ToString())
            {
                case "left":
                    _tetrisModel.StepShapeLeft();
                    break;
                case "right":
                    _tetrisModel.StepShapeRight();
                    break;
                case "down":
                    _tetrisModel.StepShapeDown();
                    break;
            }
        }

        private void _tetrisModel_NextShapeStatusChanged(object sender, TetrisFieldChangedAgrs e)
        {
            if (e.IsFieldEmpty)
            {
                NextShapeFields[e.YPosition * 4 + e.XPosition].IsFree = true;
            }
            else
            {
                NextShapeFields[e.YPosition * 4 + e.XPosition].IsFree = false;
            }
        }

        private void _tetrisModel_ScoreChanged(object sender, ScoreChangedArgs e)
        {
            Score = e.GameScore;
        }

        private void _tetrisModel_GameOver(object sender, GameOverArgs e)
        {
            GameOver?.Invoke(this, new GameOverEventArgs { Score = e.Score });
        }

        private void _tetrisModel_FieldStatusChanged(object sender, TetrisFieldChangedAgrs e)
        {
            if (e.IsFieldEmpty)
            {
                Fields[e.YPosition * _columnNumber + e.XPosition].IsFree = true;
            }
            else
            {
                Fields[e.YPosition * _columnNumber + e.XPosition].IsFree = false;
            }
        }

        public void StartGame()
        {
            CreateTable();
            foreach (TetrisField label in NextShapeFields)
            {
                label.IsFree = true;
            }
            WindowWidth = 110 + 25 * _columnNumber;
            _tetrisModel.StartNewGame(_columnNumber, _rowNumber);
        }

        public void CreateTable()
        {
            Fields.Clear();
            for (int i = 0; i < _rowNumber * _columnNumber; i++)
            {
                Fields.Add(new TetrisField { IsFree = true });
            }
        }

        private void OnOKButtonPressed()
        {
            TableSizeDialogClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}

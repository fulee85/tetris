using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Tetris.View;
using Tetris.Model;
using Tetris.Model.EventArg;


namespace Tetris.ViewModel
{
    public class TetrisViewModel : ViewModelBase
    {
        public ObservableCollection<TetrisField> Fields { get; set; }
        public ObservableCollection<TetrisField> NextShapeFields { get; set; }
        private int _rowNumber = 16, _columnNumber = 8;
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

        private TetrisModel _tetrisModel;

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

            TablesizeChangeCommand = new DelegateCommand(param =>
            {
                switch (param.ToString()) {
                    case "Small": ColumnNumber = 4;
                        break;
                    case "Medium": ColumnNumber = 8;
                        break;
                    case "Large": ColumnNumber = 12;
                        break;
                }
            });
            SizeDialogOK = new DelegateCommand(param =>
                { OnOKButtonPressed(); }
            );
            RotateShape = new DelegateCommand(param => { _tetrisModel.rotateShape(); });
            MoveShape = new DelegateCommand(param =>
            {
                switch (param.ToString())
                {
                    case "left":
                        _tetrisModel.stepShapeLeft();
                        break;
                    case "right":
                        _tetrisModel.stepShapeRight();
                        break;
                    case "down":
                        _tetrisModel.stepShapeDown();
                        break;
                }
            });
            PauseGame = new DelegateCommand(param => { _tetrisModel.pauseGame(); });
            ExitGame = new DelegateCommand(param => { exitGame(); });
            StartNewGame = new DelegateCommand(param => { startNewGame(); });       
        }

        private void startNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void exitGame()
        {
            Exit?.Invoke(this, EventArgs.Empty);
        }

        private void _tetrisModel_NextShapeStatusChanged(object sender, TetrisFieldChangedAgrs e)
        {
            if (e.IsEmptyField)
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
            GameOver?.Invoke(this, new GameOverEventArgs { Time = e.GameTime });
        }

        private void _tetrisModel_FieldStatusChanged(object sender, TetrisFieldChangedAgrs e)
        {
            if (e.IsEmptyField)
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
            _tetrisModel.startNewGame(_columnNumber, _rowNumber);
        }

        public void CreateTable()
        {
            Fields.Clear();
            for (int i = 0; i < _rowNumber * _columnNumber; i++)
            {
                Fields.Add(new TetrisField{ IsFree = true });
            }
        }

        private void OnOKButtonPressed()
        {
            TableSizeDialogClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}

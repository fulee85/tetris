using System;
using System.Windows;
using Tetris.View;
using Tetris.ViewModel;
using Tetris.Model;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TetrisViewModel _viewModel;
        private MainWindow _view;
        private TetrisModel _model;
        private DispatcherTimer _timer;
        private TableSizeDialog _tableSizeDialog;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new TetrisModel();

            _viewModel = new TetrisViewModel(_model);
            _viewModel.TableSizeDialogClosed += _viewModel_TableSizeDialogClosed;
            _viewModel.GameOver += _viewModel_GameOver;
            _viewModel.Exit += _viewModel_Exit;
            _viewModel.NewGame += _viewModel_NewGame;

            _tableSizeDialog = new TableSizeDialog();
            _tableSizeDialog.DataContext = _viewModel;
            _tableSizeDialog.ShowDialog();

            _view = new MainWindow
            {
                DataContext = _viewModel
            };
            _view.Closed += _view_Closed;
            _view.Show();

            _viewModel.StartGame();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.stepGame();
        }

        private void _viewModel_NewGame(object sender, EventArgs e)
        {
            Current.Dispatcher.BeginInvoke(new Action(() => this.startNewGame()));
        }

        private void _viewModel_Exit(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void _viewModel_GameOver(object sender, GameOverEventArgs e)
        {
            _timer.Stop();
            var answer = MessageBox.Show("Game over, your time: " + e.Time + " s\n Would you like a new Game?", "Game over", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.No)
            {
                Current.Dispatcher.BeginInvoke(new Action(() => this.Shutdown()));
            }
            else
            {
                Current.Dispatcher.BeginInvoke(new Action(() => this.startNewGame()));
            }          
        }

        private void startNewGame()
        {
            _timer.Stop();
            _view.Hide();
            _tableSizeDialog.ShowDialog();
            _view.Show();
            _viewModel.StartGame();
            _timer.Start();
        }

        private void _viewModel_TableSizeDialogClosed(object sender, EventArgs e)
        {
            _tableSizeDialog.Hide();
        }

        private void _view_Closed(object sender, EventArgs e)
        {
            Shutdown();
        }
    }
}

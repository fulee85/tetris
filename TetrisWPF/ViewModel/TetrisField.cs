namespace Tetris.ViewModel
{
    public class TetrisField : ViewModelBase
    {
        private bool _isFree;

        public bool IsFree
        {
            get
            {
                return _isFree;
            }
            set
            {
                _isFree = value;
                OnPropertyChanged();
            }
        }
    }
}

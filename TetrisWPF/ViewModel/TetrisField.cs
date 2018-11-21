using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

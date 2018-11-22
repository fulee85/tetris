using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Tetris.Model.Shapes
{
    public interface IShapeFactory
    {
        Shape GetNewShape(int playFieldHorizontalSize);
    }
}

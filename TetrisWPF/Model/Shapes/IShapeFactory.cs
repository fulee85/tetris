namespace Tetris.Model.Shapes
{
    public interface IShapeFactory
    {
        Shape GetNewShape(int tableCenter);
    }
}

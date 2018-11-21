﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Model.Structs;

namespace Tetris.Model.Shapes
{
    public class TShape : Shape
    {
        public TShape(int playFieldHorizontalSize) : base(playFieldHorizontalSize)
        {

            center = new Coordinates(playFieldHorizontalSize / 2, 1);
            shapePosition[0] = new Coordinates(center.x - 1, center.y);
            shapePosition[1] = center;
            shapePosition[2] = new Coordinates(center.x + 1, center.y);
            shapePosition[3] = new Coordinates(center.x, center.y - 1);
        }
    }
}

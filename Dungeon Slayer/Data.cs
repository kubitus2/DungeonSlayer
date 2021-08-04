using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{

    struct Vector2DInt
    {
        public int x;
        public int y;

        public Vector2DInt(int a, int b)
        {
            x = a;
            y = b;
        }

        public static Vector2DInt operator +(Vector2DInt a, Vector2DInt b)
        {
            Vector2DInt sum = new Vector2DInt();
            sum.x = a.x + b.x;
            sum.y = a.y + b.y;

            return sum;
        }



    }
}
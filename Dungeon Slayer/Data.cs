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

        public static Vector2DInt operator +(Vector2DInt a, Vector2DInt b) => new Vector2DInt(a.x + b.x, a.y + b.y);

    }

    struct Stats
    {
        public int luck;
        public int agility;
        public int power;

        public Stats (int l, int a, int p)
        {
            luck = l;
            agility = a;
            power = p;
        }
    }
}
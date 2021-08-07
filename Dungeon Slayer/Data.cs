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

        public static Vector2DInt operator +(Vector2DInt vec1, Vector2DInt vec2)
        {
            Vector2DInt ret = new Vector2DInt(vec1.x + vec2.x, vec1.y + vec2.y);

            return ret;


        }


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
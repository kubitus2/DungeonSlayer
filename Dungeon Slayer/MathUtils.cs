using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon_Slayer
{
    class MathUtils
    {
        //Clamp values between min and max value.
        public static int Clamp(int min, int max, int value)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }
    }
}

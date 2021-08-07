namespace Dungeon_Slayer
{
    //Vector2D to stor (int, int) position.
    struct Vector2DInt
    {
        //Coords.
        public int x;
        public int y;

        public Vector2DInt(int a, int b)
        {
            x = a;
            y = b;
        }

        //Overload + operator to add two vectors together.
        public static Vector2DInt operator +(Vector2DInt a, Vector2DInt b) => new Vector2DInt(a.x + b.x, a.y + b.y);

    }

    //Stats container.
    struct Stats
    {
        public int luck;
        public int agility;
        public int power;

        public Stats(int l, int a, int p)
        {
            luck = l;
            agility = a;
            power = p;
        }
    }

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

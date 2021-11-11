using System;

namespace Lab_1_worldOfWorms.Engine
{
    public class Vector2d
    {
        public float x, y;

        public Vector2d(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2d() : this(0, 0) {}
        
        public static Vector2d operator+(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2d operator -(Vector2d v)
        {
            return new Vector2d(-v.x, -v.y);
        }

        public static Vector2d operator -(Vector2d v1, Vector2d v2)
        {
            return v1 + -v2;
        }

        public static Vector2d operator *(Vector2d v, float f)
        {
            return new Vector2d(v.x * f, v.y * f);
        }

        public static Vector2d operator /(Vector2d v, float f)
        {
            return v * (1 / f);
        }

        public float Length()
        {
            return (float) Math.Sqrt(x * x + y * y);
        }

        public Vector2d Normalize()
        {
            return this / Length();
        }

        public static explicit operator Position(Vector2d v)
        {
            return new Position((int) Math.Round(v.x), (int) Math.Round(v.y));
        }
    }
}
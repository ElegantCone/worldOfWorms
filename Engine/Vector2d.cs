using System;

namespace WormsWorld.Engine
{
    public class Vector2d
    {
        private readonly float _x;
        private readonly float _y;

        public Vector2d(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        public Vector2d() : this(0, 0) {}
        
        public static Vector2d operator+(Vector2d v1, Vector2d v2)
        {
            return new Vector2d(v1._x + v2._x, v1._y + v2._y);
        }

        public static Vector2d operator -(Vector2d v)
        {
            return new Vector2d(-v._x, -v._y);
        }

        public static Vector2d operator -(Vector2d v1, Vector2d v2)
        {
            return v1 + -v2;
        }

        public static Vector2d operator *(Vector2d v, float f)
        {
            return new Vector2d(v._x * f, v._y * f);
        }

        public static Vector2d operator /(Vector2d v, float f)
        {
            return v * (1 / f);
        }

        public float Length()
        {
            return (float) Math.Sqrt(_x * _x + _y * _y);
        }

        public Vector2d Normalize()
        {
            return this / Length();
        }

        public static explicit operator Position(Vector2d v)
        {
            return new Position((int) Math.Round(v._x), (int) Math.Round(v._y));
        }
    }
}
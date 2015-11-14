using UnityEngine;
using System.Collections;

namespace AISandbox
{
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // Declare which operator to overload (+), the types 
        // that can be added (two Complex objects), and the 
        // return type (Complex):
        public static Vector2Int operator +(Vector2Int c1, Vector2Int c2)
        {
            return new Vector2Int(c1.x + c2.x, c1.y + c2.y);
        }

        public static Vector2Int operator -(Vector2Int c1, Vector2Int c2)
        {
            return new Vector2Int(c1.x - c2.x, c1.y - c2.y);
        }

        public static Vector2Int operator *(Vector2Int c, int f)
        {
            return new Vector2Int(c.x * f, c.y * f);
        }

        public static Vector2Int operator /(Vector2Int c, int f)
        {
            return new Vector2Int(c.x / f, c.y / f);
        }
    }
}

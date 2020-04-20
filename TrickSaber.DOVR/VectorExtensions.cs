using System;
using UnityEngine;

namespace TrickSaber.DOVR
{
    public static class VectorExtensions
    {
        public static Vector3 GetBiggestAxisDirection(this Vector3 input)
        {
            var x = Math.Abs(input.x);
            var y = Math.Abs(input.y);
            var z = Math.Abs(input.z);

            if (x > y && x > z) return Vector3.right;

            if (y > z) return Vector3.up;

            return Vector3.forward;
        }

        public static Vector3 GetBiggestAxis(this Vector3 input)
        {
            var biggestAxis = input.GetBiggestAxisDirection();
            return new Vector3(input.x * biggestAxis.x, input.y * biggestAxis.y, input.z * biggestAxis.z);
        }

        public static float Sum(this Vector3 input)
        {
            return input.x + input.y + input.z;
        }
    }
}
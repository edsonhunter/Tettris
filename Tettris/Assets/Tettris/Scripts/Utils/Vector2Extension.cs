using System;
using System.Numerics;

namespace Tettris.Utils
{
    public static class Vector2Extension
    {
        public static Vector2 RotateAroundPivotVector2(this Vector2 point, Vector2 pivot, float angleDegrees)
        {
            var rad = angleDegrees * (MathF.PI / 180f);
            var cos = MathF.Cos(rad);
            var sin = MathF.Sin(rad);

            var dx = point.X - pivot.X;
            var dy = point.Y - pivot.Y;

            var newX = dx * cos - dy * sin + pivot.X;
            var newY = dx * sin + dy * cos + pivot.Y;

            return new Vector2((float)Math.Round(newX), (float)Math.Round(newY));
        }
    }
}
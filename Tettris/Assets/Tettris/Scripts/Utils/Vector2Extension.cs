using UnityEngine;

namespace Tettris.Utils
{
    public static class Vector2Extension
    {
        public static Vector2 RotateAroundPivotVector2(this Vector2 Point, Vector3 Pivot, Quaternion Angle)
        {
            return RotateAroundPivotVector3(Point, Pivot, Angle);
        }
        
        public static Vector3 RotateAroundPivotVector3(this Vector3 Point, Vector3 Pivot, Quaternion Angle)
        {
            return Angle * (Point - Pivot) + Pivot;
        }
    }
}
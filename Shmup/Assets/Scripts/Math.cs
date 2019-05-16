using UnityEngine;

namespace Shmup.Math
{
    public static class MathHelper
    {
        public static float GetAngleTo(Vector2 fromPosition, Vector2 toPosition)
        {
            float angle = Mathf.Atan2(toPosition.y - fromPosition.y, toPosition.x - fromPosition.x);
            return angle * Mathf.Rad2Deg;
        }
    }
}
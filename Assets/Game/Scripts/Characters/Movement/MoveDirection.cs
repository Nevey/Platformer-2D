using UnityEngine;

namespace Game.Characters.Movement
{
    public enum MoveDirection
    {
        Right,
        Left,
    }

    public static class MoveDirectionExtensions
    {
        public static Vector3 GetVectorNormalized(this MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    return Vector2.left;
                
                case MoveDirection.Right:
                    return Vector2.right;
            }

            return Vector2.zero;
        }
    }
}

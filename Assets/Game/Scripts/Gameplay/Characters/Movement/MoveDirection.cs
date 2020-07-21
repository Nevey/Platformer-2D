using UnityEngine;

namespace Game.Gameplay.Characters.Movement
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

        public static MoveDirection? GetMoveDirection(Transform self, Transform target)
        {
            Vector3 directionVector = target.position - self.position;

            if (directionVector.x > 0)
            {
                return MoveDirection.Right;
            }

            if (directionVector.x < 0)
            {
                return MoveDirection.Left;
            }

            return null;
        }
    }
}

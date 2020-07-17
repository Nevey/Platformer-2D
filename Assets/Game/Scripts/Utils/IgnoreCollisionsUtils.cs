using UnityEngine;

namespace Game.Utils
{
    public static class IgnoreCollisionsUtil
    {
        public static void IgnoreAllCollisions(GameObject a, GameObject b)
        {
            Collider2D[] aColliders = a.GetComponentsInChildren<Collider2D>();
            Collider2D[] bColliders = b.GetComponentsInChildren<Collider2D>();

            for (int i = 0; i < aColliders.Length; i++)
            {
                for (int k = 0; k < bColliders.Length; k++)
                {
                    Physics2D.IgnoreCollision(aColliders[i], bColliders[k], true);
                }
            }
        }
    }
}

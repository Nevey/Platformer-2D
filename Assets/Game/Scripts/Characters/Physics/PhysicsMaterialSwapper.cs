using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters.Physics
{
    public class PhysicsMaterialSwapper : MonoBehaviour
    {
        [SerializeField] private PhysicsMaterial2D physicsMaterialToApply;

        private Collider2D[] colliders;
        private Dictionary<Collider2D, PhysicsMaterial2D> originalPhysicsMaterials = new Dictionary<Collider2D, PhysicsMaterial2D>();

        private void Awake()
        {
            colliders = GetComponentsInChildren<Collider2D>();

            for (int i = 0; i < colliders.Length; i++)
            {
                originalPhysicsMaterials[colliders[i]] = colliders[i].sharedMaterial;
            }
        }

        public void Swap(Collider2D collider2D)
        {
            collider2D.sharedMaterial = physicsMaterialToApply;
        }

        public void Swap()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                Swap(colliders[i]);
            }
        }

        public void RevertSwap(Collider2D collider2D, PhysicsMaterial2D physicsMaterial2D)
        {
            collider2D.sharedMaterial = physicsMaterial2D;
        }

        public void RevertSwap()
        {
            foreach (var item in originalPhysicsMaterials)
            {
                RevertSwap(item.Key, item.Value);
            }
        }
    }
}

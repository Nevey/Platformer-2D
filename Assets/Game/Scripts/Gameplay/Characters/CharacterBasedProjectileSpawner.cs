﻿using UnityEngine;
using Game.Utils;
using Game.Gameplay.Projectiles;
using Game.Gameplay.Characters.Movement;

#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Game.Gameplay.Characters
{
    public abstract class CharacterBasedProjectileSpawner : SpawnerAddForce
    {
        protected CharacterController characterController;

        protected override void Awake()
        {
            base.Awake();

            characterController = GetComponentInParent<CharacterController>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() != null || owner == transform || owner == null)
            {
                return;
            }

            if (owner.GetComponent<CharacterController>() == null)
            {
                throw Log.Exception($"No type of <b>CharacterController</b> found on Parent of <b>{name}</b>!");
            }
        }
#endif

        protected override Vector2 GetSpawnDirectionNormalized()
        {
            return characterController.MoveDirection.GetVectorNormalized();
        }
    }
}

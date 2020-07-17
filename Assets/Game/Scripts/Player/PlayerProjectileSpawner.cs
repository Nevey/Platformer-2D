using Game.Characters.Movement;
using Game.Projectiles;
using Game.Utils;
using UnityEngine;
using CharacterController = Game.Characters.CharacterController;
using Game.DI;
using Game.UserInput;

#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Game.Player
{
    public class PlayerProjectileSpawner : ProjectileSpawner
    {
        [Inject] private PlayerInputController playerInputController;

        private CharacterController characterController;

        protected override void Awake()
        {
            base.Awake();

            playerInputController.ActionEvent += OnAction;
            characterController = GetComponentInParent<CharacterController>();
        }

        protected override void OnDestroy()
        {
            playerInputController.ActionEvent -= OnAction;

            base.OnDestroy();
        }

        private void OnValidate()
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() != null || transform.root == transform)
            {
                return;
            }

            if (GetComponentInParent<CharacterController>() == null)
            {
                throw Log.Exception($"No type of <b>CharacterController</b> found on Parent of <b>{name}</b>!");
            }
        }

        private void OnAction(PlayerInputAction playerInputAction, ActionState actionState)
        {
            if (playerInputAction != PlayerInputAction.Shoot || actionState != ActionState.Start)
            {
                return;
            }

            GameObject instance = Spawn();

            IgnoreCollisionsUtil.IgnoreAllCollisions(transform.root.gameObject, instance);
        }

        protected override Vector2 GetSpawnDirectionNormalized()
        {
            return characterController.MoveDirection.GetVectorNormalized();
        }
    }
}

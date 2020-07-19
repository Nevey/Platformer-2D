using Game.Gameplay.Characters.Movement;
using Game.Gameplay.Projectiles;
using Game.Utils;
using UnityEngine;
using CharacterController = Game.Gameplay.Characters.CharacterController;
using Game.DI;
using Game.Gameplay.UserInput;
using Game.Gameplay.Characters;

#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Game.Gameplay.Player
{
    public class PlayerProjectileSpawner : CharacterBasedProjectileSpawner
    {
        [Inject] private PlayerInputController playerInputController;

        protected override void Awake()
        {
            base.Awake();

            playerInputController.ActionEvent += OnAction;
        }

        protected override void OnDestroy()
        {
            playerInputController.ActionEvent -= OnAction;

            base.OnDestroy();
        }

        private void OnAction(PlayerInputAction playerInputAction, ActionState actionState)
        {
            if (playerInputAction != PlayerInputAction.Shoot || actionState != ActionState.Start)
            {
                return;
            }

            Spawn();
        }
    }
}

using Game.Characters;
using Game.Enemies.Behaviours;

namespace Game.Enemies
{
    public class EnemyProjectileSpawner : CharacterBasedProjectileSpawner
    {
        private EnemyAttackBehaviour enemyAttackBehaviour;

        protected override void Awake()
        {
            base.Awake();

            EnemyController enemyController = (EnemyController)characterController;

            enemyAttackBehaviour = enemyController.GetComponent<EnemyAttackBehaviour>();
            enemyAttackBehaviour.FireAttackEvent += OnFireAttack;
        }

        protected override void OnDestroy()
        {
            enemyAttackBehaviour.FireAttackEvent -= OnFireAttack;

            base.OnDestroy();
        }

        private void OnFireAttack()
        {
            Spawn();
        }
    }
}

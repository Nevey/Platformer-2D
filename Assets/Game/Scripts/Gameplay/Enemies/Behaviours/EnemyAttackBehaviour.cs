using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay.Enemies.Behaviours
{
    [RequireComponent(typeof(EnemyTargetBehaviour))]
    public class EnemyAttackBehaviour : MonoBehaviour
    {
        [SerializeField] private float attackInterval = 1.5f;

        private EnemyTargetBehaviour enemyTargetBehaviour;
        private bool isAttacking;
        private Coroutine attackRoutine;

        public event Action FireAttackEvent;

        private void Awake()
        {
            enemyTargetBehaviour = GetComponent<EnemyTargetBehaviour>();
        }

        private IEnumerator AttackRoutine()
        {
            while (isAttacking)
            {
                FireAttackEvent?.Invoke();

                yield return new WaitForSeconds(attackInterval);
            }
        }

        public bool StartAttack()
        {
            if (!enemyTargetBehaviour.HasTarget)
            {
                return false;
            }

            isAttacking = true;
            attackRoutine = StartCoroutine(AttackRoutine());

            return true;
        }

        public void StopAttack()
        {
            isAttacking = false;

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }
        }
    }
}

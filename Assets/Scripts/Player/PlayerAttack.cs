using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private Player _player;
        
        private float _damage;

        public float MaxHP;
        public float CurrentHP;

        public Enemy.Enemy Enemy { get; set; }

        private void Awake()
        {
            _player = GetComponent<Player>();
            
            _damage = 1f;

            if (MaxHP == 0)
            {
                MaxHP = 10;
            }

            if (CurrentHP == 0)
            {
                CurrentHP = MaxHP;
            }
        }
        
        public void Attack()
        {
            StartCoroutine(_Attack());
        }

        IEnumerator _Attack()
        {
            yield return new WaitForSeconds(0.5f);
            if (Enemy)
            {
                Enemy.CauseDamage(_damage, _player);
            }
        }

        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                Die();
            }
            else
            {
                _player.FinishedAttack();
            }
        }

        private void Die()
        {
            CurrentHP = MaxHP;
            _player.UndoFight();
            Enemy.UndoFight();
        }
    }
}
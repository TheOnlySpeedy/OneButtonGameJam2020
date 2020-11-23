using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public float MaxHP;
        public float CurrentHP;
        public float Damage;

        private void Awake()
        {
            if (MaxHP == 0)
            {
                MaxHP = 5f;
            }

            if (CurrentHP == 0)
            {
                CurrentHP = MaxHP;
            }

            if (Damage == 0)
            {
                Damage = 1;
            }
        }

        public void CauseDamage(float damage, Player.Player player)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                player.FinishedAttack();
                Die();
                return;
            }

            StartCoroutine(InflictDamage(player));
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        IEnumerator InflictDamage(Player.Player player)
        {
            yield return new WaitForSeconds(0.5f);
            player.TakeDamage(Damage);
        }

        public void UndoFight()
        {
            CurrentHP = MaxHP;
        }
    }
}
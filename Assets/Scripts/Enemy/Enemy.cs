using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public float MaxHP;
        public float CurrentHP;
        public float Damage;

        private Animator _animator;
        private Player.Player _player;
        private bool _updateLookAt;

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

            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_updateLookAt && _player)
            {
                _updateLookAt = false;
                transform.LookAt(_player.transform);
            }
        }

        public void TakeDamage(float damage, Player.Player player)
        {
            _player = player;
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                player.FinishedAttack();
                Die();
                return;
            }

            _updateLookAt = true;
            StartCoroutine(AttackBack());
        }

        IEnumerator AttackBack()
        {
            yield return new WaitForSeconds(0.5f);
            _animator.Play("Attack");
        }

        public void Attack()
        {
            _player.TakeDamage(Damage);
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public void UndoFight()
        {
            CurrentHP = MaxHP;
        }
    }
}
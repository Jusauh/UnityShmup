using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Player
{
    public class PlayerBullet : MonoBehaviour
    {
        public enum State
        {
            NONE = 0,
            ALIVE = 1,
            HIT = 2
        }

        [SerializeField]
        private float _speed = 20;
        [SerializeField]
        private float _hitBox = 10;
        [SerializeField]
        private float _damage;
        private GameManager _gameManager;
        private State _state;

        public void Init()
        {
            // NOTE Get GameManager without finding, fix later.
            _gameManager = FindObjectOfType<GameManager>();
            if(_gameManager == null)
            {
                Debug.LogError("Couldn't find GameManager");
            }
        }

        private void Awake()
        {
            _state = State.ALIVE;
        }

        private void Update()
        {
            if (_state == State.ALIVE)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + _speed);
                if (_gameManager.Enemies.Count > 0)
                {
                    for (int i = _gameManager.Enemies.Count - 1; i >= 0; i--)
                    {
                        if (Vector2.Distance(_gameManager.Enemies[i].transform.position, transform.position) < _gameManager.Enemies[i].HitBox + _hitBox)
                        {
                            _gameManager.Enemies[i].GetHit(_damage);
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
                if (transform.position.y > GameManager.GAME_AREA_Y + 30)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
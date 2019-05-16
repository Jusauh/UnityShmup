using Shmup.DataStructures;
using Shmup.Identifiers;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Shmup.Behaviour
{
    [Serializable]
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        protected GameManager _gameManager;
        protected Vector2 _position;
        private Vector2 _fromPosition;
        private Vector2 _targetPosition;
        private float _lerp;
        private float _lerpMod;
        private float _lerpAcceleration;
        private float _moveDuration;
        protected float _heath;
        public float Health
        {
            get
            {
                return _heath;
            }
        }
        protected float _hitBox;
        public float HitBox
        {
            get
            {
                return _hitBox;
            }
        }
        protected SpriteRenderer _spriteRenderer;

        protected virtual void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            if(_gameManager == null)
            {
                Debug.LogError("No GameManager available");
            }
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if(_spriteRenderer == null)
            {
                Debug.LogError("No Spriterenderer available");
            }
        }

        protected virtual void Update()
        {
            UpdateMovement();
        }
        
        private void UpdateMovement()
        {
            if (_moveDuration > 0)
            {
                _moveDuration--;
                if (_moveDuration != 0)
                {
                    _lerpMod += _lerpAcceleration;
                    _lerp += _lerpMod;
                }
                else
                {
                    _lerp = 1;
                }
                _position = Vector3.Lerp(_fromPosition, _targetPosition, _lerp);
            }
            transform.position = _position;
        }

        public void MoveSmooth(Vector2 position, float duration)
        {
            _moveDuration = duration;
            _fromPosition = _position;
            _targetPosition = position;
            _lerp = 0;
            _lerpMod = 1f / duration;
            _lerpAcceleration = 0;
        }

        public void MoveAcceleration(Vector2 position, float duration)
        {
            _moveDuration = duration;
            _fromPosition = _position;
            _lerpAcceleration = -(2f / Mathf.Pow(duration,2));
            _lerpMod = Mathf.Abs(_lerpAcceleration * duration);
            _lerp = 0;
            _targetPosition = position;
        }

        public void MoveAccelerationReverse(Vector2 position, float duration)
        {
            _moveDuration = duration;
            _fromPosition = _position;
            _lerpAcceleration = (2f / Mathf.Pow(duration, 2));
            _lerpMod = 0;
            _lerp = 0;
            _targetPosition = position;
        }

        public void MoveRandom(float minX, float minY, float maxX, float maxY, float minDistance, float maxDistance, float duration)
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            float distance = Random.Range(minDistance, maxDistance);
            Vector2 movePosition = new Vector2(_position.x + distance * Mathf.Cos(angle), _position.y + distance * Mathf.Sin(angle));
            while (movePosition.x < minX || movePosition.x > maxX || movePosition.y < minY || movePosition.y > maxY)
            {
                angle = Random.Range(0, Mathf.PI * 2);
                distance = Random.Range(minDistance, maxDistance);
                movePosition = new Vector2(_position.x + distance * Mathf.Cos(angle), _position.y + distance * Mathf.Sin(angle));
            }
            MoveAcceleration(movePosition, duration);
        }

            protected void CreateBullet(Vector2 position, int delay)
        {
            if (_gameManager.GetBullet() != null)
            {
                _gameManager.GetBullet().Create(position, delay);
            }
        }

        protected void SetBulletData(int delay, float? speed, float acceleration, float maxSpeed, float? angle, float rotation, BulletSpriteIdentifier sprite)
        {
            if (_gameManager.GetBullet() != null)
            {
                _gameManager.GetBullet().AddData(new BulletDataContainer(delay, speed, acceleration, maxSpeed, angle, rotation, sprite));
            }
        }

        protected void Shoot()
        {
            _gameManager.FireBullet();
        }

        protected Vector2 GetPosition
        {
            get
            {
                return transform.position;
            }
        }

        protected Vector2 GetPlayerPosition
        {
            get
            {
                return _gameManager.Player.transform.position;
            }
        }

        protected Vector2 PositionAtAngle(Vector2 origo, float radius, float angle)
        {
            return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + origo;
        }

        public virtual void GetHit(float damage)
        {
            _heath -= damage;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        protected virtual void Kill()
        {
            gameObject.SetActive(false);
            _gameManager.RemoveEnemy(this);
        }
    }
}
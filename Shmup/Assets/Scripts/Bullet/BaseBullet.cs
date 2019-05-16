using Shmup.DataStructures;
using Shmup.Identifiers;
using Shmup.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup
{
    public enum SpriteRotation
    {
        NONE = 0,
        ROTATE = 1,
        SPIN = 2
    }

    public enum BulletState
    {
        NONE = 0,
        SPAWNING = 1,
        ALIVE = 2
    }

    [Serializable]
    public class BulletSprite
    {
        public BulletSpriteIdentifier spriteIdentifier;
        public Sprite sprite;
    }

    public class BaseBullet : MonoBehaviour
    {
        GameManager _gameManager;
        protected SpriteRenderer _spriteRenderer;
        [SerializeField]
        protected PoofEffectHandler _destroyEffect;
        [SerializeField]
        protected Scaler _delayEffect;
        [SerializeField]
        protected BulletSprite[] _identifiers;
        protected Dictionary<BulletSpriteIdentifier, Sprite> _spriteDictionary;
        protected List<BulletDataContainer> _bulletDataContainer = new List<BulletDataContainer>();
        protected SpriteRotation _spriteRotation;
        protected BulletState _state;
        protected float _speed;
        protected float _acceleration;
        protected float _targetSpeed;
        protected float _angle;
        protected float _spriteSpinAngle;
        protected float _rotation;
        protected Vector2 _hitBox;
        protected Vector2 _position;
        private int _dataIndex;
        private int _lifeTime;
        private int _delay;

        public void Create()
        {
            _spriteDictionary = new Dictionary<BulletSpriteIdentifier, Sprite>();
            _destroyEffect = Instantiate(_destroyEffect);
            _destroyEffect.Init(0.5f, 1, 8, 1, 0, 4);
            _destroyEffect.gameObject.SetActive(false);
            for (int i = 0; i < _identifiers.Length; i++)
            {
                _spriteDictionary.Add(_identifiers[i].spriteIdentifier, _identifiers[i].sprite);
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("No sprite renderer available");
            }

            _gameManager = FindObjectOfType<GameManager>();
            if (_gameManager == null)
            {
                Debug.LogError("No game manager available");
            }
        }

        public void UpdateBehaviour()
        {
            if(_delay > 0)
            {
                _state = BulletState.SPAWNING;
                _delay--;
            }
            else
            {
                _state = BulletState.ALIVE;
            }

            if (_state == BulletState.ALIVE)
            {
                ReadData();
                UpdateMovement();
                if (CheckCollision)
                {
                    _gameManager.RestBullet(this, true, true);
                }

                _lifeTime++;
            }
        }

        protected void UpdateMovement()
        {
            _angle += _rotation;
            _speed += _acceleration;
            ClampSpeed();
            _position.x += Mathf.Cos(_angle) * _speed;
            _position.y += Mathf.Sin(_angle) * _speed;
            switch (_spriteRotation)
            {
                case SpriteRotation.ROTATE:
                    transform.rotation = Quaternion.AngleAxis(_angle * Mathf.Rad2Deg, Vector3.forward);
                    break;
                case SpriteRotation.SPIN:
                    _spriteSpinAngle += 2f;
                    transform.rotation = Quaternion.AngleAxis(_spriteSpinAngle, Vector3.forward);
                    break;
            }

            transform.position = _position;
            CheckBoundaries();
        }

        protected void ClampSpeed()
        {
            if (_acceleration < 0)
            {
                _speed = Mathf.Clamp(_speed, _targetSpeed, _speed);
            }
            else if (_acceleration > 0)
            {
                _speed = Mathf.Clamp(_speed, _speed, _targetSpeed);
            }
        }

        protected void CheckBoundaries()
        {
            if (_position.x < -GameManager.REST_BOUNDARY || _position.x > GameManager.GAME_AREA_X + GameManager.REST_BOUNDARY ||
               _position.y < -GameManager.REST_BOUNDARY || _position.y > GameManager.GAME_AREA_Y + GameManager.REST_BOUNDARY)
            {
                _gameManager.RestBullet(this, false, false);
            }
        }

        protected void ReadData()
        {
            if (_bulletDataContainer != null && _dataIndex < _bulletDataContainer.Count && _lifeTime == _bulletDataContainer[_dataIndex].Delay)
            {
                BulletDataContainer newData = _bulletDataContainer[_dataIndex];
                if (newData.Speed.HasValue)
                {
                    _speed = newData.Speed.Value;
                }

                if (newData.Angle.HasValue)
                {
                    _angle = newData.Angle.Value * Mathf.Deg2Rad;
                }

                _acceleration = newData.Acceleration;
                _rotation = newData.Rotation * Mathf.Deg2Rad;
                _targetSpeed = newData.MaxSpeed;
                SetSprite(newData.BulletSprite);
                SetHitBox(newData.BulletSprite);
                _dataIndex++;
            }
        }

        public void Create(Vector2 position, int delay)
        {
            _position = position;
            transform.position = _position;
            SetSprite(BulletSpriteIdentifier.NONE);
            _delay = delay;
            _lifeTime = 0;
            _dataIndex = 0;
            _bulletDataContainer.Clear();
            if (delay > 0)
            {
                _delayEffect.Init(1, 0.2f, delay);
            }
        }

        public void AddData(BulletDataContainer bulletData)
        {
            if (_bulletDataContainer == null)
            {
                _bulletDataContainer = new List<BulletDataContainer>();
            }

            _bulletDataContainer.Add(bulletData);
        }

        protected void SetSprite(BulletSpriteIdentifier identifier)
        {
            switch (identifier)
            {
                case BulletSpriteIdentifier.OVAL_SMALL:
                case BulletSpriteIdentifier.OVAL_LARGE:
                    _spriteRotation = SpriteRotation.ROTATE;
                    break;
                case BulletSpriteIdentifier.STAR_SMALL:
                case BulletSpriteIdentifier.STAR_LARGE:
                    _spriteRotation = SpriteRotation.SPIN;
                    break;
                default:
                    _spriteRotation = SpriteRotation.NONE;
                    break;
            }
            if(identifier == BulletSpriteIdentifier.NONE)
            {
                _spriteRenderer.sprite = null;
                return;
            }
            _spriteRenderer.sprite = _spriteDictionary[identifier];
        }

        protected void SetHitBox(BulletSpriteIdentifier identifier)
        {
            switch (identifier)
            {
                case BulletSpriteIdentifier.ROUND_SMALL:
                    _hitBox.Set(8, 8);
                    break;
                case BulletSpriteIdentifier.ROUND_MEDIUM:
                    _hitBox.Set(18, 18);
                    break;
                case BulletSpriteIdentifier.ROUND_LARGE:
                    _hitBox.Set(26, 26);
                    break;
                case BulletSpriteIdentifier.OVAL_SMALL:
                    _hitBox.Set(22, 12);
                    break;
                case BulletSpriteIdentifier.OVAL_LARGE:
                    _hitBox.Set(33, 17);
                    break;
                case BulletSpriteIdentifier.STAR_SMALL:
                    _hitBox.Set(11, 11);
                    break;
                case BulletSpriteIdentifier.STAR_LARGE:
                    _hitBox.Set(19, 19);
                    break;
                default:
                    _hitBox = Vector2.zero;
                    break;
            }
        }

        private bool CheckCollision
        {
            get
            {
                if (_hitBox.x == _hitBox.y)
                {
                    return Vector3.Distance(_gameManager.Player.transform.position, _position) <= _hitBox.x / 2;
                }
                else
                {
                    float checkAngle = MathHelper.GetAngleTo(_position, _gameManager.Player.transform.position) * Mathf.Deg2Rad - _angle;
                    float distance = ((_hitBox.y / 2) * (_hitBox.x / 2)) / Mathf.Sqrt((Mathf.Pow((_hitBox.x / 2) * Mathf.Sin(checkAngle), 2)) + (Mathf.Pow((_hitBox.y / 2) * Mathf.Cos(checkAngle), 2)));
                    return Vector3.Distance(_position, _gameManager.Player.transform.position) <= distance;
                }
            }
        }

        public void Poof()
        {
            _destroyEffect.transform.position = _position;
            _destroyEffect.Activate();
        }
    }
}
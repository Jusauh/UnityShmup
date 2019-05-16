using Shmup.Identifiers;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Player
{
    public class PlayerHandler : MonoBehaviour
    {
        private enum FocusState
        {
            NONE = 0,
            FOCUSED = 1,
            NOT_FOCUSED = 2
        }

        [SerializeField]
        private GameObject _hitBox;
        [SerializeField]
        private PlayerFamiliar _familiarRef;
        private List<PlayerFamiliar> _familiars = new List<PlayerFamiliar>();
        [SerializeField]
        private PlayerBullet _bulletRef;
        private Queue<PlayerBullet> _playerBullets = new Queue<PlayerBullet>();
        [SerializeField]
        private PlayerBullet _familiarBulletRef;
        private Queue<PlayerBullet> _familiarBullets = new Queue<PlayerBullet>();
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _focusSpeed;
        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }
        private Vector2 _velocity;
        private int _power;
        private int _maxPower = 4;
        private float _familiarDistance = 50;
        [SerializeField]
        private int _fireRate = 5;
        private int _fireAt;
        [SerializeField]
        private int _familiarFireRate = 7;
        private int _familiarFireAt;
        private float _familiarAngle;
        private FocusState _focusState;
        private GameManager _gameManager;

        private void Awake()
        {
            for(int i = 0; i < _maxPower; i++)
            {
                _familiars.Add(Instantiate(_familiarRef, transform).GetComponent<PlayerFamiliar>());
                _familiars[i].Init();
                _familiars[i].gameObject.SetActive(false);
            }
            for(int i = 0; i < 10; i++)
            {
                PlayerBullet bullet = Instantiate(_bulletRef);
                bullet.Init();
                bullet.gameObject.SetActive(false);
                _playerBullets.Enqueue(bullet);
            }
            for (int i = 0; i < 40; i++)
            {
                PlayerBullet bullet = Instantiate(_familiarBulletRef);
                bullet.Init();
                bullet.gameObject.SetActive(false);
                _familiarBullets.Enqueue(bullet);
            }
            SetPower(4);
            _position = transform.position;
            _focusState = FocusState.NOT_FOCUSED;
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            UpdateMovement();
            UpdateFamiliars();
        }

        private void UpdateMovement()
        {
            _velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _velocity.y--;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _velocity.y++;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _velocity.x--;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _velocity.x++;
            }

            if(Input.GetKey(KeyCode.Z))
            {
                Shoot();
            }

            if(Input.GetKey(KeyCode.LeftShift))
            {
                _focusState = FocusState.FOCUSED;
            }
            else
            {
                _focusState = FocusState.NOT_FOCUSED;
            }

            if (_focusState == FocusState.FOCUSED)
            {
                _velocity = _velocity.normalized * _focusSpeed;
                _hitBox.SetActive(true);
                if(_familiarDistance > 20)
                {
                    _familiarDistance -= 3.5f;
                }
            }
            else
            {
                _velocity = _velocity.normalized * _speed;
                _hitBox.SetActive(false);
                if (_familiarDistance < 50)
                {
                    _familiarDistance += 3.5f;
                }
            }

            _position += _velocity;
            _position.x = Mathf.Clamp(_position.x, 20, 330 - 20);
            _position.y = Mathf.Clamp(_position.y, 26, 440 - 26);

            transform.position = _position;
        }

        private void Shoot()
        {
            if(_fireAt < Time.frameCount)
            {
                if (!_playerBullets.Peek().gameObject.activeSelf)
                {
                    _playerBullets.Peek().transform.position = transform.position;
                    _playerBullets.Peek().gameObject.SetActive(true);
                    _playerBullets.Enqueue(_playerBullets.Dequeue());
                }
                else
                {
                    Debug.LogWarning("PlayerBullet cache too small!");
                }
                _fireAt = Time.frameCount + _fireRate;
            }
            if (_familiarFireAt < Time.frameCount)
            {
                for (int i = 0; i < _power; i++)
                {
                    if (!_familiarBullets.Peek().gameObject.activeSelf)
                    {
                        _familiarBullets.Peek().transform.position = _familiars[i].transform.position;
                        _familiarBullets.Peek().gameObject.SetActive(true);
                        _familiarBullets.Enqueue(_familiarBullets.Dequeue());
                    }
                    else
                    {
                        Debug.LogWarning("FamiliarBullet cache too small!");
                    }
                }
                _familiarFireAt = Time.frameCount + _familiarFireRate;
            }
        }

        private void UpdateFamiliars()
        {
            for(int i = 0; i < _power; i++)
            {
                _familiars[i].transform.localPosition = new Vector3(Mathf.Cos(_familiarAngle * Mathf.Deg2Rad) * _familiarDistance, Mathf.Sin(_familiarAngle * Mathf.Deg2Rad) * _familiarDistance);
                _familiarAngle += (360f / _power);
            }
            _familiarAngle += 7.5f;
        }

        private void SetPower(int power)
        {
            _power = power;
            for (int i = 0; i < _maxPower; i++)
            {
                if(i < power)
                {
                    _familiars[i].gameObject.SetActive(true);
                }
                else
                {
                    _familiars[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

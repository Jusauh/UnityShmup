using Shmup.DataStructures;
using Shmup.Identifiers;
using UnityEngine;
using System.Collections;
using Shmup.Math;

namespace Shmup.Behaviour
{
    public class Behaviour03 : BaseEnemyBehaviour
    {
        private int _frame = 0;
        private int _frame2 = 0;
        private int _mFrame = 0;
        private float _rainWidth;
        private float _angle = 0;
        private float _rotation = 0;
        private float _heathTreshold;
        private int _intensitiy = 0;
        private Vector2 _velocity;

        [SerializeField]
        private GameObject _charge;

        public void Init(Vector3 position)
        {
            _position = position;
            transform.position = _position;
            _velocity = new Vector2(0, -3);
        }

        protected override void Awake()
        {
            base.Awake();
            _hitBox = 30;
            _heath = 2000;
            _heathTreshold = 1700;
            _rotation = 0.8f;
        }

        protected override void Update()
        {
            _frame++;
            _mFrame++;
            _frame2++;

            if (_frame == 40)
            {
                _angle = Random.Range(0, 360);
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_BIG, AudioPlayAction.RESTART);
                for(int i = 0; i < 18 + _intensitiy * 2; i++)
                {
                    CreateBullet(GetPosition, 0);
                    SetBulletData(0, 3, 0, 0, _angle, _rotation, BulletSpriteIdentifier.ROUND_LARGE);
                    Shoot();
                    CreateBullet(GetPosition, 6);
                    SetBulletData(0, 3, 0, 0, _angle, _rotation, BulletSpriteIdentifier.ROUND_MEDIUM);
                    Shoot();
                    CreateBullet(GetPosition, 10);
                    SetBulletData(0, 3, 0, 0, _angle, _rotation, BulletSpriteIdentifier.ROUND_SMALL);
                    Shoot();
                    _angle += 360f / (18f + _intensitiy * 2);
                }
                _frame = 15;
                _rotation *= -1;
            }

            if (_frame2 == 10)
            {
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_SMALL, AudioPlayAction.RESTART);
                for (int i = 0; i < 12; i++)
                {
                    CreateBullet(GetPosition + new Vector2(Mathf.Cos(Mathf.Deg2Rad * ((360f / 12) * i + 15)) * 40, Mathf.Sin(Mathf.Deg2Rad * ((360f / 12) * i + 15)) * 40), 20);
                    SetBulletData(0, 7, 0, 0, 360f / 12 * i + 15, 0, BulletSpriteIdentifier.OVAL_LARGE);
                    Shoot();
                }
                _frame2 = _intensitiy;
            }

            if(_heath < _heathTreshold)
            {
                _gameManager.PlayAudio(AudioIdentifier.WARP, AudioPlayAction.RESTART);
                _heathTreshold = _heathTreshold - 350;
                _intensitiy++;
                Instantiate(_charge, transform);
            }

            if (_mFrame < 40)
            {
                _position += _velocity;
            }
            else if (_mFrame > 12000)
            {
                _position -= _velocity;
                if (_mFrame > 1240)
                {
                    Kill();
                }
            }

            if (_heath <= 0)
            {
                _gameManager.PlayAudio(AudioIdentifier.DESTROY, AudioPlayAction.RESTART);
                _gameManager.ClearBullets(true);
                Kill();
            }
            base.Update();
        }
    }
}
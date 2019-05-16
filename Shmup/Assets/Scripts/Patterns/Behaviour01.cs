using Shmup.DataStructures;
using Shmup.Identifiers;
using UnityEngine;
using System.Collections;
using Shmup.Math;

namespace Shmup.Behaviour
{
    public class Behaviour01 : BaseEnemyBehaviour
    {
        private int _frame = 0;
        private int _mFrame = 0;
        private float _rainWidth;
        private Vector2 _velocity;

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
            _heath = 1000;
        }

        protected override void Update()
        {
            _frame++;
            _mFrame++;

            if(_frame == 40)
            {
                _gameManager.PlayAudio(AudioIdentifier.SHOOT, AudioPlayAction.RESTART);
                CreateBullet(new Vector2(GetPosition.x + _rainWidth, 400 + Random.Range(-10,20)), 20);
                SetBulletData(0, 1, 0.03f, 3.3f, Random.Range(-80, -100), Random.Range(-0.05f, 0.05f), BulletSpriteIdentifier.OVAL_SMALL);
                Shoot();
                _rainWidth += Random.Range(7,15);
                CreateBullet(new Vector2(GetPosition.x + -_rainWidth, 400 + Random.Range(-10, 20)), 20);
                SetBulletData(0, 1, 0.03f, 3.3f, Random.Range(-80, -100), Random.Range(-0.05f, 0.05f), BulletSpriteIdentifier.OVAL_SMALL);
                Shoot();
                _rainWidth += Random.Range(15, 20);
                if (_rainWidth > 200)
                {
                    _rainWidth -= 200;
                }
                _frame = 35;
            }

            if (_mFrame < 40)
            {
                _position += _velocity;
            }
            else if (_mFrame > 600)
            {
                _position -= _velocity;
                if (_mFrame > 640)
                {
                    Kill();
                }
            }

            if (_heath <= 0)
            {
                _gameManager.PlayAudio(AudioIdentifier.DESTROY, AudioPlayAction.RESTART);
                Kill();
            }
            base.Update();
        }
    }
}
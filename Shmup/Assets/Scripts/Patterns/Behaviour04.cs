using Shmup.DataStructures;
using Shmup.Identifiers;
using UnityEngine;
using System.Collections;
using Shmup.Math;

namespace Shmup.Behaviour
{
    public class Behaviour04 : BaseEnemyBehaviour
    {
        private int _frame = 0;
        private int _mFrame = 0;
        private float _mAngle = 90;
        private float _mAngleMod = 2;
        private float _rainWidth;
        private Vector2 _velocity;

        public void Init(Vector3 position, float angle, int frame)
        {
            _position = position;
            transform.position = _position;
            _mAngle = angle;
            _frame = frame;
            if(position.x < 165)
            {
                _mAngleMod = -2;
            }
            else
            {
                _mAngleMod = 2;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _hitBox = 16;
            _heath = 10;
        }

        protected override void Update()
        {
            _frame++;
            _mFrame++;

            if(_frame > 30 && _frame % 2 == 0)
            {
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_SMALL, AudioPlayAction.RESTART,1,(_position.x - 165) / 165f);
                for (int i = 0; i < 4; i++)
                {
                    CreateBullet(GetPosition, 5);
                    SetBulletData(0, 0, 0.05f, 1.7f, (360 / 4) * i, 0, BulletSpriteIdentifier.ROUND_SMALL);
                    Shoot();
                }
            }
            if(_frame > 44)
            {
                _frame = 16;
            }

            _velocity = new Vector2(Mathf.Cos(_mAngle * Mathf.Deg2Rad) * 3, Mathf.Sin(_mAngle * Mathf.Deg2Rad) * 3);
            _position += _velocity;

            if(_position.y > 330)
            {
                _mAngle += _mAngleMod;
            }
            _mAngle = Mathf.Clamp(_mAngle, 0, 180);

            if (_heath <= 0 || _position.x < -50 || _position.x > 380)
            {
                if(_heath <= 0)
                {
                    _gameManager.PlayAudio(AudioIdentifier.DESTROY, AudioPlayAction.RESTART);
                }
                Kill();
            }
            base.Update();
        }

        protected override void Kill()
        {
            for (int i = 0; i < 20; i++)
            {
                CreateBullet(GetPosition, 4);
                SetBulletData(0, 0, 0.4f, 5, MathHelper.GetAngleTo(GetPosition, GetPlayerPosition) + (360f / 20f) * i, 0, BulletSpriteIdentifier.ROUND_SMALL);
                Shoot();
                CreateBullet(GetPosition, 6);
                SetBulletData(0, 0, 0.4f, 5, MathHelper.GetAngleTo(GetPosition, GetPlayerPosition) + (360f / 20f) * i + 1, 0, BulletSpriteIdentifier.ROUND_SMALL);
                Shoot();
                CreateBullet(GetPosition, 6);
                SetBulletData(0, 0, 0.4f, 5, MathHelper.GetAngleTo(GetPosition, GetPlayerPosition) + (360f / 20f) * i - 1, 0, BulletSpriteIdentifier.ROUND_SMALL);
                Shoot();
            }
            base.Kill();
        }
    }
}
using Shmup.Identifiers;
using Shmup.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Behaviour.Boss
{
    public class BossBehaviour02 : BaseEnemyBehaviour
    {
        private int _frame = 0;
        private int _frame2 = 0;
        private int _mFrame = 0;
        private float _angle = 0;
        private float _angle2 = 0;
        private float _radius = 0;

        private Vector2 _warpPosition;
        private float _scaler;
        private float _scaleMod = 0.2f;

        protected override void Awake()
        {
            base.Awake();
            _heath = 2400;
            _position = transform.position;
            MoveAcceleration(new Vector2(160, 300),40);
        }

        protected override void Update()
        {
            _frame++;
            _frame2++;
            _mFrame++;

            if(_frame == 60)
            {
                for (int i = 0; i < 4; i++)
                {
                    CreateBullet(PositionAtAngle(GetPosition, Mathf.Sin(_radius) * 120, _angle), 20);
                    SetBulletData(0, 0, 0.04f, 2.4f, _angle + _angle2, 0, BulletSpriteIdentifier.STAR_SMALL);
                    Shoot();
                    _angle += 360f / 4;
                }
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_SMALL, AudioPlayAction.RESTART);
                _angle2 += 1.845f;
                _angle += 7.21f;
                _radius += 0.09f;
                _frame = 58;
            }

            if(_frame2 == 200)
            {
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_BIG, AudioPlayAction.RESTART);
                for (int i = 0; i < 30; i++)
                {
                    CreateBullet(PositionAtAngle(GetPlayerPosition, 60, (360f/30f)*i), 5);
                    SetBulletData(0, -0.05f, -0.001f, -1, (360f / 30f) * i, 0, BulletSpriteIdentifier.STAR_LARGE);
                    SetBulletData(150, null, 0.2f, 10, null, 0, BulletSpriteIdentifier.STAR_LARGE);
                    Shoot();
                }
                _frame2 = -150;
            }

            WarpUpdate();
            base.Update();
        }

        private void Warp(Vector2 position)
        {
            _scaleMod *= -1;
            _warpPosition = position;
            _gameManager.PlayAudio(AudioIdentifier.WARP, AudioPlayAction.RESTART);
        }

        private void WarpUpdate()
        {
            _scaler += _scaleMod;
            if (_scaler <= 0 && _scaleMod < 0)
            {
                _position = _warpPosition;
                _scaleMod *= -1;
            }
            _scaler = Mathf.Clamp(_scaler, 0, 1);
            transform.localScale = new Vector3(_scaler, 2 - _scaler);
        }
    }
}
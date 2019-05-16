using Shmup.Identifiers;
using Shmup.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Behaviour.Boss
{
    public class BossBehaviour01 : BaseEnemyBehaviour
    {
        private int _frame = 0;
        private int _frame2 = 0;
        private int _mFrame = 0;
        private float _angle = 0;
        private float _speed = 0;

        private Vector2 _warpPosition;
        private float _scaler;
        private float _scaleMod = 0.2f;

        protected override void Awake()
        {
            base.Awake();
            _heath = 1900;
            _position = new Vector2(-100, -100);
            Warp(new Vector2(165, 340));
        }

        protected override void Update()
        {
            _frame++;
            _frame2++;
            _mFrame++;

            if (_mFrame == 5)
            {
                for(int i = 0; i < 30; i++)
                {
                    CreateBullet(GetPosition, 5);
                    SetBulletData(0,0,0.2f,2.4f,(360/30)*i,0,BulletSpriteIdentifier.STAR_LARGE);
                    Shoot();
                    CreateBullet(GetPosition, 5);
                    SetBulletData(0, 0, 0.04f, 2.4f, (360 / 30) * i + 6, 0, BulletSpriteIdentifier.STAR_LARGE);
                    Shoot();
                }
            }

            if(_frame == 60)
            {
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_SMALL, AudioPlayAction.RESTART);
                for(int i = 0; i < 10; i++)
                {
                    CreateBullet(new Vector2(50,400), 20);
                    SetBulletData(0, 2.2f + _speed, 0.05f, 3 + _speed, _angle, 0, BulletSpriteIdentifier.STAR_SMALL);
                    Shoot();
                    CreateBullet(new Vector2(280, 400), 20);
                    SetBulletData(0, 2.2f + _speed, 0.05f, 3 + _speed, -_angle + 20, 0, BulletSpriteIdentifier.STAR_SMALL);
                    Shoot();
                    _angle += 360f / 10f;
                }
                _angle += 1.34f;
                _speed += 0.1f;
                if(_speed > 0.8f)
                {
                    _speed = 0;
                    _angle += 18f;
                }
                _frame = 54;
            }

            if(_frame2 == 90)
            {
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_BIG, AudioPlayAction.RESTART);
                for (int i = 0; i < 30; i++)
                {
                    CreateBullet(GetPosition + new Vector2(0,26), 7);
                    SetBulletData(0, 0, 0.2f, 2.4f, (360 / 30) * i + MathHelper.GetAngleTo(GetPosition,GetPlayerPosition), 0, BulletSpriteIdentifier.STAR_LARGE);
                    Shoot();
                }
                _frame2 = 20;
            }

            if(_mFrame == 120)
            {
                MoveRandom(100,280,230,360,30,50,40);
                _mFrame = 30;
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
            transform.localScale = new Vector3(_scaler, 2- _scaler);
        }
    }
}
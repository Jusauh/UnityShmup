using Shmup.Identifiers;
using Shmup.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Behaviour.Boss
{
    public class BossBehaviour04 : BaseEnemyBehaviour
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
            MoveAcceleration(new Vector2(160, 350), 40);
        }

        protected override void Update()
        {
            _frame++;
            _frame2++;
            _mFrame++;
            if (_frame == 60)
            {
                _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_SMALL, AudioPlayAction.RESTART);
                for (int i = 0; i < 3; i++)
                {
                    Star(GetPosition, 60, _angle + (360f / 3f) * i, 12, 2.5f);
                }

                _angle += 72.45f;
                _frame = 44;
            }

            if (_mFrame == 120)
            {
                MoveRandom(130, 330, 200, 380, 10, 20, 40);
                _mFrame = 30;
            }

            WarpUpdate();
            base.Update();
        }

        private void Star(Vector2 origo, float radius, float angle, float density, float speed)
        {
            float currentAngle = angle;
            float moveAngle = Random.Range(0, 360);
            Vector2 createPosition;
            for (int i = 0; i < density * 3; i++)
            {
                createPosition = PositionAtAngle(origo, radius, currentAngle);
                CreateBullet(createPosition, 4);
                SetBulletData(0, 0, 0.05f, speed, angle, 0, BulletSpriteIdentifier.ROUND_SMALL);
                Shoot();
                currentAngle += 360f / (density * 3);
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < density; j++)
                {
                    createPosition = Vector2.Lerp(PositionAtAngle(origo, radius, currentAngle), PositionAtAngle(origo, radius, currentAngle + 120), j / density);
                    CreateBullet(createPosition, 4);
                    SetBulletData(0, 0, 0.05f, speed, angle, 0, BulletSpriteIdentifier.ROUND_SMALL);
                    Shoot();
                    moveAngle += 360f / 40f;
                }
                currentAngle += 120;
                if (i == 2)
                {
                    currentAngle += 60;
                }
            }
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
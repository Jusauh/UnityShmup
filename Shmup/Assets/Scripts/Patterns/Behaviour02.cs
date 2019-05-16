using Shmup.DataStructures;
using Shmup.Identifiers;
using UnityEngine;
using System.Collections;
using Shmup.Math;

namespace Shmup.Behaviour
{
    public class Behaviour02 : BaseEnemyBehaviour
    {
        private int _frame = 0;
        private int _frame2 = 0;
        private int _mFrame = 0;
        private int _delay = 0;
        private Vector2 _velocity;

        public void Init(Vector3 position, int delay)
        {
            _position = position;
            transform.position = _position;
            _delay = delay;
            if (position.x < 0)
            {
                _velocity = new Vector2(3,0);
            }
            else if (position.x > GameManager.GAME_AREA_X)
            {
                _velocity = new Vector2(-3, 0);
            }
            else if(position.y < 0)
            {
                _velocity = new Vector2(0, 3);
            }
            else if(position.y > GameManager.GAME_AREA_Y)
            {
                _velocity = new Vector2(0, -3);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _hitBox = 30;
            _heath = 200;
        }

        protected override void Update()
        {
            _delay--;
            if (_delay < 0)
            {
                _frame++;
                _frame2++;
                _mFrame++;

                if (_frame == 40 && _frame2 < 120)
                {
                    _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_SMALL, AudioPlayAction.RESTART);
                    float angle = MathHelper.GetAngleTo(GetPosition, GetPlayerPosition);
                    for (int i = 0; i < 2; i++)
                    {
                        CreateBullet(new Vector2(GetPosition.x + Mathf.Cos((angle - 90) * Mathf.Deg2Rad) * 70, GetPosition.y + Mathf.Sin((angle - 90) * Mathf.Deg2Rad) * 50), 10);
                        SetBulletData(0, 5, 0.04f, 6, angle, 0, BulletSpriteIdentifier.OVAL_LARGE);
                        Shoot();
                        CreateBullet(new Vector2(GetPosition.x + Mathf.Cos((angle + 90) * Mathf.Deg2Rad) * 70, GetPosition.y + Mathf.Sin((angle + 90) * Mathf.Deg2Rad) * 50), 10);
                        SetBulletData(0, 5, 0.04f, 6, angle, 0, BulletSpriteIdentifier.OVAL_LARGE);
                        Shoot();
                        angle += 180;
                    }
                    _frame = 35;
                }

                if (_frame2 == 80)
                {
                    _gameManager.PlayAudio(AudioIdentifier.ENEMY_SHOOT_BIG, AudioPlayAction.RESTART);
                    float angle = MathHelper.GetAngleTo(GetPosition, GetPlayerPosition);
                    for (int i = 0; i < 3; i++)
                    {
                        CreateBullet(GetPosition, 10);
                        SetBulletData(0, 6 + i * 0.5f, 0, 0, angle, 0, BulletSpriteIdentifier.ROUND_LARGE);
                        Shoot();
                    }
                }

                if (_mFrame < 40)
                {
                    _position += _velocity;
                }
                else if (_mFrame > 150)
                {
                    _position -= _velocity;
                    if (_mFrame > 210)
                    {
                        Kill();
                    }
                }
                if (_velocity.x < 0)
                {
                    _spriteRenderer.flipX = true;
                }
                else if (_velocity.x > 0)
                {
                    _spriteRenderer.flipX = false;
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
}
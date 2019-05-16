using Shmup.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Stage
{
    public class Stage : MonoBehaviour
    {
        private int _frame;
        private int _phase = 0;
        [SerializeField]
        GameManager _gameManager;
        [SerializeField]
        BehaviourManager _boss;
        [SerializeField]
        Behaviour02 _enemyLargeTunnel;
        [SerializeField]
        Behaviour01 _enemyLargeRain;
        [SerializeField]
        Behaviour03 _enemyLargeLast;
        [SerializeField]
        Behaviour04 _enemySmallRipRound;

        private void Update()
        {
            _frame++;
            if(_phase == 0)
            {
                PhaseEdgeSkulls();
            }
            if(_phase == 1)
            {
                PhaseRain();
            }
            if(_phase == 2)
            {
                PhaseLast();
            }
            if(_phase == 3)
            {
                PhaseLargeSolo();
            }
            if (_phase == 4)
            {
                Boss();
            }
        }

        private void Boss()
        {
            if(_frame == 90)
            {
                _gameManager.Enemies.Add(Instantiate(_boss));
            }
        }

        private void PhaseEdgeSkulls()
        {
            if (_frame >= 40 && _frame % 40 == 0 && _frame <= 1000)
            {
                if (_frame % 80 == 0)
                {
                    Behaviour04 enemy = Instantiate(_enemySmallRipRound);
                    enemy.Init(new Vector3(20, -30), 90, 0);
                    _gameManager.Enemies.Add(enemy);
                }
                else
                {
                    Behaviour04 enemy = Instantiate(_enemySmallRipRound);
                    enemy.Init(new Vector3(310, -30), 90, 16);
                    _gameManager.Enemies.Add(enemy);
                }
            }
            if(_frame == 1100)
            {
                NextPhase();
            }
        }

        private void PhaseLargeSolo()
        {
            if(_frame == 40)
            {
                Behaviour03 enemy = Instantiate(_enemyLargeLast);
                enemy.Init(new Vector3(165, 470));
                _gameManager.Enemies.Add(enemy);
            }
            if(_frame > 40 && _gameManager.Enemies.Count == 0)
            {
                NextPhase();
            }
        }

        private void PhaseRain()
        {
            if(_frame == 1)
            {
                Behaviour01 enemy = Instantiate(_enemyLargeRain);
                enemy.Init(new Vector3(165,470));
                _gameManager.Enemies.Add(enemy);
            }
            else if (_frame == 700)
            {
                Behaviour01 enemy = Instantiate(_enemyLargeRain);
                enemy.Init(new Vector3(165, 470));
                _gameManager.Enemies.Add(enemy);
            }
            else if (_frame == 1400)
            {
                Behaviour01 enemy = Instantiate(_enemyLargeRain);
                enemy.Init(new Vector3(165, 470));
                _gameManager.Enemies.Add(enemy);
            }

            if(_frame >= 120 && _frame % 120 == 0 && _frame <= 1920)
            {
                if(_frame % 240 == 0)
                {
                    Behaviour04 enemy = Instantiate(_enemySmallRipRound);
                    enemy.Init(new Vector3(20, -30), 90, 16);
                    _gameManager.Enemies.Add(enemy);
                }
                else
                {
                    Behaviour04 enemy = Instantiate(_enemySmallRipRound);
                    enemy.Init(new Vector3(310, -30), 90, 0);
                    _gameManager.Enemies.Add(enemy);
                }
            }

            else if(_frame == 2100)
            {
                NextPhase();
            }
        }

        private void PhaseLast()
        {
            if (_frame == 60)
            {
                for (int i = 0; i < 7; i++)
                {
                    Behaviour02 enemy = Instantiate(_enemyLargeTunnel);
                    enemy.Init(new Vector3(-85, 40 + 60 * i), i * 5);
                    _gameManager.Enemies.Add(enemy);
                }
            }
            else if (_frame == 200)
            {
                for (int i = 0; i < 7; i++)
                {
                    Behaviour02 enemy = Instantiate(_enemyLargeTunnel);
                    enemy.Init(new Vector3(330 + 85, 40 + 60 * i), i * 5);
                    _gameManager.Enemies.Add(enemy);
                }
            }
            else if (_frame == 340)
            {
                for (int i = 0; i < 5; i++)
                {
                    Behaviour02 enemy = Instantiate(_enemyLargeTunnel);
                    enemy.Init(new Vector3(45 + 60 * i, -85), i * 5);
                    _gameManager.Enemies.Add(enemy);
                }
            }
            else if (_frame == 480)
            {
                for (int i = 0; i < 5; i++)
                {
                    Behaviour02 enemy = Instantiate(_enemyLargeTunnel);
                    enemy.Init(new Vector3(45 + 60 * i, 440 + 85), i * 5);
                    _gameManager.Enemies.Add(enemy);
                }
            }
            else if (_frame == 600)
            {
                NextPhase();
            }
        }

        private void NextPhase()
        {
            _frame = 0;
            _phase++;
        }
    }
}

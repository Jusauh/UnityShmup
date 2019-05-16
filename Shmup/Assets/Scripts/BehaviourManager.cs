using Shmup.Behaviour;
using Shmup.Behaviour.Boss;
using Shmup.Identifiers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shmup
{
    class BehaviourManager : BaseEnemyBehaviour
    {
        private List<BaseEnemyBehaviour> _behaviours = new List<BaseEnemyBehaviour>();
        private int _timer = 0;
        private int _currentBehaviour = 0;

        protected override void Awake()
        {
            _hitBox = 40;
            base.Awake();
            _behaviours.Add(gameObject.AddComponent<BossBehaviour01>());
            _behaviours.Add(gameObject.AddComponent<BossBehaviour02>());
            _behaviours.Add(gameObject.AddComponent<BossBehaviour03>());
            _behaviours.Add(gameObject.AddComponent<BossBehaviour04>());

            foreach (BaseEnemyBehaviour behaviour in _behaviours)
            {
                behaviour.enabled = false;
            }

            _behaviours[0].enabled = true;
        }

        protected override void Update()
        {
            if(_behaviours[_currentBehaviour].Health <= 0)
            {
                _behaviours[_currentBehaviour].enabled = false;
                _currentBehaviour++;
                if (_currentBehaviour < _behaviours.Count)
                {
                    _behaviours[_currentBehaviour].enabled = true;
                }
                else
                {
                    _gameManager.PlayAudio(AudioIdentifier.DESTROY, AudioPlayAction.RESTART);
                    Kill();
                }
                _gameManager.ClearBullets(true);
            }
        }

        public override void GetHit(float damage)
        {
            _behaviours[_currentBehaviour].GetHit(damage);
        }
    }
}

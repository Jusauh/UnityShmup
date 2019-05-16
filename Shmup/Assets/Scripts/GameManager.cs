using Shmup.Behaviour;
using Shmup.Identifiers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shmup
{
    public class GameManager : MonoBehaviour
    {
        public const int GAME_AREA_X = 330;
        public const int GAME_AREA_Y = 440;
        public const int REST_BOUNDARY = 30;

        private int _hits;
        private Queue<BaseBullet> _bulletCache;
        [SerializeField]
        private AudioHandler _audioHandler;
        [SerializeField]
        private GameObject _baseBullet;
        [SerializeField]
        private GameObject _player;
        public GameObject Player
        {
            get
            {
                return _player;
            }
        }
        private List<BaseEnemyBehaviour> _enemies = new List<BaseEnemyBehaviour>();
        public List<BaseEnemyBehaviour> Enemies
        {
            get
            {
                return _enemies;
            }
        }
        private List<BaseBullet> _activeBullets = new List<BaseBullet>();

        private void Update()
        {
            for(int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                _activeBullets[i].UpdateBehaviour();
            }
        }

        void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            _bulletCache = new Queue<BaseBullet>();
            for (int i = 0; i < 5000; i++)
            {
                BaseBullet bullet = Instantiate(_baseBullet, transform).GetComponent<BaseBullet>();
                bullet.Create();
                bullet.GetComponent<SpriteRenderer>().sortingOrder = i;
                _bulletCache.Enqueue(bullet);
            }
        }

        public void RemoveEnemy(BaseEnemyBehaviour enemy)
        {
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }

        public BaseBullet GetBullet()
        {
            return _bulletCache.Peek();
        }

        public void FireBullet()
        {
            if (_bulletCache != null && _bulletCache.Count > 0)
            {
                _activeBullets.Add(_bulletCache.Peek());
                _bulletCache.Enqueue(_bulletCache.Dequeue());
            }
        }

        public void RestBullet(BaseBullet bullet, bool hitPlayer, bool showPoof)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = null;
            _activeBullets.Remove(bullet);
            bullet.Poof();
            if (hitPlayer)
            {
                _hits++;
            }
        }

        public void ClearBullets(bool showPoof)
        {
            for(int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                RestBullet(_activeBullets[i], false, showPoof);
            }
        }

        public void PlayAudio(AudioIdentifier id, AudioPlayAction action, float volume = 1, float pan = 0, int priority = 128)
        {
            _audioHandler.PlayAudio(id, action, volume, pan, priority);
        }
    }
}
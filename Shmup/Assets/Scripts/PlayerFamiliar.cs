using Shmup.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Player
{
    public class PlayerFamiliar : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private PlayerHandler _playerHandler;

        private void Awake()
        {

        }

        private void Update()
        {

        }

        public void Init()
        {
            SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();
            PlayerHandler playerHandler = GetComponentInParent<PlayerHandler>();
        }
    }
}

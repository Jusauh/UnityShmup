using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Identifiers
{
    [Serializable]
    public enum AudioIdentifier
    {
        NONE = 0,
        SHOOT = 1,
        HIT = 2,
        DESTROY = 3,
        WARP = 4,
        ENEMY_SHOOT_BIG = 5,
        ENEMY_SHOOT_SMALL = 6
    }
}
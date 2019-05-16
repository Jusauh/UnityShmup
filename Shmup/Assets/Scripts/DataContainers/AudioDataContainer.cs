using Shmup.Identifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.DataStructures
{
    [Serializable]
    public struct AudioDataContainer
    {
        public AudioClip AudioClip;
        public AudioIdentifier Id;
    }
}

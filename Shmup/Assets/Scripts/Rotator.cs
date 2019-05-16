using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup.Misc
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        private float _angle;
        void Update()
        {
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            _angle += _speed;
        }
    }
}

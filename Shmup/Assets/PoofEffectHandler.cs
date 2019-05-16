using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup
{
    public class PoofEffectHandler : MonoBehaviour
    {
        [SerializeField]
        private Scaler _circle;
        [SerializeField]
        private Scaler _ball;
        private float _circleStart;
        private float _circleEnd;
        private float _circleDuration;
        private float _ballStart;
        private float _ballEnd;
        private float _ballDuration;

        public void Init(float cStart, float cEnd, float cDuration, float bStart, float bEnd, float bDuration)
        {
            _circleStart = cStart;
            _circleEnd = cEnd;
            _circleDuration = cDuration;
            _ballStart = bStart;
            _ballEnd = bEnd;
            _ballDuration = bDuration;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _circle.Init(_circleStart, _circleEnd, _circleDuration);
            _ball.Init(_ballStart, _ballEnd, _ballDuration);
            StartCoroutine(Deactivate());
        }

        IEnumerator Deactivate()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            gameObject.SetActive(false);
        }
    }
}

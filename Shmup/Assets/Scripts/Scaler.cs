using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField]
    private float _startScale;
    [SerializeField]
    private float _endScale;
    [SerializeField]
    private float _duration;
    private float _mod;
    private float _end;

    private void Awake()
    {
        if(_mod == 0)
        {
            Init(_startScale, _endScale, _duration);
        }
    }

    public void Init(float start, float end, float duration)
    {
        gameObject.SetActive(true);
        _startScale = start;
        _endScale = end;
        _duration = duration;
        _end = Time.frameCount + _duration;
        _mod = (_endScale - _startScale) / _duration;
        transform.localScale = new Vector3(_startScale, _startScale);
    }

    private void Update ()
    {
        transform.localScale = new Vector3(transform.localScale.x + _mod, transform.localScale.y + _mod);
		if(_end <= Time.frameCount)
        {
            gameObject.SetActive(false);
        }
	}
}

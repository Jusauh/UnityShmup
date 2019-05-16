using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite[] _sprites;
    [SerializeField]
    private int _speed;
    private int _nextFrame;
    private int _index;

	void Awake ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("No Sprite renderer available for Animator");
        }
        _index = 0;
        _nextFrame = Time.frameCount + _speed;
        _spriteRenderer.sprite = _sprites[_index];
	}
	
	void Update ()
    {
        if (Time.frameCount >= _nextFrame)
        {
            _nextFrame = Time.frameCount + _speed;
            _index++;
            if(_index >= _sprites.Length)
            {
                _index = 0;
            }
            _spriteRenderer.sprite = _sprites[_index];
        }
	}
}

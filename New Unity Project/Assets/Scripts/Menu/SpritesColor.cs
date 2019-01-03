using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesColor : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve curve;
    private SpriteRenderer[] _sprites;
    private float _threshold;
    private bool _open;
    private float _t;
    private float _speed;

    // Use this for initialization
    void Start()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        _open = true;
        _t = 0;
        _speed = 0.5f;
        _threshold = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime * _speed;
        if (curve.Evaluate(_t) <= _threshold && !_open)
        {
            _open = true;
            foreach (var sprite in _sprites)
            {
                sprite.color = Color.green;
            }
        }
        if (curve.Evaluate(_t) > _threshold && _open)
        {
            _open = false;
            foreach (var sprite in _sprites)
            {
                sprite.color = Color.red;
            }
        }
    }
}

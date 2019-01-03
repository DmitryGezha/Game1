using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform point;
    private Transform _thisTransform;
    [SerializeField]
    private AnimationCurve x;
    [SerializeField]
    private AnimationCurve y;
    [SerializeField]
    private AnimationCurve z;
    private float _t;
    private float _speed;

    // Use this for initialization
    void Start()
    {
        _thisTransform = transform;
        _t = 0;
        _speed = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime * _speed;
        if (_t >= 10) _t -= 10;
        _thisTransform.localPosition = new Vector3(x.Evaluate(_t), y.Evaluate(_t), z.Evaluate(_t));
        if (_t >= 8)
        {
            _thisTransform.position += new Vector3(0, point.position.y - _thisTransform.position.y, 0);
        }
        _thisTransform.LookAt(target);
    }
}

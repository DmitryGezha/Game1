using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve curve;
    private Transform _thisTransform;
    private float _t;
    private float _speed;
    private Vector3 _start;

    // Use this for initialization
    void Start()
    {
        _thisTransform = transform;
        _t = 0;
        _speed = 0.5f;
        _start = _thisTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime * _speed;
        _thisTransform.localPosition = _start + _thisTransform.up * curve.Evaluate(_t);
    }
}

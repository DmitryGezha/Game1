using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _keysOpened;
    private bool _trigger;

    private Transform _door;
    private Rigidbody _doorRB;
    private Vector3 _closePos;
    private Vector3 _openPos;
    private readonly float _speed = 0.2f;

    [SerializeField]
    private SpriteRenderer[] _sprites;

    [SerializeField]
    private Key[] _keys;

    [SerializeField]
    private bool _broken = false;

    [SerializeField]
    private ParticleSystem[] _sparks;

    // Use this for initialization
    void Start()
    {
        IsOpen = false;
        _keysOpened = false;
        _trigger = false;
        _door = transform.Find("Door");
        _doorRB = _door.GetComponent<Rigidbody>();
        _closePos = _door.position;
        _openPos = _broken ?
            _closePos + _door.up * 1.5f:
            _closePos + _door.up * 4; ;
    }

    public bool IsOpen { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Touching")
        {
            _trigger = true;
            Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Touching")
        {
            _trigger = false;
            if (!_keysOpened)
                Close();
        }
    }

    void Open()
    {
        if (IsOpen) return;
        IsOpen = true;
        StopAllCoroutines();
        StartCoroutine(MoveDoor(_openPos));
        foreach (var s in _sprites)
        {
            s.color = Color.green;
        }
        if (_sparks != null)
        {
            foreach (var s in _sparks)
            {
                s.Play();
            }
        }
    }

    void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;
        StopAllCoroutines();
        StartCoroutine(MoveDoor(_closePos));
        foreach (var s in _sprites)
        {
            s.color = Color.red;
        }
        if (_sparks != null)
        {
            foreach (var s in _sparks)
            {
                s.Stop();
            }
        }
    }

    IEnumerator MoveDoor(Vector3 tar)
    {
        Vector3 d = (tar.y - _door.position.y >= 0) ?
            new Vector3(0, _speed, 0) :
            new Vector3(0, -_speed, 0);
        _doorRB.isKinematic = false;
        while (Vector3.Distance(_door.position, tar) > 0.1f)
        {
            _doorRB.MovePosition(_door.position + d);
            yield return null;
        }
        _doorRB.isKinematic = true;
        _door.position = tar;
    }

    public void CheckKeys()
    {
        if (_keys == null) return;
        foreach (var b in _keys)
        {
            if (!b.Open)
            {
                _keysOpened = false;
                if (!_trigger)
                    Close();
                return;
            }
        }
        _keysOpened = true;
        Open();
    }
}

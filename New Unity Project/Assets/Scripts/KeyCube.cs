using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCube : Key {

    private Obj _obj;
    private bool _prevState;

	// Use this for initialization
	void Start () {
        _obj = GetComponent<Obj>();
        _prevState = _obj.Real;
	}
	
	// Update is called once per frame
	void Update () {
        if (_obj.Real != _prevState)
        {
            _prevState = _obj.Real;
            _opened = _obj.Real;
            door.CheckKeys();
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Key {

    [SerializeField]
    private SpriteRenderer[] sprites;

	// Use this for initialization
	void Start () {
        _opened = false;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            _opened = true;
            foreach (var s in sprites)
            {
                s.color = Color.green;
            }
            if (door != null)
                door.CheckKeys();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            _opened = false;
            foreach (var s in sprites)
            {
                s.color = Color.red;
            }
            if (door != null)
                door.CheckKeys();
        }
    }
}

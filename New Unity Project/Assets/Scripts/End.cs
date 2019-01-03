using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.ShowText("You win!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.RemoveText();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undestroyable : MonoBehaviour {

    private static Dictionary<string, bool> loaded = new Dictionary<string, bool>();
    private string objectName;

    void Awake()
    {
        objectName = name;
        if (loaded.ContainsKey(objectName) && loaded[objectName]) Destroy(gameObject);
        loaded[objectName] = true;
        DontDestroyOnLoad(gameObject);
    }
}

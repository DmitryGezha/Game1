using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{

    [SerializeField]
    private int number;

    private static Action[] action;

    void Awake()
    {
        action = new Action[3];
        action[1] = CanCloseEyes;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("Checkpoint") < number)
            {
                PlayerPrefs.SetInt("Checkpoint", number);
                GameController.Instance.ShowText("Checkpoint!");
                if (action[number] != null)
                    action[number]();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.RemoveText();
        }
    }

    void CanCloseEyes()
    {
        GameController.Instance.Player.CanCloseEyes = true;
    }

    public static void AllTriggerActions(int n)
    {
        for (int i = 0; i <= n; i++)
        {
            if (action[i] != null)
                action[i]();
        }
    }
}

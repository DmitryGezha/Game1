using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, определяющий соприкосновение игрока с объектами
/// </summary>
public class Touching : MonoBehaviour
{
    private Transform _thisTransform;

    // Use this for initialization
    void Start()
    {
        _thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Перемещать объект вместе с игроком
        _thisTransform.position = GameController.Instance.PlayerTransform.position; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Obj>())
        {
            other.GetComponent<Obj>().IsTouched = true;     // Соприкоснуться с объектом
        }
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Obj>())
        {
            other.GetComponent<Obj>().IsTouched = false;    // Прекратить соприкосновение
        }
    }
}

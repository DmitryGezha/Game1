using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс объектов, которые можно взять в руки
/// </summary>
public class PickUpable : MonoBehaviour
{
    private Transform _thisTransform;
    private Rigidbody _rb;
    private Collider _col;

    private bool _take;                         // Взял ли игрок объект
    private bool _hold;                         // Держит ли игрок объект

    [SerializeField]
    private float attractSpeed = 50;     // Скорость притяжения
    [SerializeField]
    private float dist = 2f;             // Расстояние от игрока до объекта при притяжении
    [SerializeField]
    private float thrust = 10;          // Сила броска

    void Awake()
    {
        _thisTransform = transform;
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    void Start()
    {
        _take = false;
        _hold = false;
    }

    void Update()
    {


        if (_hold)                       // Если объект подобран, изменять его положение
        {
            _thisTransform.position = GameController.Instance.CamTransform.position
                + (GameController.Instance.CamTransform.forward * dist);
            _thisTransform.rotation = GameController.Instance.PlayerTransform.rotation;
            GameController.Instance.Player.SetBoxCollider(_thisTransform.position);
            //Vector3 tarPos = GameController.instance.camTransform.position
            //    + (GameController.instance.camTransform.forward * dist);
            //_rb.MovePosition(tarPos);
            //Quaternion tarRot = GameController.instance.playerTransform.rotation;
            //_rb.MoveRotation(tarRot);
            //GameController.instance.player.SetBoxCollider(_thisTransform.position);
        }
    }

    /// <summary>
    /// Взять объект
    /// </summary>
    public void PickUp()
    {
        _take = true;
        _col.enabled = false;
        _rb.isKinematic = true;
        _thisTransform.rotation = GameController.Instance.PlayerTransform.rotation;
        StartCoroutine("Approach");
    }

    // Притяжение объекта
    IEnumerator Approach()
    {
        while (Vector3.Distance(GameController.Instance.PlayerTransform.position, _thisTransform.position) >= dist)
        {
            _thisTransform.position =
                Vector3.MoveTowards(_thisTransform.position,
                GameController.Instance.PlayerTransform.position,
                Time.deltaTime * attractSpeed);
            yield return null;
            if (!_take)          // Если игрок бросил объект, не дождавшись конца притяжения, оно прерывается
            {
                yield break;
            }
        }
        _take = false;
        _hold = true;                        // Остановка притяжения
        // Установка коллаидера игрока на месте объекта
        GameController.Instance.Player.SetBoxCollider(_thisTransform.position, (_col as BoxCollider).size);
    }

    /// <summary>
    /// Бросить объект
    /// </summary>
    public void Cast()
    {
        _take = false;
        _hold = false;
        GameController.Instance.Player.DisableBoxCollider();
        _rb.isKinematic = false;
        _col.enabled = true;
        _rb.AddForce(GameController.Instance.CamTransform.forward * thrust);    // К объекту прекладывается сила броска
    }
}

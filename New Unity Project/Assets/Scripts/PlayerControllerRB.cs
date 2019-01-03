using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управления игроком
/// </summary>
public class PlayerControllerRB : MonoBehaviour
{

    private Transform _thisTransform;
    private Rigidbody _rb;
    private CapsuleCollider _cc;

    [SerializeField]
    private float minXRotation = -90;                // Ограничения вращения мыши
    [SerializeField]
    private float maxXRotation = 60;
    [SerializeField]
    private float mouseSens = 5;                     // Чувствительность мыши

    [SerializeField]
    private float speed = 4;                         // Скорость передвижения игрока
    [SerializeField]
    private float jumpSpeed = 6;                     // Сила прыжка
    [SerializeField]
    private float crawlSpeed = 2;
    [SerializeField]
    private float step = 0.1f;
    private Vector3 _dir;
    private Vector3 _rot;

    bool _crawling;

    void Awake()
    {
        _thisTransform = transform;
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CapsuleCollider>();
    }

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;                     // Скрытие курсора мыши
        _dir = Vector3.zero;
        _rot = Vector3.zero;
        _crawling = false;
    }

    void Update()
    {
        MouseRotation();
    }

    void FixedUpdate()
    {
        Walk();
    }

    /// <summary>
    /// Вращение мышью
    /// </summary>
    void MouseRotation()
    {
        if (!_crawling)
        {
            // При вертикальном передвижении мыши вращается только камера
            _rot = GameController.Instance.CamTransform.rotation.eulerAngles;
            float _temp = Clamp(_rot.x - Input.GetAxis("Mouse Y") * mouseSens, minXRotation, maxXRotation);
            GameController.Instance.CamTransform.rotation = Quaternion.Euler(_temp, _rot.y, _rot.z);
        }

        // При горизонтальном - сам игрок
        _thisTransform.Rotate(0, Input.GetAxis("Mouse X") * mouseSens, 0);
    }

    /// <summary>
    /// Ходьба
    /// </summary>
    void Walk()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Crawl();
        }
        else
        {
            Stand();
        }

        _dir += _thisTransform.right * hor + _thisTransform.forward * ver;
        if ((ver != 0) || (hor != 0))
        {
            _dir += _thisTransform.up * step;
        }
        if (_crawling)
        {
            _dir *= crawlSpeed * Time.fixedDeltaTime;
        }
        else
        {
            _dir *= speed * Time.fixedDeltaTime;
        }

        if (isGrounded())
        {
            if (Input.GetButtonDown("Jump"))            // Прыжок
            {
                _rb.AddForce(_thisTransform.up * jumpSpeed, ForceMode.Impulse);
            }
        }
        else
        {
            _dir *= 0.5f;
        }

        _rb.MovePosition(_thisTransform.position + _dir);
    }

    /// <summary>
    /// Находится ли игрок на поверхности
    /// </summary>
    /// <returns></returns>
    bool isGrounded()
    {
        return Physics.CheckSphere(_thisTransform.position - new Vector3(0, 0.6f, 0), 0.5f, -1029);
    }

    float Clamp(float f, float min, float max)      // Ограничение угла поворота
    {
        if (f > max && f < 180) f = max;
        if (f >= 180 && f < 360 + min) f = 360 + min;
        return f;
    }

    void Crawl()
    {
        if (_crawling) return;
        _crawling = true;
        _cc.height = 1.5f;
        GameController.Instance.CamTransform.localPosition -= new Vector3(0, 0.5f, 0);
        GameController.Instance.CamTransform.rotation = _thisTransform.rotation;
    }

    void Stand()
    {
        if (!_crawling) return;
        _crawling = false;
        _cc.height = 2;
        GameController.Instance.CamTransform.localPosition += new Vector3(0, 0.5f, 0);
    }
}

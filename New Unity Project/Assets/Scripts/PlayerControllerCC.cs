using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerCC : MonoBehaviour {

    private Transform _thisTransform;
    private CharacterController _cc;

    [SerializeField]
    private float minXRotation = -90;                // Ограничения вращения мыши
    [SerializeField]
    private float maxXRotation = 45;
    [SerializeField]
    private float mouseSens = 5;                     // Чувствительность мыши

    [SerializeField]
    private float speed = 8;                      // Скорость передвижения игрока
    [SerializeField]
    private float jumpSpeed = 8;                     // Сила прыжка
    [SerializeField]
    private float gravity = 20;
    private Vector3 _dir;
    private Vector3 _rot;

    void Awake()
    {
        _thisTransform = transform;
        _cc = GetComponent<CharacterController>();
    }

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;                     // Скрытие курсора мыши	
        _dir = Vector3.zero;
        _rot = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        MouseRotation();
        Walk();
	}

    /// <summary>
    /// Вращение мышью
    /// </summary>
    void MouseRotation()
    {
        // При вертикальном передвижении мыши вращается только камера
        _rot = GameController.Instance.CamTransform.rotation.eulerAngles;
        float _temp = Clamp(_rot.x - Input.GetAxis("Mouse Y") * mouseSens, minXRotation, maxXRotation);
        GameController.Instance.CamTransform.rotation = Quaternion.Euler(_temp, _rot.y, _rot.z);

        // При горизонтальном - сам игрок
        _thisTransform.Rotate(0, Input.GetAxis("Mouse X") * mouseSens, 0);
    }

    float Clamp(float f, float min, float max)      // Ограничение угла поворота
    {
        if (f > max && f < 180) f = max;
        if (f >= 180 && f < 360 + min) f = 360 + min;
        return f;
    }

    /// <summary>
    /// Ходьба
    /// </summary>
    void Walk()
    {
        if (_cc.isGrounded)
        {
            _dir = _thisTransform.forward * Input.GetAxis("Vertical") +
                _thisTransform.right * Input.GetAxis("Horizontal");
            _dir *= speed;
            if (Input.GetButton("Jump"))
            {
                _dir.y = jumpSpeed;
            }
        }
        _dir.y -= gravity * Time.deltaTime;
        _cc.Move(_dir * Time.deltaTime);
    }
}

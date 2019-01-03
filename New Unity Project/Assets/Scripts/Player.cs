using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, который определяет поведение игрока, кроме передвижения
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private RawImage aim;                // Картинка прицела
    [SerializeField]
    private RawImage eye;                // Картинка глаза
    [SerializeField]
    private Image blackout;              // Темный экран
    private Texture[] _img;                     // Массив картинок
    private int _imgOfAim;                      // Номер текущей картинки прицела 
    // (0 - нет объектов, которые можно подобрать, 1 - есть объект)
    [SerializeField]
    private float playersRange = 3;      // Область досигаемости игрока

    private BoxCollider _bc;                    // Коллаидер на месте объекта, который будет держать игрок

    private PickUpable _pickedObj;              // Объект, который держит игрок


    void Awake()
    {
        // Содание коллаидера. При взятии объекта примет координаты этого объекта
        _bc = gameObject.AddComponent<BoxCollider>();
    }

    // Use this for initialization
    void Start()
    {
        _imgOfAim = 0;
        _img = new Texture[4];
        _img[0] = Resources.Load<Texture>("Sprites/0");
        _img[1] = Resources.Load<Texture>("Sprites/1");
        _img[2] = Resources.Load<Texture>("Sprites/Open");
        _img[3] = Resources.Load<Texture>("Sprites/Close");  // Картинки интерфейса
        _bc.enabled = false;
        EyesOpen = true;
        CanCloseEyes = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanCloseEyes)
        {
            CloseEyes();
        }
        if (Input.GetKeyUp(KeyCode.E) && CanCloseEyes)
        {
            OpenEyes();
        }

        if (Input.GetKey(KeyCode.R))
        {
            GameController.Instance.RestartFromCheckpoint();
        }

        if (GameController.Instance.PlayerTransform.position.y < -250)
        {
            GameController.Instance.RestartFromCheckpoint();
        }
    }

    void FixedUpdate()
    {
        Ray _ray;
        RaycastHit _hit = new RaycastHit();
        bool _raycast = false;                  // Попадание луча
        bool _pickUpable = false;               // Объект можно взять
        bool _obj = false;                      // Объект может быть реальным или мнимым
        Obj _o = null;
        bool _real = false;                     // Объект реален

        //-----------------------------------------------------------------------------------------

        if (EyesOpen)                          // Если глаза открыты
        {
            if (!_pickedObj)                    // Если в руках ничего нет
            {
                _ray = GameController.Instance.Cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                _raycast = Physics.Raycast(_ray, out _hit, playersRange, ~((1 << 2) | (1 << 8)));
                if (_raycast)
                {
                    _pickUpable = _hit.transform.GetComponent<PickUpable>();
                    _obj = _hit.transform.GetComponent<Obj>();
                    if (_obj)
                    {
                        _o = _hit.transform.GetComponent<Obj>();
                        _real = _o.Real;
                    }
                }
            }
            else                                // Если игрок держит объект
            {
                _obj = _pickedObj.GetComponent<Obj>();
                if (_obj)
                {
                    _o = _pickedObj.GetComponent<Obj>();
                }
            }

            //-----------------------------------------------------------------------------------------

            if (_raycast && _pickUpable && (!_obj || _real) || _pickedObj)  // Если луч попадает на объект, который можно
            {                                                               // взять и он реален или игрок уже держит объект -
                ChangeImageOfAim(1);                                        // поставить картинку прицела №1
                if (Input.GetKey(KeyCode.Mouse0) && !_pickedObj)            // Если нажата мышь - взять предмет
                {
                    _pickedObj = _hit.transform.GetComponent<PickUpable>();
                    if (_obj)
                    {
                        _o.IsTouched = true;
                    }
                    _pickedObj.PickUp();
                }
            }
            else
            {
                ChangeImageOfAim(0);                                        // иначе - поставить картинку прицела №0
            }
        }

        //-----------------------------------------------------------------------------------------

        if (_pickedObj && !Input.GetKey(KeyCode.Mouse0))                    // Если игрок держит объект и отжата мышь
        {                                                                   // бросить объект
            _obj = _pickedObj.GetComponent<Obj>();
            if (_obj)
            {
                _o = _pickedObj.GetComponent<Obj>();
                _o.IsTouched = false;
            }
            _pickedObj.Cast();
            _pickedObj = null;
        }
    }

    /// <summary>
    /// Открыты ли глаза у игрока
    /// </summary>
    public bool EyesOpen { get; private set; }

    public bool CanCloseEyes { get; set; }

    // Открыть глаза
    void OpenEyes()
    {
        if (EyesOpen) return;
        EyesOpen = true;
        blackout.enabled = false;
        eye.texture = _img[2];
        aim.enabled = true;
    }

    // Закрыть глаза
    void CloseEyes()
    {
        if (!EyesOpen) return;
        EyesOpen = false;
        blackout.enabled = true;
        eye.texture = _img[3];
        aim.enabled = false;
    }

    // Поставить коллаидер на указанное место
    public void SetBoxCollider(Vector3 center, Vector3 size)
    {
        _bc.enabled = true;
        _bc.center = GameController.Instance.PlayerTransform.InverseTransformPoint(center) - new Vector3(0, 0, 0.5f);
        _bc.size = size + Vector3.forward;
    }

    public void SetBoxCollider(Vector3 center)
    {
        _bc.enabled = true;
        _bc.center = GameController.Instance.PlayerTransform.InverseTransformPoint(center) - new Vector3(0, 0, 0.5f);
    }

    public void DisableBoxCollider()
    {
        _bc.enabled = false;
    }

    // Изменить картинку прицела
    void ChangeImageOfAim(int n)
    {
        if (_imgOfAim != n)
        {
            _imgOfAim = n;
            aim.texture = _img[_imgOfAim];
        }
    }
}

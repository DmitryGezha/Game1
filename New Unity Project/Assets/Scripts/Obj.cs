using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс объектов, которые могут быть реальными (когда на них смотрит игрок) и мнимыми (не смотрит)
/// </summary>
public class Obj : MonoBehaviour
{
    [SerializeField]
    private bool _real;                           // Реален ли объект
    private bool _touched;                        // Соприкасается ли с объектом игрок

    private Transform _thisTransform;
    private Renderer _rend;
    private Collider _col;

    private bool _pickupable;

    void Awake()
    {
        _thisTransform = transform;
        _rend = GetComponent<Renderer>();
        _col = GetComponent<Collider>();
        _pickupable = GetComponent<PickUpable>();
    }

    void FixedUpdate()
    {
        if (!IsIntersected)                         // Если объект ничего не пересекает
        {
            if (IsVisible() || IsTouched)           // если игрок видит объект или осязает его
            {
                BecomeReal();                       // стать или оставаться реальным
            }
            else
            {
                BecomeImaginary();                  // иначе - мнимым
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_thisTransform.position.y < -100)       // Если объект полетел вниз - уничтожить его
            Destroy(_thisTransform.gameObject);
    }

    /// <summary>
    /// Виден ли объект игроком
    /// </summary>
    /// <returns></returns>
    public bool IsVisible()
    {
        // Находится ли объект в поле зрения камеры и нет ли препятствий
        return InFieldOfView(GameController.Instance.Cam, _rend.bounds) && 
            !Obstacle(GameController.Instance.CamTransform.position, FindCorners(_rend.bounds));
    }

    /// <summary>
    /// Соприкасается ли игрок с объектом
    /// </summary>
    public bool IsTouched
    {
        get
        { return _touched; }
        set
        {
            if (value != _touched)
            {
                _touched = value;
            }
        }
    }

    /// <summary>
    /// Пересекается ли объект с чем-либо
    /// </summary>
    public bool IsIntersected
    {
        get
        {
            //return _intersections.Count > 0; 
            if (_col as BoxCollider)
            {
                return Physics.OverlapBox(_thisTransform.position,
                    0.95f * _thisTransform.lossyScale / 2,
                    _thisTransform.rotation, ~(1 << 2)).Length > 1;
            }
            if (_col as SphereCollider)
            {
                return Physics.OverlapSphere(_thisTransform.position,
                    (_col as SphereCollider).radius * 0.95f).Length > 1;
            }
            return true;
        }
    }

    /// <summary>
    /// Реален ли объект
    /// </summary>
    public bool Real
    {
        get
        { return _real; }
    }

    /// <summary>
    /// Стать мнимым объектом
    /// </summary>
    public void BecomeImaginary()
    {
        if (!_real) return;
        _rend.material = _pickupable ? Resources.Load<Material>("Materials/PickUpableImaginary") :
            Resources.Load<Material>("Materials/Imaginary");
        _real = false;
        //_thisTransform.tag = "Imaginary";
        _thisTransform.gameObject.layer = 8;
    }

    /// <summary>
    /// Стать реальным объектом
    /// </summary>
    public void BecomeReal()
    {
        if (_real) return;
        _rend.material = _pickupable ? Resources.Load<Material>("Materials/PickUpable") :
            Resources.Load<Material>("Materials/Real");
        _real = true;
        //_thisTransform.tag = "Real";
        _thisTransform.gameObject.layer = 9;
    }

    // Найти координаты углов объекта
    Vector3[] FindCorners(Bounds b)
    {
        Vector3 min = b.min;
        Vector3 max = b.max;
        Vector3[] result = new Vector3[9];

        result[0] = b.center;
        result[1] = min;
        result[2] = new Vector3(max.x, min.y, min.z);
        result[3] = new Vector3(min.x, max.y, min.z);
        result[4] = new Vector3(min.x, min.y, max.z);
        result[5] = new Vector3(max.x, max.y, min.z);
        result[6] = new Vector3(max.x, min.y, max.z);
        result[7] = new Vector3(min.x, max.y, max.z);
        result[8] = max;

        return result;
    }

    // Есть ли препятствие между точкой и массивом точек
    bool Obstacle(Vector3 start, Vector3[] ends)
    {
        RaycastHit hit;
        int layerMask = -1285; // ~((1 << 10) | (1 << 2) | (1 << 8));
        foreach (var e in ends)
        {
            if (!Physics.Linecast(start, e, out hit, layerMask) || hit.transform.Equals(_thisTransform)) return false;
        }
        return true;
    }

    // Находится ли объект в поле зрения камеры
    bool InFieldOfView(Camera c, Bounds b)
    {
        if (!GameController.Instance.Player.EyesOpen) return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(c);
        return GeometryUtility.TestPlanesAABB(planes, b);
    }
}

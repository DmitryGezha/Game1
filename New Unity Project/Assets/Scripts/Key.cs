using UnityEngine;

public class Key : MonoBehaviour
{

    protected bool _opened;
    [SerializeField] protected Door door;

    public bool Open
    {
        get { return _opened; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Синглтон, содержащий информацию об игре
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform[] _checkpoints;
    [SerializeField]
    private Text text;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Player = PlayerTransform.GetComponent<Player>();
        Cam = Camera.main;
        CamTransform = Cam.transform;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(8, 10);
        Physics.IgnoreLayerCollision(10, 10);
        MoveToCheckpoint();
    }

    void MoveToCheckpoint()
    {
        int currentCheckpoint = PlayerPrefs.GetInt("Checkpoint");
        Checkpoint.AllTriggerActions(currentCheckpoint);
        PlayerTransform.position = _checkpoints[currentCheckpoint].position;
        PlayerTransform.rotation = _checkpoints[currentCheckpoint].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Синглтон игры
    /// </summary>
    static public GameController Instance { get; private set; }

    /// <summary>
    /// Трансформ игрока
    /// </summary>
    public Transform PlayerTransform { get; private set; }

    /// <summary>
    /// Экземпляр класса игрока
    /// </summary>
    public Player Player { get; private set; }

    /// <summary>
    /// Трансформ камеры
    /// </summary>
    public Transform CamTransform { get; private set; }

    /// <summary>
    /// Камера
    /// </summary>
    public Camera Cam { get; private set; }

    public void ShowText(string str)
    {
        text.text = str;
        text.enabled = true;
    }

    public void RemoveText()
    {
        text.enabled = false;
    }

    public void RestartFromCheckpoint()
    {
        SceneManager.LoadScene("Game");
        MoveToCheckpoint();
        StartCoroutine(ShowTextForSeconds("Restart from checkpoint!", 2));
    }

    IEnumerator ShowTextForSeconds(string str, int sec)
    {
        ShowText(str);
        yield return new WaitForSeconds(sec);
        RemoveText();
    }
}

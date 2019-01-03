using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    private string _name;
    private Texture2D[] _img;
    private UnityEngine.UI.Button _btn;

	// Use this for initialization
	void Start () {
        _name = name;
        _btn = GetComponent<UnityEngine.UI.Button>();
        _img = new Texture2D[2];
        _img[0] = Resources.Load<Texture2D>("Sprites/cursor_0");
        _img[1] = Resources.Load<Texture2D>("Sprites/cursor_1");
        if (_name == "Resume" && PlayerPrefs.GetInt("Checkpoint", 0) == 0)
        {
            _btn.interactable = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Resume()
    {
        SceneManager.LoadScene("Game");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PointerEnter()
    {
        if (_btn.interactable)
        {
            Cursor.SetCursor(_img[1], new Vector2(15, 15), CursorMode.Auto);
        }
    }

    public void PointerExit()
    {
        if (_btn.interactable)
        {
            Cursor.SetCursor(_img[0], new Vector2(15, 15), CursorMode.Auto);
        }
    }
}

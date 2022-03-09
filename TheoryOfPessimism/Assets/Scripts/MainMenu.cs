using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadGame;
    public Button controlUI;
    public bool controlUIShow;
    public int sceneToLoad;
    public GameObject controls;

    public void PlayOnAwake()
    {
        controlUIShow = false;
        /////SoundManager.Instance.PlayMusic(Music.UI_Game_Audio);
    }

    private void Start()
    {
        loadGame.onClick.AddListener(LoadGame);
        controlUI.onClick.AddListener(ShowControlMenu);
        //SoundManager.Instance.PlayMusic(Music.UI_Game_Audio);
    }

    private void Update()
    {
        if (controlUIShow)
        {
            ControlMenu();
        }
        else
        {

        }
    }

    public void LoadGame()
    {
        //SoundManager.Instance.PlaySound(Sound.Button_Click);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ShowControlMenu()
    {
        //SoundManager.Instance.PlaySound(Sound.Button_Click);
        controlUIShow = true;
    }

    public void ControlMenu()
    {
        controls.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controls.SetActive(false);
            controlUIShow = false;
        }

    }

}

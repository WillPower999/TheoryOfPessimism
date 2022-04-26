using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadGame;
    public Button newGame;
    public Button controlUI;
    public bool controlUIShow;
    public bool gameToLoad;
    public int sceneToLoad;
    public GameObject controls;
    public GameObject saveSystem;

    public void Awake()
    {
        controlUIShow = false;
        SoundManager.Instance.PlayMusic(Music.Theme_Music);
        gameToLoad = false;
    }

    private void Start()
    {
        newGame.onClick.AddListener(NewGame);
        loadGame.onClick.AddListener(LoadGame);
        controlUI.onClick.AddListener(ShowControlMenu);
        //SoundManager.Instance.PlayMusic(Music.UI_Game_Audio);
        //SoundManager.Instance.PlayMusic(Music.Theme_Music);
        gameToLoad = SaveSystem.GetBool("isInteractable");
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
        
        if(gameToLoad)
        {
            loadGame.interactable = true;
        }
    }

    public void NewGame()
    {
        //SoundManager.Instance.PlaySound(Sound.Button_Click);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
        DontDestroyOnLoad(saveSystem);
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

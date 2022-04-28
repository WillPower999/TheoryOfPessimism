using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public CharacterMovement player;
    public EventHandling[] events;
    public MainMenu mM;
    public bool isMainMenu;
    public Transform playerStartTransform;
    private static bool loadNewGame;


    void Awake()
    {
        if (isMainMenu)
        {
            mM = FindObjectOfType<MainMenu>();
        }
        else
        {

        }
        player = FindObjectOfType<CharacterMovement>();
        events = FindObjectsOfType<EventHandling>();

    }


    void Start()
    {
        if (!isMainMenu)
        {
            if (loadNewGame)
            {
                player.gameObject.transform.position = playerStartTransform.position;
                player.health = player.maxHealth;
            }
            else
            {
                player.gameObject.transform.position = SaveSystem.GetVector3("Player Position");
                player.health = SaveSystem.GetInt("Player Health");
            }
        }
    }


    void Update()
    {
        if (isMainMenu)
        {
            print("mM.newGameLoad = " + mM.newGameLoad);
            if (mM.newGameLoad)
            {
                loadNewGame = true;
            }
            else
            {
                loadNewGame = false;
            }
        }
        else
        {

        }

        print("loadNewGame = " + loadNewGame);
    }
}

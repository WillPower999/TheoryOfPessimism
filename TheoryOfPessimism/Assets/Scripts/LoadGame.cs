using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public CharacterMovement player;
    public EventHandling[] events;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<CharacterMovement>();
        events = FindObjectsOfType<EventHandling>();
    }

    // Update is called once per frame
    void Start()
    {
        player.gameObject.transform.position = SaveSystem.GetVector3("Player Position");
        player.health = SaveSystem.GetInt("Player Health");
    }
}

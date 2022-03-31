using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    public float minMoveTime;
    public float maxMoveTime;
    private int directionKey;
    public int movement;

    void Start()
    {
        directionKey = 0;
        StartCoroutine(Move());
    }

    void Update()
    {
        if(directionKey == 1)
        {
            gameObject.transform.Translate(0, movement, 0);
            directionKey = 0;
            StartCoroutine(Move());
        }
        else if (directionKey == 2)
        {
            gameObject.transform.Translate(0, -movement, 0);
            directionKey = 0;
            StartCoroutine(Move());
        }
        else if (directionKey == 3)
        {
            gameObject.transform.Translate(movement, 0, 0);
            directionKey = 0;
            StartCoroutine(Move());
        }
        else if (directionKey == 4)
        {
            gameObject.transform.Translate(-movement, 0, 0);
            directionKey = 0;
            StartCoroutine(Move());
        }
        else if (directionKey == 0)
        {

        }
    }

    private IEnumerator Move()
    {
        StopCoroutine(Move());
        float moveTime = Random.Range(minMoveTime, maxMoveTime);
        yield return new WaitForSeconds(moveTime);
        directionKey = Random.Range(1, 5);
    }
}

using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;
using UnityEngine;

public class DoorTransport : MonoBehaviour
{
    public CharacterMovement cM;
    public GameObject player;
    public GameObject doorToGoTo;
    public bool upOrDown;
    private float playerOffset;

    // Start is called before the first frame update
    void Start()
    {
        cM = FindObjectOfType<CharacterMovement>().GetComponent<CharacterMovement>();
        if (upOrDown)
        {
            playerOffset = 1.5f;
        }
        else
        {
            playerOffset = -1.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cM.canMove = false;
        StartCoroutine(EnterDoor());
    }

    private IEnumerator EnterDoor()
    {
        yield return new WaitForSeconds(1);
        player.gameObject.transform.position = new Vector3(doorToGoTo.gameObject.transform.position.x, doorToGoTo.gameObject.transform.position.y + playerOffset);
        cM.movement.x = 0;
        cM.movement.y = 0;
        cM.canMove = true;
    }
}

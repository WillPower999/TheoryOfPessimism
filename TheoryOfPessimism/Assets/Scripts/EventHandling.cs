using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandling : MonoBehaviour
{
    [SerializeField] private GameObject[] eventSequence;
    [SerializeField] private bool[] doesEventWait;
    [SerializeField] private int[] eventTime;
    public int arraySize;
    public CharacterMovement cM;
    private int sequenceNumber;
    // Start is called before the first frame update

    bool canInteract;
    bool interacting;
    void Start()
    {
       // eventSequence = new GameObject [arraySize];
       // doesEventWait = new bool[arraySize];
       // eventTime = new int[arraySize];
        cM = FindObjectOfType<CharacterMovement>().GetComponent<CharacterMovement>();
        sequenceNumber = 0;
    }

    private void Update()
    {
        //Debug.Log(sequenceNumber);

        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Slash))
            {
                canInteract = false;
                //interacting = true;
                cM.canMove = false;
                StartCoroutine(AdvanceSequence(0));

                //eventSequence[sequenceNumber].gameObject.SetActive(true);
                //sequenceNumber = sequenceNumber + 1;
            }
        }

        if (interacting)
        {
            if (!doesEventWait[sequenceNumber])
            {
                print("hit 'E' now");
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Slash))
                {
                    //sequenceNumber += 1;
                    StartCoroutine(AdvanceSequence(0));
                }
            }
            else
            {
               // sequenceNumber += 1;
                StartCoroutine(AdvanceSequence(eventTime[sequenceNumber]));
            }
            
        }
        if (sequenceNumber >= arraySize)
        {
            cM.gameObject.SetActive(true);
            interacting = false;
            canInteract = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
        print("entered");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }

    private IEnumerator AdvanceSequence(float duration)
    {
        print("sequence number: " + sequenceNumber + ", for " + duration + " seconds");
        yield return new WaitForSeconds(duration);
        interacting = true;
        print("interacting = " + interacting);
        eventSequence[sequenceNumber].gameObject.SetActive(true);
        print("item " + sequenceNumber + " exists");
        if (sequenceNumber >= arraySize - 1)
        {
            cM.canMove = true;
            interacting = false;
            canInteract = true;
        }
        else
        {
            sequenceNumber += 1;
            StopCoroutine(AdvanceSequence(duration));
            yield return null;
        }
    }
}

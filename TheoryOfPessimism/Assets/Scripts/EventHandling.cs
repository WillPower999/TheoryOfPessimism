using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandling : MonoBehaviour
{
    [SerializeField] public GameObject[] eventSequence;
    [SerializeField] private bool[] doesEventWait;
    [SerializeField] private int[] eventTime;
    [SerializeField] private bool[] doesGetDestroyed;
    [SerializeField] private bool[] doesTranslate;
    [SerializeField] private Vector3[] toTranslate;
    public int movementSpeed;
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
            if (doesGetDestroyed[sequenceNumber])
            {
                Destroy(eventSequence[sequenceNumber]);
            }
            //cM.gameObject.SetActive(true);
            interacting = false;
            canInteract = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }

    private IEnumerator AdvanceSequence(float duration)
    {
        yield return new WaitForSeconds(duration);
        interacting = true;
        if (sequenceNumber > 0)
            if (doesGetDestroyed[sequenceNumber - 1])
            {
                Destroy(eventSequence[sequenceNumber - 1]);
            }
        eventSequence[sequenceNumber].gameObject.SetActive(true);
        if (doesTranslate[sequenceNumber])
        {
            eventSequence[sequenceNumber].gameObject.transform.Translate(toTranslate[sequenceNumber] * movementSpeed * Time.deltaTime);
        }
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

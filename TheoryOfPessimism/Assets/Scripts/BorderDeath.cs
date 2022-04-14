using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderDeath : MonoBehaviour
{
    public CharacterMovement player;
    public float deathTime;

    private bool canDamage;
    
    // Start is called before the first frame update
    void Start()
    {
        canDamage = true;
    }

    void Update()
    {
        print("canDamage = " + canDamage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthLoss();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(canDamage)
        {
            HealthLoss();
        }
        else
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private IEnumerator HealthLoss()
    {
        canDamage = false;
        yield return new WaitForSeconds(deathTime);
        player.health -= 1;
        canDamage = true;
    }
}

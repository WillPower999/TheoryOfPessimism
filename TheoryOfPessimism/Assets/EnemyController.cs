using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerFightController player;
    
    public int enemyHealth;
    public EnemyStates state;
    public PlayerStates playerState;
    
    // Start is called before the first frame update
    void Start()
    {
        state = EnemyStates.normal;
        player = FindObjectOfType<PlayerFightController>().GetComponent<PlayerFightController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyStates.death:
                EnemyDies();
                break;
        }
        
        if(playerState == PlayerStates.attacking && state != EnemyStates.blocking)
        {
            enemyHealth = enemyHealth - player.attackDamage;
        }

        if(enemyHealth <= 0)
        {
            state = EnemyStates.death;
        }
    }

    void EnemyDies()
    {
        Destroy(gameObject);
    }
}

public enum EnemyStates
{
    normal,
    attacking,
    blocking,
    dodging,
    death
}

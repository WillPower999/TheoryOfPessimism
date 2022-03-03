using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerFightController player;

    public int enemyHealth;
    public int attackDamage;

    public int aI;
    public int aIMatch;

    [SerializeField] private int MAX_AI;

    public float delayTime;

    public bool isVulnerable;
    public bool isCheckingMovement;

    public EnemyStates state;
    public PlayerStates playerState;

    public Vector3 center;
    public Vector3 left;
    public Vector3 right;
    public Vector3 noTilt = Vector3.zero;
    public Vector3 leftTilt;
    public Vector3 rightTilt;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyStates.normal;
        player = FindObjectOfType<PlayerFightController>().GetComponent<PlayerFightController>();

        isVulnerable = false;
        isCheckingMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyStates.normal:
                NormalUpdate();
                break;
            case EnemyStates.attacking:
                AttackUpdate();
                break;
            case EnemyStates.blocking:
                BlockUpdate();
                break;
            case EnemyStates.dodging:
                // enemies cannot hit
                break;
            case EnemyStates.death:
                EnemyDies();
                break;
        }
    }

    void NormalUpdate()
    {
        if(playerState == PlayerStates.attacking && !isVulnerable)
        {
            state = EnemyStates.blocking;
            return;
        }
        
        if (state == EnemyStates.normal)
        {
            if (enemyHealth <= 0)
            {
                state = EnemyStates.death;
                return;
            }
        }
        
        if(!isCheckingMovement)
        { 
            StartCoroutine(MovementOpportunityCheck());
        }
        else
        {

        }

    }

    void AttackUpdate()
    {
        StartCoroutine(Attack());
    }

    void BlockUpdate()
    {
        StartCoroutine(BlockDelay());
    }

    void EnemyDies()
    {
        Destroy(gameObject);
    }

    private IEnumerator MovementOpportunityCheck()
    {
        isCheckingMovement = true;
        delayTime = Random.Range(1.5f, 2.5f);
        yield return new WaitForSeconds(delayTime);
        aIMatch = Random.Range(0, MAX_AI);
        if(aI >= aIMatch && playerState == PlayerStates.normal)
        {
            state = EnemyStates.attacking;
        }
        else
        {
            state = EnemyStates.normal;
        }
        isCheckingMovement = false;
    }

    private IEnumerator Attack()
    {
        if(playerState != PlayerStates.blocking || playerState != PlayerStates.dodging)
        { 
            player.playerHealth = player.playerHealth - attackDamage; 
        }
        state = EnemyStates.normal;
        isVulnerable = true;
        StartCoroutine(Vulnerability());
        yield return null;
    }

    private IEnumerator Vulnerability()
    {
        yield return new WaitForSeconds(Random.Range(1, 1.5f));
        isVulnerable = false;
    }

    private IEnumerator BlockDelay()
    {
        yield return new WaitForSeconds(5);
        state = EnemyStates.normal;
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

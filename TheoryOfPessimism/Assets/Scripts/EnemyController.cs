using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerFightController player;

    public int enemyHealth;
    public int attackDamage;

    public int aI;
    public int aIMatch;
    public int sceneToLoad;

    [SerializeField] private int MAX_AI;

    public float delayTime;
    public float vulnerabilityTimer;

    public bool isVulnerable;
    public bool isCheckingMovement;

    public EnemyStates state;

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
        print("State = " + state);
        if(player.state == PlayerStates.attacking && !isVulnerable)
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
            //StopCoroutine(nameof(MovementOpportunityCheck));
        }

    }

    void AttackUpdate()
    {
        StartCoroutine(Attack());
    }

    void BlockUpdate()
    {
        print("State = " + state);
        if (!(player.state == PlayerStates.attacking) && !isVulnerable)
        {
            state = EnemyStates.normal;
        }
        //StartCoroutine(BlockDelay());
    }

    void EnemyDies()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator MovementOpportunityCheck()
    {
        print("State = " + state);
        isCheckingMovement = true;
        delayTime = Random.Range(1.5f, 2.5f);
        yield return new WaitForSeconds(delayTime);
        aIMatch = Random.Range(0, MAX_AI);
        if(aI >= aIMatch && player.state == PlayerStates.normal)
        {
            state = EnemyStates.attacking;
        }
        else
        {
            state = EnemyStates.normal;
            isCheckingMovement = false;
        }
    }

    private IEnumerator Attack()
    {
        print("State = " + state);
        if (!(player.state == PlayerStates.blocking || player.state == PlayerStates.dodging))
        {
            player.playerHealth = player.playerHealth - attackDamage;
            state = EnemyStates.normal;
            isVulnerable = true;
            StartCoroutine(Vulnerability());
            yield return null;
        }
        else
        {
            state = EnemyStates.normal;
            isVulnerable = true;
            StartCoroutine(Vulnerability());
            yield return null;
        }

    }

    private IEnumerator Vulnerability()
    {
        print("State = " + state);
        vulnerabilityTimer = Random.Range(1, 1.5f);
        yield return new WaitForSeconds(vulnerabilityTimer);
        isCheckingMovement = false;
        isVulnerable = false;
    }

    //private IEnumerator BlockDelay()
    //{
    //    print("State = " + state);
    //    yield return new WaitForSeconds(5);
    //    state = EnemyStates.normal;
    //}
}

public enum EnemyStates
{
    normal,
    attacking,
    blocking,
    dodging,
    death
}

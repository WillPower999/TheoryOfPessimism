using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerFightController player;

    public int enemyHealth;
    public int attackDamage;

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
        if (state == EnemyStates.normal)
        {
            if (enemyHealth <= 0)
            {
                state = EnemyStates.death;
            }
        }
    }

    void AttackUpdate()
    {
        if (playerState == PlayerStates.normal)
        {
            StartCoroutine(Attack());
        }
        else
        {
            state = EnemyStates.normal;
        }
    }

    void BlockUpdate()
    {
        StartCoroutine(BlockDelay());
    }

    void EnemyDies()
    {
        Destroy(gameObject);
    }

    private IEnumerator Attack()
    {
        player.playerHealth = player.playerHealth - attackDamage;
        state = EnemyStates.normal;
        yield return null;
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

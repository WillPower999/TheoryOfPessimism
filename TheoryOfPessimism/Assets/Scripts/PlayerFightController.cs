using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightController : MonoBehaviour
{
    private EnemyController enemy;

    public PlayerStates state;
    public PlayerPosition position;
    public EnemyStates enemyState;

    public int playerHealth;
    public int attackDamage;
    public int blockMeterMax;
    public int blockMeter;

    public bool blockPossible;

    public Vector3 center;
    public Vector3 left;
    public Vector3 right;
    public Vector3 noTilt = Vector3.zero;
    public Vector3 leftTilt;
    public Vector3 rightTilt;

    // Start is called before the first frame update
    void Start()
    {
        position = PlayerPosition.center;
        state = PlayerStates.normal;

        enemy = FindObjectOfType<EnemyController>().GetComponent<EnemyController>();

        blockMeter = blockMeterMax;

        blockPossible = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerStates.normal:
                NormalUpdate();
                break;
            case PlayerStates.attacking:
                AttackUpdate();
                break;
            case PlayerStates.blocking:
                BlockUpdate();
                break;
            case PlayerStates.dodging:
                // enemies cannot hit
                break;
            case PlayerStates.death:
                PlayerDies();
                break;
        }

        switch (position)
        {
            case PlayerPosition.center:
                gameObject.transform.position = center;
                gameObject.transform.rotation = Quaternion.Euler(noTilt);
                break;
            case PlayerPosition.left:
                gameObject.transform.position = left;
                gameObject.transform.rotation = Quaternion.Euler(leftTilt);
                break;
            case PlayerPosition.right:
                gameObject.transform.position = right;
                gameObject.transform.rotation = Quaternion.Euler(rightTilt);
                break;
        }

        if (playerHealth <= 0)
        {
            state = PlayerStates.death;
        }
    }

    void NormalUpdate()
    {
        if (state == PlayerStates.normal)
        {
            //attack and defend
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                state = PlayerStates.attacking;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && blockPossible)
            {
                state = PlayerStates.blocking;
            }

            //dodge
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                position = PlayerPosition.left;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                position = PlayerPosition.right;
            }

            //Movement return
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                position = PlayerPosition.center;
            }

            //dying
            if (playerHealth <= 0)
            {
                state = PlayerStates.death;
            }
        }

    }

    void AttackUpdate()
    {
        if (enemyState == EnemyStates.normal)
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                enemy.enemyHealth = enemy.enemyHealth - attackDamage;
                state = PlayerStates.normal;
            }
        }
    }

    void BlockUpdate()
    {
        if(blockMeter > 0)
        {
            StartCoroutine(Block());
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                state = PlayerStates.normal;
                blockMeter = blockMeterMax;
            }
        }
        else 
        {
            blockPossible = false;
            state = PlayerStates.normal;
            blockMeter = blockMeterMax;
            StartCoroutine(BlockDelay()); 
        }

    }

    void PlayerDies()
    {
        Destroy(gameObject);
    }

    private IEnumerator Block()
    {
        yield return new WaitForSeconds(5);
        blockMeter = blockMeter - 1;
    }

    private IEnumerator BlockDelay()
    {
        yield return new WaitForSeconds(3);
        blockPossible = true;
    }
}

public enum PlayerStates
{
    normal,
    attacking,
    blocking,
    dodging,
    death
}

public enum PlayerPosition
{
    center,
    left,
    right
}
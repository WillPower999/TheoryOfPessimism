using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerFightController : MonoBehaviour
{
    private EnemyController enemy;

    public Animator animator;

    public AnimationClip idlecenter;
    public AnimationClip idleleft;
    public AnimationClip idleright;
    public AnimationClip attackcenter;
    public AnimationClip attackleft;
    public AnimationClip attackright;
    public AnimationClip blockcenter;
    public AnimationClip blockleft;
    public AnimationClip blockright;

    public PlayerStates state;
    public PlayerPosition position;
    public EnemyStates enemyState;

    public int playerHealth;
    public int attackDamage;
    public int blockMeterMax;
    public int blockMeter;
    public int sceneToLoad;

    public bool blockPossible;

    public Vector3 center;
    public Vector3 left;
    public Vector3 right;
    //public Vector3 noTilt = Vector3.zero;
    //public Vector3 leftTilt;
    //public Vector3 rightTilt;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayMusic(Music.Battle_Music);
        
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
                //gameObject.transform.rotation = Quaternion.Euler(noTilt);
                animator.Play("idlecenter");
                break;
            case PlayerPosition.left:
                gameObject.transform.position = left;
                //gameObject.transform.rotation = Quaternion.Euler(leftTilt);
                animator.Play("idleleft");
                break;
            case PlayerPosition.right:
                gameObject.transform.position = right;
                //gameObject.transform.rotation = Quaternion.Euler(rightTilt);
                animator.Play("idleright");
                break;
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
        if(position == PlayerPosition.center)
        {
            animator.Play("attackcenter");
        }
        if (position == PlayerPosition.left)
        {
            animator.Play("attackleft");
        }
        if (position == PlayerPosition.right)
        {
            animator.Play("attackright");
        }
        if (enemyState == EnemyStates.normal && enemy.isVulnerable)
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                enemy.enemyHealth = enemy.enemyHealth - attackDamage;
                enemy.isVulnerable = false;
                state = PlayerStates.normal;
            }
        }
        else if(!enemy.isVulnerable)
        {
            state = PlayerStates.normal;
        }
    }

    void BlockUpdate()
    {
        if(blockMeter > 0)
        {
            if (position == PlayerPosition.center)
            {
                animator.Play("blockcenter");
            }
            if (position == PlayerPosition.left)
            {
                animator.Play("blockleft");
            }
            if (position == PlayerPosition.right)
            {
                animator.Play("blockright");
            }
            StartCoroutine(nameof(Block));
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                state = PlayerStates.normal;
                blockMeter = blockMeterMax;
                StopCoroutine(nameof(Block));
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
        SceneManager.LoadScene(sceneToLoad);
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
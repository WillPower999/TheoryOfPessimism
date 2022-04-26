using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public Camera gameCamera;

    public int health;

    public int maxHealth;
    
    public float movementSpeed;

    public GameObject pauseMenu;

    public Rigidbody2D rb;

    public Animator animator;

    public Vector2 movement;

    public bool menuOpen;

    public bool canMove;

    public Button saveGame;

    private void Start()
    {
        menuOpen = false;
        canMove = true;
        health = maxHealth;
        saveGame.onClick.AddListener(SaveGame);
        SoundManager.Instance.PlayMusic(Music.Overworld_Music);
        //gameCamera.transform.position = gameObject.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (canMove)
        {
            //Vertical movement
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                movement.y = movement.y + 1;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                movement.y = movement.y - 1;
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                movement.y = 0;
            }

            //Horizontal movement
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                movement.x = movement.x + 1;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movement.x = movement.x - 1;
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                movement.x = 0;
            }

            if (menuOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    canMove = false;
                    pauseMenu.gameObject.SetActive(true);
                    menuOpen = true;
                }
            }
            else if (menuOpen == true)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    canMove = true;
                    pauseMenu.gameObject.SetActive(false);
                    menuOpen = false;

                }
            }

            //Animations

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        if(health <= 0)
        {
            Death();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
        }
        //gameCamera.transform.position = gameObject.transform.position;
    }

    private void Death()
    {
        SceneManager.LoadScene("WorldScene");
        health = maxHealth;
    }

    public void SaveGame()
    {
        SaveSystem.SetBool("isInteractable", true);
        SaveSystem.SetVector3("Player Position", gameObject.transform.position);
        SaveSystem.SetInt("Player Health", health);
    }
}

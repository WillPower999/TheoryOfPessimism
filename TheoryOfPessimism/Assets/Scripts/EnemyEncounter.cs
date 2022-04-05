using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    private EnemyController enemy;

    public GameObject player;
    public bool defeated;
    public int enemyAI;
    public int sceneToLoad;
    
    // Start is called before the first frame update
    void Start()
    {
        //enemy = FindObjectOfType<EnemyController>().GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D player)
    {
        if(!defeated)
        {
            SceneManager.LoadScene(sceneToLoad);
            //enemy.aI = enemyAI;
        }
        else
        {

        }
    }
}

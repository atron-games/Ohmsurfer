using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;
    private PlayerController playerControllerScript;
    private float leftBound = -25f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControllerScript.gameStarted == true && playerControllerScript.gameOver == false && playerControllerScript.bossKilled == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            animator.SetBool("movingLeft", true);

            if(transform.position.x < leftBound &! gameObject.CompareTag("Background"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            animator.SetBool("movingLeft", false);
        }

        if(playerControllerScript.bossKilled == true)
        {
            animator.SetBool("victory", true);
        }

    }
}

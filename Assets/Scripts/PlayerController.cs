using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //INPUT PARAMETERS
    private float tapThreshold = 0.075f;
    private float doubleMaxInterval = 0.05f;
    private bool checkingForDouble = false;
    private bool isDouble = false;

    //PLAYER PARAMETERS
    private Rigidbody2D playerRb;
    public float jumpForce;
    public float jetpackForce;
    private bool isOnGround = false;
    public float charge = 1;
    public GameObject projectile;
    private int hitPoints;

    //GAME PARAMETERS
    public bool gameOver = false;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && checkingForDouble == false && gameOver == false)
        {
            StartCoroutine(TapInput());
        }
    }

    IEnumerator TapInput()
    {
        //GIVE THE USER A MOMENT TO LET GO IF TAPPING
        yield return new WaitForSeconds(tapThreshold);

        //IF THE USER LET GO (TAP)
        if (Input.GetKey(KeyCode.Space) == false)
        {
            //CHECK FOR DOUBLE TAP
            checkingForDouble = true;
            isDouble = false;
            float t = 0;

            while(t < doubleMaxInterval)
            {
                t += Time.deltaTime;
               
                //SHOOT IF IT IS A DOUBLE TAP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot();
                    isDouble = true;
                    break;
                }
                yield return t;
            }

            //JUMP IF IT'S NOT A DOUBLE AND YOU ARE ON THE GROUND
            if (isOnGround && isDouble == false)
            {
                Jump();
            }

            //WAIT A LITTLE BIT SO WE DONT START FLOATING DURING THE DOUBLE TAP
            yield return new WaitForSeconds(.1f);
            checkingForDouble = false;
        }

        //FLOAT IF THE BUTTON IS HELD DOWN
        while (Input.GetKey(KeyCode.Space) && checkingForDouble == false)
        {
            if(isOnGround)
            {
                Jump();
                yield return new WaitForSeconds(.2f);
            }
            
            JetPack();
            yield return 0;
        }

    }

    //COLLISION
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnGround = true;

        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Enemy Projectile")
        {
            Destroy(collision.gameObject);
            hitPoints--;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Electric")
        {
            ChargeUp();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnGround = false;
    }

    //CHARACTER ACTIONS
    void Jump()
    {
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void JetPack()
    {
        if(charge > 0)
        {
            charge -= Time.deltaTime;
            playerRb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
        }
    }

    void Shoot()
    {
        if(charge >= 1)
        {
            charge = charge - 1;
            Instantiate(projectile, transform.position + new Vector3(1,0,0), Quaternion.identity);
        }
    }

    void ChargeUp()
    {
        if(charge <= 3)
        {
            charge += Time.deltaTime;
            playerRb.WakeUp();
        }
    }
}

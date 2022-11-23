using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //INPUT PARAMETERS
    private float tapThreshold = 0.1f;

    //PLAYER PARAMETERS
    private Rigidbody2D playerRb;
    public float jumpForce;
    public float jetpackForce;
    private bool isOnGround;
    public float charge;
    public GameObject projectile;

    private int hitPoints = 4;

    //GAME PARAMETERS
    public bool gameStarted;
    public bool gameOver;
    public bool bossAlive;
    public bool bossKilled;

    //PARTICLES
    public GameObject electricParticleObject;
    private ParticleSystem electricParticle;
    public GameObject jetpackParticleObject;
    private ParticleSystem jetpackParticle;

    //ANIMATIONS
    public Animator animator;

    //UI Elements
    public GameObject pressStart;
    public GameObject restartText;

    void Start()
    {
        isOnGround = false;
        gameStarted = false;
        gameOver = false;
        bossAlive = false;

        playerRb = GetComponent<Rigidbody2D>();
        electricParticle = electricParticleObject.GetComponent<ParticleSystem>();
        electricParticle.Play();
        jetpackParticle = jetpackParticleObject.GetComponent<ParticleSystem>();
        jetpackParticle.Play();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(gameStarted == false)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                gameStarted = true;
                pressStart.SetActive(false);
            }
        }
        
        if(gameStarted == true)
        {
            InputCheck();
            OOBCheck();

            if (bossKilled == true)
            {
                var emission = electricParticle.emission;
                emission.rateOverTime = 0;
            }
        }

        if(gameOver == true)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }


    //USER INPUT
    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameOver == false && bossKilled == false)
        {
            StartCoroutine(TapInput());
        }
    }

    IEnumerator TapInput()
    {
        //GIVE THE USER A MOMENT TO LET GO IF TAPPING
        yield return new WaitForSeconds(tapThreshold);

        //TAP
        if (Input.GetKey(KeyCode.Space) == false)
        {
            Shoot();
        }

        //HOLD
        while (Input.GetKey(KeyCode.Space) && bossKilled == false)
        {
            if(isOnGround)
            {
                Jump();
                yield return new WaitForSeconds(.2f); //Dont turn the jetpack on instantly after jumping
            }
            
            JetPack();
            yield return new WaitForEndOfFrame();
            animator.SetBool("isFloating", true);
        }

        animator.SetBool("isFloating", false);

        var emission = jetpackParticle.emission;
        emission.rateOverTime = 0;
    }

    //COLLISION
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnGround = true;
        animator.SetBool("isJumping", false);

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Enemy Projectile"))
        {
            animator.SetBool("isHurt", true);
            Destroy(collision.gameObject);
            hitPoints--;
            hpCheck();
            StartCoroutine("AnimOff");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isOnGround = true;

        if (collision.collider.CompareTag("Electric") && gameOver == false && bossKilled == false)
        {
            var emission = electricParticle.emission;
            emission.rateOverTime = 40;
            
            electricParticleObject.transform.position = new Vector2(electricParticleObject.transform.position.x, collision.transform.position.y);
            ChargeUp();
        }

        if (collision.collider.CompareTag("BuildingSide"))
        {
            hitPoints = 0;
            gameOver = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnGround = false;

        var emission = electricParticle.emission;
        emission.rateOverTime = 0;
    }

    private void OOBCheck()
    {
        if(transform.position.y < -6)
        {
            gameOver = true;
            restartText.SetActive(true);
        }
    }

    //CHARACTER ACTIONS
    void Jump()
    {
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetBool("isJumping", true);
    }

    void JetPack()
    {
        if(charge > 0)
        {
            float t = Time.deltaTime;
            charge -= t;
            playerRb.AddForce(Vector2.up * jetpackForce * t, ForceMode2D.Force);

            var emission = jetpackParticle.emission;
            emission.rateOverTime = 20;
        }
    }

    void Shoot()
    {
        if(charge >= 1)
        {
            animator.SetBool("isShooting", true);
            charge = charge - 1;
            Instantiate(projectile, transform.position + new Vector3(1,0,0), Quaternion.identity);
            StartCoroutine("AnimOff");
        }
    }

    IEnumerator AnimOff()
    {
        yield return new WaitForSeconds(.3f);
        animator.SetBool("isShooting", false);
        animator.SetBool("isHurt", false);
    }

    void ChargeUp()
    {
        if(charge <= 5)
        {
            charge += 3*Time.deltaTime;
            playerRb.WakeUp();
        }
    }

    void hpCheck()
    {
        if (hitPoints == 3)
        {
            GameObject.Find("HP4").SetActive(false);
        }
        else if (hitPoints == 2)
        {
            GameObject.Find("HP3").SetActive(false);
        }
        else if(hitPoints == 1)
        {
            GameObject.Find("HP2").SetActive(false);
        }
        else if(hitPoints == 0)
        {
            GameObject.Find("HP1").SetActive(false);
            animator.SetBool("isDead", true);
            gameOver = true;
            restartText.SetActive(true);
        }
    }
}

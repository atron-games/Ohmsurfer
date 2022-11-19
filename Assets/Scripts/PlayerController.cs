using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //INPUT PARAMETERS
    private float tapThreshold = 0.1f;

    //PLAYER PARAMETERS
    private Rigidbody2D playerRb;
    public float jumpForce;
    public float jetpackForce;
    private bool isOnGround = false;
    public float charge = 1;
    public GameObject projectile;

    private int hitPoints = 3;

    //GAME PARAMETERS
    public bool gameOver = false;

    //PARTICLES
    public GameObject electricParticleObject;
    private ParticleSystem electricParticle;
    public GameObject jetpackParticleObject;
    private ParticleSystem jetpackParticle;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        electricParticle = electricParticleObject.GetComponent<ParticleSystem>();
        electricParticle.Play();
        jetpackParticle = jetpackParticleObject.GetComponent<ParticleSystem>();
        jetpackParticle.Play();
    }

    void Update()
    {
        InputCheck();
        OOBCheck();
    }


    //USER INPUT
    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameOver == false)
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
        while (Input.GetKey(KeyCode.Space))
        {
            if(isOnGround)
            {
                Jump();
                yield return new WaitForSeconds(.2f); //Dont turn the jetpack on instantly after jumping
            }
            
            JetPack();
            yield return new WaitForEndOfFrame();
        }

        var emission = jetpackParticle.emission;
        emission.rateOverTime = 0;
    }

    //COLLISION
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnGround = true;

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Enemy Projectile"))
        {
            Destroy(collision.gameObject);
            hitPoints--;
            hpCheck();
        }

        if(collision.collider.CompareTag("Electric"))
        {
            var emission = electricParticle.emission;
            emission.rateOverTime = 40;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Electric"))
        {
            ChargeUp();
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
        }
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
            charge = charge - 1;
            Instantiate(projectile, transform.position + new Vector3(1,0,0), Quaternion.identity);
        }
    }

    void ChargeUp()
    {
        if(charge <= 3)
        {
            charge += 2*Time.deltaTime;
            playerRb.WakeUp();
        }
    }

    void hpCheck()
    {
        if(hitPoints == 2)
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
            gameOver = true;
        }
    }
}

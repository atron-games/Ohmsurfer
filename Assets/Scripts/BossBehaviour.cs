using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private int shots;
    private float seconds;

    private bool atTop;
    private bool atBottom;
    private bool inMiddle;

    public GameObject enemyProjectile;
    public GameObject phase2;

    private int bossHP = 8;
    public bool victory = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BossAction");
    }

    // Update is called once per frame
    void Update()
    {
        PositionCheck();
        //GetInputs();
    }

    IEnumerator BossAction()
    {
        yield return new WaitForSeconds(2);

        while (true)
        {
            StartCoroutine("PatternB");
            if (bossHP < 5) { break; }
            yield return new WaitForSeconds(5);

            StartCoroutine("MoveToMiddle");
            if (bossHP < 5) { break; }
            yield return new WaitForSeconds(2);

            StartCoroutine("PatternBInverted");
            if (bossHP < 5) { break; }
            yield return new WaitForSeconds(5);


            StartCoroutine("MoveToMiddle");
            if (bossHP < 5) { break; }
            yield return new WaitForSeconds(2);

        }

        StartCoroutine("MoveToMiddle");
        GameObject.Find("Phase1").SetActive(false);
        phase2.SetActive(true);
        yield return new WaitForSeconds(2);

        while (true)
        {
            StartCoroutine("PatternA");
            yield return new WaitForSeconds(3.2f);
            if (bossHP == 0) { break; }

            StartCoroutine("MoveToMiddle");
            yield return new WaitForSeconds(2);
            if (bossHP == 0) { break; }

            StartCoroutine("PatternAInverted");
            yield return new WaitForSeconds(3.2f);
            if (bossHP == 0) { break; }

            StartCoroutine("MoveToMiddle");
            yield return new WaitForSeconds(2);
            if (bossHP == 0) { break; }
        }

        Destroy(gameObject);
        victory = true;
    }
    
    void GetInputs()
    {
        //BOSS MOVEMENT MANUAL TEST
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine("MoveToTop");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine("MoveToMiddle");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("MoveToBottom");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("PatternA");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine("PatternB");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine("PatternAInverted");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine("PatternBInverted");
        }
    }

    //BOSS RECIEVE DAMAGE
    private void OnCollisionEnter2D(Collision2D collision)
    {  
        if (collision.collider.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            bossHP--;
            Debug.Log(bossHP);
        }
    }

    //BASIC MOVEMENT METHODS
    void PositionCheck()
    {
        if (transform.position.y == 3)
        {
            atTop = true;
        }
        else
        {
            atTop = false;
        }

        if (transform.position.y == -3)
        {
            atBottom = true;
        }
        else
        {
            atBottom = false;
        }

        if (transform.position.y == 0)
        {
            inMiddle = true;
        }
        else
        {
            inMiddle = false;
        }
    }

    IEnumerator MoveToTop()
    {
        while(true)
        {
            yield return new WaitForSeconds(.01f);
            transform.position += Vector3.up * Time.deltaTime * 20;

            if (transform.position.y >= 3)
            {
                transform.position = new Vector3(transform.position.x, 3, transform.position.z);
                break;
            }
        }
    }
    
    IEnumerator MoveToBottom()
    {
        while(true)
        {
            yield return new WaitForSeconds(.01f);
            transform.position += Vector3.down * Time.deltaTime * 20;

            if (transform.position.y <= -3)
            {
                transform.position = new Vector3(transform.position.x, -3, transform.position.z);
                break;
            }
        }
    }

    IEnumerator MoveToMiddle()
    {
        if(transform.position.y > 0)
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                transform.position += Vector3.down * Time.deltaTime * 20;

                if (transform.position.y <= 0)
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    break;
                }
            }
        }
        else
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                transform.position += Vector3.up * Time.deltaTime * 20;

                if (transform.position.y >= 0)
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    break;
                }
            }
        }
    }

    //SHOOTING
    void Shoot()
    {
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    }


    IEnumerator ShootOverSeconds()
    {
        float s = shots;
        int shotsTemp = shots;

        Debug.Log(shots + " " + seconds);

        while (shotsTemp > 0)
        {
            Shoot();
            yield return new WaitForSeconds(seconds / s);
            shotsTemp--;
        }

        yield return 0;
    }

    //SHOOTING PATTERNS
    IEnumerator PatternA()
    {
        shots = 7;
        seconds = 2.4f;

        if (inMiddle)
        {
            StartCoroutine("MoveToTop");
            yield return new WaitForSeconds(.9f);
        }
        else if(atBottom)
        {
            StartCoroutine("MoveToTop");
            yield return new WaitForSeconds(1.8f);
        }

        StartCoroutine("ShootOverSeconds");
        StartCoroutine("MoveToBottom");

        while (true)
        {
            if (transform.position.y <= -3)
            {
                StartCoroutine("MoveToTop");
                yield return new WaitForSeconds(1.8f);
                break;
            }
            yield return 0;
        }
    }

    IEnumerator PatternAInverted()
    {
        shots = 7;
        seconds = 2.4f;

        if (inMiddle)
        {
            StartCoroutine("MoveToBottom");
            yield return new WaitForSeconds(.9f);
        }
        else if (atTop)
        {
            StartCoroutine("MoveToBottom");
            yield return new WaitForSeconds(1.8f);
        }

        StartCoroutine("ShootOverSeconds");
        StartCoroutine("MoveToTop");

        while (true)
        {
            if (transform.position.y >= 3)
            {
                StartCoroutine("MoveToBottom");
                yield return new WaitForSeconds(1.8f);
                break;
            }
            yield return 0;
        }
    }

    IEnumerator PatternB()
    {
        shots = 5;
        seconds = .5f;

        if (inMiddle)
        {
            StartCoroutine("MoveToBottom");
            yield return new WaitForSeconds(.9f);
        }
        else if (atTop)
        {
            StartCoroutine("MoveToBottom");
            yield return new WaitForSeconds(1.8f);
        }

        StartCoroutine("ShootOverSeconds");
        yield return new WaitForSeconds(seconds);

        StartCoroutine("MoveToMiddle");
        yield return new WaitForSeconds(1);

        StartCoroutine("ShootOverSeconds");
        yield return new WaitForSeconds(seconds);

        StartCoroutine("MoveToTop");
        yield return new WaitForSeconds(1);

        StartCoroutine("ShootOverSeconds");
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator PatternBInverted()
    {
        shots = 5;
        seconds = .5f;

        if (inMiddle)
        {
            StartCoroutine("MoveToTop");
            yield return new WaitForSeconds(.9f);
        }
        else if (atBottom)
        {
            StartCoroutine("MoveToTop");
            yield return new WaitForSeconds(1.8f);
        }

        StartCoroutine("ShootOverSeconds");
        yield return new WaitForSeconds(seconds);

        StartCoroutine("MoveToMiddle");
        yield return new WaitForSeconds(1);

        StartCoroutine("ShootOverSeconds");
        yield return new WaitForSeconds(seconds);

        StartCoroutine("MoveToBottom");
        yield return new WaitForSeconds(1);

        StartCoroutine("ShootOverSeconds");
        yield return new WaitForSeconds(seconds);
    }
}

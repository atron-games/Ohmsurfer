using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour2 : MonoBehaviour
{
    public GameObject phase1Sprite;
    public GameObject phase2Sprite;
    public GameObject enemyProjectilePrefab;
    public PlayerController pcScript;

    public int bossMaxHP;
    private int bossHP;
    private int phaseBreakHP;
    public float bossMoveSpeed;
    private int shots;
    private float shotTimer;
    private bool doneShooting;

    private int phase = 0;
    private bool routineRunning = false;
    public bool bossKilled = false;

    public bool atTop;
    public bool inMiddle;
    public bool atBottom;
    
    // Start is called before the first frame update
    void Start()
    {
        pcScript = GameObject.Find("Player").GetComponent<PlayerController>();
        bossHP = bossMaxHP;
        phaseBreakHP = bossMaxHP / 2;

        StartCoroutine("BossAction");
    }

    // Update is called once per frame
    void Update()
    {
        PositionCheck();
        //GetInputs();
        Debug.Log(phase);
    }

    //DEBUG MANUAL INPUTS
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
        //    StartCoroutine("PatternA");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
        //    StartCoroutine("PatternB");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine("RoutineB");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
        //    StartCoroutine("PatternBInverted");
        }
    }

    //MOVEMENT METHODS
    private void PositionCheck()
    {
        //Spawn Sequence
        if (transform.position.x > 6.3)
        { transform.Translate(Vector3.left * Time.deltaTime * 4); }

        //Positional Checks
        if (transform.position.y == 3) { atTop = true; }
        else { atTop = false; }

        if (transform.position.y == -3) { atBottom = true; }
        else { atBottom = false; }

        if (transform.position.y == 0) { inMiddle = true; }
        else { inMiddle = false; }
    }

    IEnumerator MoveToTop()
    {
        while (bossHP > phaseBreakHP)
        {
            transform.position += Vector3.up * Time.deltaTime * bossMoveSpeed;
            yield return new WaitForEndOfFrame();

            if (transform.position.y >= 3)
            {
                transform.position = new Vector3(transform.position.x, 3, transform.position.z);
                break;
            }
        }
    }

    IEnumerator MoveToBottom()
    {
        while (bossHP > phaseBreakHP)
        {
            transform.position += Vector3.down * Time.deltaTime * bossMoveSpeed;
            yield return new WaitForEndOfFrame();

            if (transform.position.y <= -3)
            {
                transform.position = new Vector3(transform.position.x, -3, transform.position.z);
                break;
            }
        }
    }
    IEnumerator MoveToMiddle()
    {
        if (transform.position.y > 0)
        {
            while (bossHP > phaseBreakHP)
            {
                transform.position += Vector3.down * Time.deltaTime * bossMoveSpeed;
                yield return new WaitForEndOfFrame();

                if (transform.position.y <= 0)
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    break;
                }
            }
        }
        else
        {
            while (bossHP > phaseBreakHP)
            {
                transform.position += Vector3.up * Time.deltaTime * bossMoveSpeed;
                yield return new WaitForEndOfFrame();

                if (transform.position.y >= 0)
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    break;
                }
            }
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

    //BOSS CONTROLLER
    IEnumerator BossAction()
    {
        yield return new WaitForSeconds(2);
        
        //PHASE 1
        while(phase == 0)
        {
            if(routineRunning == false)
            {
                StartCoroutine("RoutineA");
            }

            if (bossHP <= phaseBreakHP)
            {
                phase++;
            }

            yield return 0;
        }

        //PHASE CHANGE SEQUENCE
        phaseBreakHP = 0;
        StartCoroutine("MoveToMiddle");
        phase1Sprite.SetActive(false);
        phase2Sprite.SetActive(true);
        yield return new WaitForSeconds(3);

        //PHASE 2
        while(phase == 1)
        {
            if (routineRunning == false)
            {
                StartCoroutine("RoutineB");
            }

            if (bossHP <= phaseBreakHP)
            {
                phase++;
            }

            yield return 0;
        }

        //BOSS DEFEATED
        if (phase == 2)
        {
            pcScript.bossKilled = true;
            Destroy(gameObject);
        }

        yield return null;
    }

    //BOSS ATTACKS AND SEQUENCES
    private void Shoot()
    {
        Instantiate(enemyProjectilePrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    }

    IEnumerator BurstFire()
    {
        doneShooting = false;
        
        int shotsTemp = shots;
        while (shotsTemp >= 0)
        {
            if (bossHP <= phaseBreakHP) { break; }
            Shoot();
            yield return new WaitForSeconds(shotTimer / shots);
            shotsTemp--;
        }

        doneShooting = true;
        yield return null;
    }

    IEnumerator RoutineA()
    {
        routineRunning = true;

        float t = 0;
        shots = 5;
        shotTimer = .5f;

        StartCoroutine("MoveToTop");

        while(true)
        {
            if (bossHP <= phaseBreakHP) { break; }
            if (atTop)
            {
                StartCoroutine("BurstFire");
                break;
            }
            yield return null;
        }

        while(true)
        {
            if (bossHP <= phaseBreakHP) { break; }
            if (doneShooting)
            {
                StartCoroutine("MoveToMiddle");
                break;
            }
            yield return null;
        }

        while (true)
        {
            if (bossHP <= phaseBreakHP) { break; }
            if (inMiddle)
            {
                StartCoroutine("BurstFire");
                break;
            }
            yield return null;
        }

        while (true)
        {
            if (bossHP <= phaseBreakHP) { break; }
            if (doneShooting)
            {
                StartCoroutine("MoveToBottom");
                break;
            }
            yield return null;
        }

        while (true)
        {
            if (bossHP <= phaseBreakHP) { break; }
            if (atBottom)
            {
                StartCoroutine("BurstFire");
                break;
            }
            yield return null;
        }


        while (true)
        {
            if (bossHP <= phaseBreakHP) { break; }
            if (doneShooting)
            {
                StartCoroutine("MoveToMiddle");
                break;
            }
            yield return null;
        }

        while (bossHP > phaseBreakHP)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (t >= 3) { break; }
        }

        routineRunning = false;
        yield break;
    }
    
    IEnumerator RoutineB()
    {
        routineRunning = true;

        float t = 0;
        shots = 8;
        shotTimer = 12 / bossMoveSpeed;

        StartCoroutine("MoveToTop");

        while (true)
        {
            if (atTop)
            {
                StartCoroutine("BurstFire");
                StartCoroutine("MoveToBottom");
                break;
            }
            yield return null;
        }

        while (true)
        {
            if (atBottom)
            {
                StartCoroutine("MoveToTop");
                break;
            }
            yield return null;
        }

        while (true)
        {
            if (atTop)
            {
                StartCoroutine("MoveToMiddle");
                break;
            }
            yield return null;
        }

        while (bossHP > phaseBreakHP)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if(t >= 3) { break; }
        }

        routineRunning = false;
        yield break;
    }

}

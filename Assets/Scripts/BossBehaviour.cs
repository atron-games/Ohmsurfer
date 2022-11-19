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

    private GameObject gun;
    public GameObject enemyProjectile;
    
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.Find("Gun");
    }

    // Update is called once per frame
    void Update()
    {
        PositionCheck();
        
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
            shots = 7;
            seconds = 3.6f;
            StartCoroutine("PatternA");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            shots = 5;
            seconds = .5f;
            StartCoroutine("PatternB");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

        }

    }

    void PositionCheck()
    {
        if (gun.transform.position.y == 3)
        {
            atTop = true;
        }
        else
        {
            atTop = false;
        }

        if (gun.transform.position.y == -3)
        {
            atBottom = true;
        }
        else
        {
            atBottom = false;
        }

        if (gun.transform.position.y == 0)
        {
            inMiddle = true;
        }
        else
        {
            inMiddle = false;
        }

        Debug.Log(atTop + " " + inMiddle + " " + atBottom);
    }

    void Shoot()
    {
            Instantiate(enemyProjectile, gun.transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    }


    IEnumerator ShootOverSeconds()
    {
        float s = shots;
        int shotsTemp = shots;
        
        while(shotsTemp > 0)
        {
            Shoot();
            yield return new WaitForSeconds(seconds / s);
            shotsTemp--;
        }
        
        yield return 0;
    }
    
    IEnumerator MoveToTop()
    {
        while(true)
        {
            yield return new WaitForSeconds(.01f);
            gun.transform.position += Vector3.up * Time.deltaTime * 20;

            if (gun.transform.position.y >= 3)
            {
                gun.transform.position = new Vector3(gun.transform.position.x, 3, gun.transform.position.z);
                break;
            }
        }
    }
    
    IEnumerator MoveToBottom()
    {
        while(true)
        {
            yield return new WaitForSeconds(.01f);
            gun.transform.position += Vector3.down * Time.deltaTime * 20;

            if (gun.transform.position.y <= -3)
            {
                gun.transform.position = new Vector3(gun.transform.position.x, -3, gun.transform.position.z);
                break;
            }
        }
    }


    IEnumerator MoveToMiddle()
    {
        if(gun.transform.position.y > 0)
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                gun.transform.position += Vector3.down * Time.deltaTime * 20;

                if (gun.transform.position.y <= 0)
                {
                    gun.transform.position = new Vector3(gun.transform.position.x, 0, gun.transform.position.z);
                    break;
                }
            }
        }
        else
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);
                gun.transform.position += Vector3.up * Time.deltaTime * 20;

                if (gun.transform.position.y >= 0)
                {
                    gun.transform.position = new Vector3(gun.transform.position.x, 0, gun.transform.position.z);
                    break;
                }
            }
        }
    }

    IEnumerator PatternA()
    {
        if(atTop == false)
        {
            StartCoroutine("MoveToTop");
        }

        else
        {
            StartCoroutine("ShootOverSeconds");
            StartCoroutine("MoveToBottom");

            while(true)
            {
                if (gun.transform.position.y <= -3)
                {
                    StartCoroutine("MoveToTop");
                    yield return new WaitForSeconds(1.8f);
                    StartCoroutine("MoveToMiddle");
                    break;
                }
                yield return 0;
            }
        }
    }


    IEnumerator PatternB()
    {
        if(atBottom == false)
        {
            StartCoroutine("MoveToBottom");
        }

        else
        {
            StartCoroutine("ShootOverSeconds");
            yield return new WaitForSeconds(1);

            StartCoroutine("MoveToMiddle");
            yield return new WaitForSeconds(1);

            StartCoroutine("ShootOverSeconds");
            yield return new WaitForSeconds(1);

            StartCoroutine("MoveToTop");
            yield return new WaitForSeconds(1);

            StartCoroutine("ShootOverSeconds");
            yield return new WaitForSeconds(1);

            StartCoroutine("MoveToMiddle");
            yield return new WaitForSeconds(1);
        }
    }
}

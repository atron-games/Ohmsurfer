using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float tapThreshold;
    public float jumpForce;
    public float jetpackForce;

    private Rigidbody2D playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(InputCheck());
        }
    }

    IEnumerator InputCheck()
    {
        yield return new WaitForSeconds(tapThreshold);
        bool isOnGround = true;

        if (Input.GetKey(KeyCode.Space) == false && isOnGround)
        {
            Debug.Log("Jump!");
            Jump();
        }

        while (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Floating...");
            JetPack();

            yield return 0;
        }

    }

    void Jump()
    {
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void JetPack()
    {
        playerRb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
    }
}

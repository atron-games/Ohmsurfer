using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatform : MonoBehaviour
{
    private Vector3 startPosX;
    public float backgroundScale;
    private float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(backgroundScale, backgroundScale, backgroundScale);
        repeatWidth = (GetComponent<BoxCollider>().size.x / 2) * backgroundScale;

        startPosX = transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -7)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 4);
        }

        {
            if (transform.position.x < startPosX.x - repeatWidth)
            {
                transform.position = new Vector2(startPosX.x, transform.position.y);
            }
        }
    }
}
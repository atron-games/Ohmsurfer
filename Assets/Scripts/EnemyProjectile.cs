using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    private float leftBound = -16f;

    public AudioSource projectileAudio;
    public AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        projectileAudio.PlayOneShot(shootSound);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(Random.Range(0.9f, 2.2f), 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

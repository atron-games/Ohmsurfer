using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeIndicators : MonoBehaviour
{
    private PlayerController playerControllerScript;
    public int indicatorIndex;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Indicator(playerControllerScript.charge);
    }

    void Indicator(float charge)
    {
        float i = indicatorIndex;
        float r = (1 - (charge - i));
        
        float g;
        if (charge - i > 1)
        {
            g = 1;
        }
        else
        {
            g = charge - i;
        }

        
        rend.material.color = new Color(r, g, 0, 1);
    }
}

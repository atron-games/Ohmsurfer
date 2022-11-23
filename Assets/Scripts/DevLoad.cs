using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevLoad : MonoBehaviour
{
    public float rotationSpeed;
    public float toggleSpeed;
    public float loadingTime;

    private float heldTime;

    private GameObject pivot;
    private GameObject holdSpace;

    // Start is called before the first frame update
    void Start()
    {
        pivot = GameObject.Find("pivotPoint");
        holdSpace = GameObject.Find("holdspace");

        StartCoroutine("HoldSpace");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) == true)
        {
            pivot.transform.Rotate(new Vector3(0, 0, -1) * rotationSpeed * Time.deltaTime);

            if (loadingTime > heldTime)
            {
                heldTime += Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            heldTime = 0;
        }

        Debug.Log(heldTime);
    }

    IEnumerator HoldSpace()
    {
        while(true)
        {
            holdSpace.SetActive(true);
            yield return new WaitForSeconds(toggleSpeed);
            holdSpace.SetActive(false);
            yield return new WaitForSeconds(toggleSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    public GameObject particleObj;
    public float runTime;
    public int probality;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        particleObj = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) % probality == 0)
        {
            particleObj.SetActive(true);
            time = Time.time + runTime;
        }

        if(Time.time > time)
        {
            particleObj.SetActive(false);
        }
    }
}

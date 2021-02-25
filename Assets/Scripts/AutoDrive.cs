using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDrive : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public float speedLimit;
    public float crushPower;
    public float radius;
    public GameObject uiController;
    private float timeTemp=0;
    private int behaviour;
    private GameObject[] targets;
    private GameObject target;
    private Vector3 goal;
    private GameObject closest;
    private GameObject random;
    private bool once = true;
    // Start is called before the first frame update
    void Start()
    {
        random = new GameObject();
        targets = GameObject.FindGameObjectsWithTag("Car");
        GameObject[] temp = new GameObject[targets.Length-1];
        int temp_i=0;
        for (int i = 0; i < targets.Length; i++)
        {
            if(targets[i] != this.gameObject)
            {

                temp[temp_i] = targets[i];
                temp_i++;
            }
        }
        targets = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -2 && once == true)
        {
            uiController.GetComponent<UIController>().CountDown();
            GetComponent<Rigidbody>().isKinematic = true;
            once = false;
        }
        if (Time.time > timeTemp)
        {
            behaviour = Random.Range(0,2);
            timeTemp = Time.time + 10;
            random.transform.position = RandPoint();
        }
        switch (behaviour)
        {
            case 0:
                GoEnemy();
                break;
            case 1:
                if(Vector3.Distance(transform.position,random.transform.position) < 2 ){
                    random.transform.position = RandPoint();
                }
                RotateTowards(random);
                break;
        }
       
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed * Time.deltaTime, ForceMode.Impulse);
        if (GetComponent<Rigidbody>().velocity.magnitude > speedLimit)
        {
            GetComponent<Rigidbody>().velocity = Vector3.Normalize(GetComponent<Rigidbody>().velocity) * speedLimit;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            Vector3 forceDirection = collision.gameObject.transform.position - transform.position;
            forceDirection.Normalize();
            //Debug.Log(collision.relativeVelocity.magnitude);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(crushPower * forceDirection * collision.relativeVelocity.magnitude, ForceMode.VelocityChange);
        }
       
    }

    

    private void GoEnemy()
    {
        target = ClosestTarget();
        RotateTowards(target);
    }

    private GameObject ClosestTarget()
    {
        
        float closestM = 100000;
        float temp;
        foreach(GameObject obj in targets)
        {
            temp = (obj.transform.position - transform.position).magnitude;
            if(temp < closestM)
            {
                closest = obj;
                closestM = temp;
            }
        }
        return closest;

    }

    private Vector3 RandPoint()
    {
        
        Vector3 point;
        point.y = 0;
        point.x = Random.Range(-radius, radius);
        point.z = Random.Range(-radius, radius);
        
        return point;

    }
    private void RotateTowards(GameObject obj)
    {
        

        Quaternion desiredRot = Quaternion.LookRotation(obj.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, turnSpeed/10 * Time.deltaTime);
        


    }
}

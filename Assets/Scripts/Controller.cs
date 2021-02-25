using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject wheel;
    public GameObject car;
    public GameObject cam;
    public float turnLimit;
    public float turnSpeed;
    public float speed;
    public float speedLimit;
    public float shakePower;
    public float shakeBackPower;
    public float crushPower;
    public GameObject uiController;
    private bool once = true;
    private float startPositionY;
    private float position;
    private float angle=0;
    private bool rotateBack = false;
    private bool once2 = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            Vector3 forceDirection = collision.gameObject.transform.position  - transform.position;
            forceDirection.Normalize();
            //Debug.Log(collision.relativeVelocity.magnitude);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(crushPower  * forceDirection * collision.relativeVelocity.magnitude,ForceMode.VelocityChange);
        }
        cam.transform.Rotate(shakePower*collision.relativeVelocity.magnitude*(transform.position -collision.transform.position));
        rotateBack = true;
        //Debug.Log(collision.relativeVelocity);
    }
   
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -2 && once2 == true)
        {
            uiController.GetComponent<UIController>().Fail();
            GetComponent<Rigidbody>().isKinematic = true;
            once2 = false;
        }

        if (rotateBack)
        {
            
            cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation,new Quaternion(0,0,0,1),shakeBackPower/100);
            
        }
        if (Input.GetMouseButton(0))
        {
            if (once)
            {
                once = false;
                startPositionY = Input.mousePosition.x;
            }
            
            position = (Input.mousePosition.x - startPositionY) * turnSpeed/100 * Time.deltaTime;
            if(position> turnLimit)
            {
                position = turnLimit;
            }
                
            if( position < -turnLimit)
            {
                position = -turnLimit;
            }
            
                wheel.transform.Rotate(new Vector3(0,0,angle -(Input.mousePosition.x - startPositionY)/5), Space.Self);
                angle = (Input.mousePosition.x - startPositionY)/5;


            
            car.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, position, 0), ForceMode.Impulse);
            car.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*speed*Time.deltaTime, ForceMode.Impulse);
            if (GetComponent<Rigidbody>().velocity.magnitude > speedLimit)
            {
                GetComponent<Rigidbody>().velocity = Vector3.Normalize(GetComponent<Rigidbody>().velocity) * speedLimit;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            once = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("raum"))
        {
        //    Debug.Log("entered");
            this.transform.position.Set(0, 0, 0);
            FixedUpdate();
        }
    }


    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        this.transform.position += z * transform.forward * Time.deltaTime ;
        this.transform.position += x * transform.right * Time.deltaTime ;
    }

    // stayCount allows the OnTriggerStay to be displayed less often
    // than it actually occurs.
    //private float stayCount = 0.0f;
    //private void OnTriggerStay(Collider other)
    //{
    //    if (true)
    //    {
    //        transform.position.Set(0, 0, 0);
    //        if (stayCount > 0.25f)
    //        {
    //            Debug.Log("staying");
    //            stayCount = stayCount - 0.25f;
    //        }
    //        else
    //        {
    //            stayCount = stayCount + Time.deltaTime;
    //        }
    //    }

    //}
}

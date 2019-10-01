using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFrame : MonoBehaviour
{
    public GameObject panel1;
    //public GameObject panel2;
    //public GameObject panel3;


    // Start is called before the first frame update
    void Start()
    {
        panel1.transform.localScale = new Vector3(panel1.transform.localScale.x * 0.5f, panel1.transform.localScale.y * 0.5f, panel1.transform.localScale.z * 0.5f);
    //    panel2.transform.localScale = new Vector3(panel2.transform.localScale.x * 0.5f, panel2.transform.localScale.y * 0.5f, panel2.transform.localScale.z * 0.5f);
    //    panel3.transform.localScale = new Vector3(panel3.transform.localScale.x * 0.6f, panel3.transform.localScale.y * 0.6f, panel3.transform.localScale.z * 0.6f);
    //
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Panel1()
    {

        panel1.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = true;
    //    panel2.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    //    panel3.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    //
    }

    //public void Panel2()
    //{

    //    panel1.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    //    panel2.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = true;
    //    panel3.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    //}

    //public void Panel3()
    //{

    //    panel1.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    //    panel2.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    //    panel3.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = true;
    //}


}

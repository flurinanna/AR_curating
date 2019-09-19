using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFrame : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Panel1()
    {

        panel1.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = true;
        panel2.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
        panel3.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    }

    public void Panel2()
    {

        panel1.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
        panel2.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = true;
        panel3.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
    }

    public void Panel3()
    {

        panel1.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
        panel2.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = false;
        panel3.GetComponent<UnityEngine.XR.iOS.UnityARHitTestExample>().enabled = true;
    }


}

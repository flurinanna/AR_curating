using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraDrag : MonoBehaviour
{
    public float speed = 0.001f;
    private float X;
    private float Y;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GameObject.FindGameObjectWithTag("canvas").GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            bool noCanvasHitted = true;
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            if(results.Count > 0)
            {
                noCanvasHitted = false;
            }

            if (!GameObject.Find("pictureholder").GetComponent<PictureGenerator>().dragging && noCanvasHitted) {
                Transform t = GameObject.Find("player").transform;
                float x = 0;
                float y = 0;
                #if UNITY_EDITOR
                x = Input.GetAxis("Mouse Y") * 10;
                y = -Input.GetAxis("Mouse X") * 10;
                #endif
                //Add touch support
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Touch touch = Input.touches[0];
                    y = -touch.deltaPosition.x;
                    x = touch.deltaPosition.y;
                    /*if(Input.touchCount == 2)
                    {
                        Touch touch2 = Input.touches[1];
                        //y += -touch2.deltaPosition.x;
                        x += touch2.deltaPosition.y;

                        x /= 2;
                        y = 0;
                    }*/

                    x /= 20;
                    y /= 20;

                } 

                Vector3 movement = new Vector3(x, y, 0);
                t.Rotate(movement);

            }
        }
    }
}

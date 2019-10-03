using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class PictureGenerator : MonoBehaviour
{
    public static Color GREEN = new Color(0, 255, 0, 1);
    public static Color RED = new Color(250, 0, 0, 1);
    public static Color TRANSPARENT = new Color(0, 0, 0, 0);

    public GameObject prefab;
    public int maxWallWidth;
    public int maxWallHeigth;
    public GameObject pic;
    public JsonLoader jsonLoader;
    public PicturesForTopic pft;

    public bool picMayBeClicked;
    public GameObject hittedPicture;
    public Vector3 screenHitPoint;
    public bool dragging;
    private Vector3 relativePicturePositionX;
    private Vector3 relativePicturePositionY;

    public bool picIsInitial;
    public PictureDetails pictureDetails;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    Camera cam;

    public CharacterController characterController;

// AR Additions
    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
//    public GameObject placementIndicator;


    private void Start() {
        if (PicturesOnWallData.exPicDictionary.Count > 0) {
            GenerateFromPicturesOnWallData();
        }
        pic = null;
        cam = Camera.main;

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GameObject.FindGameObjectWithTag("canvas").GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
//ARR Additions
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        //  UpdatePlacementIndicator();
        if (picIsInitial)
        {
            var screenCenter = cam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            aRRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;
            }

            //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);
            bool pictureNotPlacedYet = true;
            foreach (ARRaycastHit hit in hits)
            {
                if (pictureNotPlacedYet && pic != null)
                {
                    movePictureDirectly(pic, hit);
                    selectPicture(pic);
                    pictureNotPlacedYet = false;
                }
            }
            if (pictureNotPlacedYet)
            {   
                Material image = null;
                Transform tPlayer = GameObject.Find("AR Raycast Manager").transform;
                if (pic != null) 
                {
                    pic.transform.rotation = tPlayer.rotation;
                    pic.transform.Rotate(0, 90, 0);
                    image = pic.GetComponent<MeshRenderer>().materials[0];
                

                    if (image != null && image.mainTexture != null)
                    {
                        float scale = Math.Min(2000f / image.mainTexture.width, 1000f / image.mainTexture.height);
                        if (scale > 1)
                        {
                            pic.transform.position = tPlayer.position + tPlayer.forward * scale;
                        }
                        else {
                            pic.transform.position = tPlayer.position + tPlayer.forward * 2f;
                        }
                    }
                    else
                    {
                        pic.transform.position = tPlayer.position + tPlayer.forward * 0.5f;
                    }
                    pic.GetComponent<Outline>().OutlineColor = RED;
                }
            } else
            {
                if (Input.GetMouseButtonUp(0) && !pictureNotPlacedYet)
                {
                    savePicture(pic);
                    pic = null;
                    picIsInitial = false;
                }
            }
        }
        else if (Input.touchCount < 2) {
            if (pic == null && Input.GetMouseButtonDown(0))
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
                if (results.Count > 0)
                {
                    noCanvasHitted = false;
                }

                if (noCanvasHitted)
                {
                 //   Debug.Log("Button Down");
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject o = hit.collider.gameObject;
                        if (o.tag.Equals("picture"))
                        {
                            picMayBeClicked = true;
                 //           Debug.Log("Button Down on Picture");
                            screenHitPoint = Input.mousePosition;
                        }
                    }
                }
            }
            else if (pic == null && Input.GetMouseButtonUp(0))
            {
             //   Debug.Log("Button Up");
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);
                bool noPictureHittedYet = true;
                foreach (RaycastHit hit in hits)
                {
                    if (noPictureHittedYet) {
                        GameObject o = hit.collider.gameObject;
                        if (o.tag.Equals("picture") && Input.mousePosition.Equals(screenHitPoint))
                        {
                            noPictureHittedYet = false;
                            selectPicture(o);
                        }
                    }
                }

            }
            else if (pic != null && Input.GetMouseButtonDown(0))
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
                if (results.Count > 0)
                {
                    noCanvasHitted = false;
                }

                if (noCanvasHitted)
                {
               //     Debug.Log("Button Down, pic not null");
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);
                    bool noPictureHittedYet = true;
                    screenHitPoint = Input.mousePosition;
                    foreach (RaycastHit hit in hits)
                    {
                        if (noPictureHittedYet)
                        {
                            GameObject o = hit.collider.gameObject;
                            if (o == pic)
                            {
                                noPictureHittedYet = false;
                                picMayBeClicked = true;
                                dragging = true;
                                relativePicturePositionX = Quaternion.Euler(0, -o.transform.eulerAngles.y, 0) * Vector3.Scale(hit.point - o.transform.position, Vector3.Cross(o.transform.up, hit.normal).normalized);
                                relativePicturePositionY = Vector3.Scale(hit.point - o.transform.position, o.transform.up.normalized);

                            }
                        }
                    }
                }
            }
            else if (pic != null && Input.GetMouseButtonUp(0))
            {

                if (Input.mousePosition.Equals(screenHitPoint))
                {
                //    Debug.Log("clicked Pic not null");
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);
                    bool noPictureHittedYet = true;
                    foreach (RaycastHit hit in hits)
                    {
                        GameObject o = hit.collider.gameObject;

                        if (o.tag.Equals("picture") && noPictureHittedYet)
                        {
                            noPictureHittedYet = false;
                            if (o == pic)
                            {
                                savePicture(pic);
                                pic = null;
                            }
                            else
                            {
                                savePicture(pic);
                                pic = null;
                                selectPicture(o);
                            }
                        }
                    }

                    if (noPictureHittedYet)
                    {
                        savePicture(pic);
                        pic = null;
                    }
                }

                picMayBeClicked = false;
                dragging = false;
            }
            //else if(dragging && !picMayBeClicked && !UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("ViewExhibition"))
            //{
            //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //    RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);
            //    foreach (RaycastHit hit in hits)
            //    {
            //        if (hit.collider.gameObject.tag.Equals("raum"))
            //        {
            //            movePicture(pic, hit);
            //        }
            //    }

            //    //Rotation while dragging picture
            //    float rotationArea = 400f;
                
            //    if (Input.mousePosition.x > Screen.width - rotationArea)
            //    {
            //        characterController.transform.Rotate(new Vector3(0, 1, 0) * (Input.mousePosition.x - Screen.width + rotationArea) * 0.005f);
            //    }
            //    else if (Input.mousePosition.x < 0 + rotationArea)
            //    {
            //        characterController.transform.Rotate(new Vector3(0, 1, 0) * (Input.mousePosition.x - rotationArea) * 0.005f);
            //    }

            //    if (Input.mousePosition.y > Screen.height - rotationArea)
            //    {
            //        characterController.transform.Rotate(new Vector3(-1, 0, 0) * (Input.mousePosition.y - (Screen.height - rotationArea)) * 0.005f);
            //    }
            //    else if (Input.mousePosition.y < 0 + rotationArea)
            //    {
            //        characterController.transform.Rotate(new Vector3(-1, 0, 0) * (Input.mousePosition.y - rotationArea) * 0.005f);
            //    }
            //}

            if (picMayBeClicked)
            {
                if (!Input.mousePosition.Equals(screenHitPoint))
                {
                    picMayBeClicked = false;
                }
            }
        }
    }

    private void selectPicture(GameObject picture)
    {
        this.pic = picture;
        pic.GetComponent<Outline>().OutlineColor = GREEN;
        pictureDetails.createPictureDetails(picture.GetComponent<PictureExporter>().pictureInfo);
    }

    private void savePicture(GameObject picture)
    {
        // hinzufügen zu PicturesOnWallData
        if (!PicturesOnWallData.exPicDictionary.ContainsKey(picture.name))
        {
            ExhibitionPicture picEx = picture.GetComponent<PictureExporter>().GetExhibitionPicture();
            PicturesOnWallData.exPicDictionary.Add(picture.name, picEx);
        }
        else
        {
            ExhibitionPicture picEx = PicturesOnWallData.exPicDictionary[picture.name];
            Transform t = picture.transform;
            picEx.Position.X = t.position.x;
            picEx.Position.Y = t.position.y;
            picEx.Position.Z = t.position.z;
            picEx.Rotation.X = t.rotation.x;
            picEx.Rotation.Y = t.rotation.y;
            picEx.Rotation.Z = t.rotation.z;
            picEx.Rotation.W = t.rotation.w;
        }
        picture.GetComponent<Outline>().OutlineColor = TRANSPARENT;
        pictureDetails.hide();
    }

    public void saveCurrentPicture()
    {
        if (pic != null) {
            savePicture(this.pic);
        }
    }

    private void movePicture(GameObject picture, ARRaycastHit hit)
    {
        Transform t = picture.transform;
        t.rotation =  Quaternion.LookRotation(hit.pose.up, Vector3.up);
        t.Rotate(new Vector3(0, 90, 0));
        t.position = hit.pose.position;
        t.Translate(hit.pose.up * t.localScale.x * 0.5f);
        Vector3 rotated = Quaternion.Euler(0, t.eulerAngles.y, 0) * relativePicturePositionX;
        if (t.eulerAngles.y < 180)
        {
            rotated *= -1;
        }
        rotated += -relativePicturePositionY;
        t.Translate(rotated, Space.World);
    }


    private void movePictureDirectly(GameObject picture, ARRaycastHit hit)
    {
        picture.transform.rotation = Quaternion.LookRotation(hit.pose.up, Vector3.up);
        picture.transform.Rotate(new Vector3(0, 90, 0));
        picture.transform.position = hit.pose.position;
        picture.transform.Translate(hit.pose.up * picture.transform.localScale.x * 0.5f);
    }

    public void createPictureOnWall(Picture pictureInfo)
    {
        if (pic != null)
        {
            saveCurrentPicture();
        }

        Transform t = GameObject.Find("AR Raycast Manager").transform;
        GameObject pictureToPlace = Instantiate(prefab, t.position, Quaternion.identity) as GameObject;
        pictureToPlace.name = pictureInfo.Bildcode;
        pictureToPlace.transform.parent = GameObject.Find("pictureholder").transform;
        pictureToPlace.GetComponent<PictureExporter>().pictureInfo = pictureInfo;
        picIsInitial = true;
        this.pic = pictureToPlace;
        StartCoroutine(LoadFromLikeCoroutine(pictureToPlace));
    }

        public void createPictureOnWall(ExhibitionPicture exPic)
    {
        GameObject picture = Instantiate(prefab, new Vector3(exPic.Position.X, exPic.Position.Y, exPic.Position.Z), Quaternion.identity) as GameObject;
        picture.name = exPic.Picture.Bildcode;
        picture.transform.rotation = new Quaternion(exPic.Rotation.X, exPic.Rotation.Y, exPic.Rotation.Z, exPic.Rotation.W);
        picture.transform.parent = GameObject.Find("pictureholder").transform;
        picture.GetComponent<PictureExporter>().pictureInfo = exPic.Picture;
        StartCoroutine(LoadFromLikeCoroutine(picture));
    }

    private IEnumerator LoadFromLikeCoroutine(GameObject picture)
    {
     //   Debug.Log("Loading ...");
        Picture pictureInfo = picture.GetComponent<PictureExporter>().pictureInfo;

        string url = "http://160.85.252.106:8080/public/wall_" + pictureInfo.Bildcode;

        UnityWebRequest wwwLoader = UnityWebRequestTexture.GetTexture(url);

        yield return wwwLoader.SendWebRequest();

        if (wwwLoader.isNetworkError || wwwLoader.isHttpError)
        {
      //      Debug.Log(wwwLoader.error);
        }
        else
        {
       //     Debug.Log("Loaded ");
            Texture myTexture = ((DownloadHandlerTexture)wwwLoader.downloadHandler).texture;
            picture.GetComponent<Renderer>().material.mainTexture = myTexture;
            picture.transform.localScale = new Vector3(0.0001f, pictureInfo.height / 100, pictureInfo.width / 100);

            // löschen der alten bilder, laden der neuen preseletion
            if (PreselectionData.picSpriteDictionary.Count > 0) {
                PreselectionData.picSpriteDictionary[pictureInfo.Bildcode].active = false;
                foreach (Transform child in pft.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                jsonLoader.createPicturesForCurrentPreselection(PreselectionData.picSpriteDictionary);
            }
        }
    }

    private void GenerateFromPicturesOnWallData() {
        foreach (var kvp in PicturesOnWallData.exPicDictionary) {
            createPictureOnWall(kvp.Value);
        }
    }


    private void UpdatePlacementPose()
    {

    }


    //private void UpdatePlacementIndicator()
    //{
    //    if(placementPoseIsValid)
    //    {
    //        placementIndicator.SetActive(true);
    //        placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
    //    }
    //    else
    //    {
    //        placementIndicator.SetActive(false);
    //    }
    //}

}


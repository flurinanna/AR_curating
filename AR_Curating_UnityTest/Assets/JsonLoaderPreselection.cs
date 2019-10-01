using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System;

public class JsonLoaderPreselection : MonoBehaviour
{
    Dictionary<string, PictureSprite> picSpriteDic = new Dictionary<string, PictureSprite>();
    public PicturesForTopic picturesForTopic;
    UnityEngine.UI.Button button;
    Image NewImage;
    Image BorderImage;
    public GameObject ParentPanel; //Parent Panel you want the new Images to be children of
    public Preselection preselection;
    UnityEngine.UI.Outline outline;
    public DetailTextLoaderPreselection detailTextLoader;

    private List<GameObject> AllPictureObjects;

    void Start() {
        AllPictureObjects = new List<GameObject>();
        
        // load all pictures at start of scene
        if (AllPicturesData.picSpriteDictionary.Count < 1) {
        StartCoroutine(loadAllPicturesFromDb()); 
        }
        CreateAllPictures();
       
    }

    public void FilterByTopic(string topic) {
        // set all to active
        if (AllPictureObjects.Count > 0) {
            foreach (var obj in AllPictureObjects) {
                obj.SetActive(true);
            }
        }
        
        if (topic != "") 
        {
            foreach (var kvp in AllPicturesData.picSpriteDictionary)
            {   
                GameObject pic = GameObject.Find(kvp.Key);
                if (pic) {
                    if (kvp.Value.picture.Kategorie == topic) {
                        pic.SetActive(true);
                    } else {
                        pic.SetActive(false);   
                    }
                }
            }
        }
    }

    public IEnumerator searchPictureInDb(string searchString)
    {
        if(!searchString.Equals("")){
            if (picSpriteDic == null)
            {
                throw new ArgumentNullException(nameof(picSpriteDic));
            }

        //    Debug.Log("before UnityWebRequest");
            UnityWebRequest webRequest = UnityWebRequest.Get("http://160.85.252.106:8080/api/pictures?q=" + searchString);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //   Debug.Log(webRequest.error);
            }
            else
            {
                //get JSON from url
                string jsonString = JsonHelper.fixJson(webRequest.downloadHandler.text);
                Picture[] myObject = JsonHelper.FromJson<Picture>(jsonString);

                // Make a loop and get all tubmnails:
                // convert base64 to image (texture) and create sprite

                Dictionary<string, Picture> results = new Dictionary<string, Picture>();

                for (int i = 0; i < myObject.Length; i++)
                {
                    results.Add(myObject[i].Bildcode, myObject[i]);
                }

                foreach (var kvp in AllPicturesData.picSpriteDictionary)
                {
                    GameObject pic = GameObject.Find(kvp.Key);
                    if (pic)
                    {
                        if (pic.activeSelf)
                        {
                            if (!results.ContainsKey(kvp.Value.picture.Bildcode))
                            {
                                pic.SetActive(false);
                            }
                        }
                    }
                }

            }
        } else
        {
    //        Debug.Log("leere Suche");
        }
    }

    public void CreateAllPictures()
    {
        foreach (var kvp in AllPicturesData.picSpriteDictionary)
        {
            GameObject NewObj = new GameObject();
            NewObj.name = kvp.Key;
            NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
            button = NewObj.AddComponent<Button>();
            outline = NewObj.AddComponent<UnityEngine.UI.Outline>();
            outline.effectColor = new Color(0, 177, 0);
            outline.effectDistance = new Vector2(0, 0);
  
            AllPictureObjects.Add(NewObj);
            
            // Falls Bild an Wand hängt oder in Preselection ist
            if (PicturesOnWallData.exPicDictionary.ContainsKey(kvp.Key)) {
                outline.effectColor = new Color(177, 0, 0);
                outline.effectDistance = new Vector2(10, 10);
            } else if (PreselectionData.picSpriteDictionary.ContainsKey(kvp.Key)) {
                outline.effectDistance = new Vector2(10, 10);
            }

            button.onClick.AddListener(() =>
            {
                preselection.togglePreselection(kvp.Key, kvp.Value);
                detailTextLoader.createDetailText(kvp.Value.sprite, kvp.Value.picture);
            });

            NewImage.sprite = kvp.Value.sprite; //Set the Sprite of the Image Component on the new GameObject
            NewObj.GetComponent<RectTransform>().SetParent(ParentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
            NewObj.SetActive(true);
            NewImage.preserveAspect = true;
            NewImage.SetNativeSize();                 
        }
    }

     IEnumerator loadAllPicturesFromDb()
    {

    //    Debug.Log("before UnityWebRequest");
        UnityWebRequest webRequest = UnityWebRequest.Get("http://160.85.252.106:8080/api/pictures?q=");
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
     //       Debug.Log(webRequest.error);
        }
        else
        {
            //get JSON from url
            string jsonString = JsonHelper.fixJson(webRequest.downloadHandler.text);
            Picture[] myObject = JsonHelper.FromJson<Picture>(jsonString);

            // Make a loop and get all tubmnails:
            // convert base64 to image (texture) and create sprite
            for (int i = 0; i < myObject.Length; i++)
            {
                string b64_string = myObject[i].thumbnail;
                byte[] b64_bytes = System.Convert.FromBase64String(b64_string);

                Texture2D tex = new Texture2D(1, 1);
                bool isLoaded = tex.LoadImage(b64_bytes);
                PictureSprite pictureSprite = new PictureSprite();

                if (isLoaded)
                {
          //          Debug.Log(myObject[i].Kategorie);
                    Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    pictureSprite.picture = myObject[i];
                    pictureSprite.sprite = s;
                    AllPicturesData.picSpriteDictionary.Add(myObject[i].Bildcode, pictureSprite);
                }
            }
        }
        CreateAllPictures();
    }
}



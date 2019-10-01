using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System;

public class JsonLoader : MonoBehaviour
{
    Dictionary<Sprite, Picture> sprites = new Dictionary<Sprite, Picture>();
    public PicturesForTopic picturesForTopic;
    Button button;
    Image NewImage;
    public GameObject ParentPanel; //Parent Panel you want the new Images to be children of
    public PictureGenerator pictureGenerator;
    public DetailViewLoader detailViewLoader;
    public DetailTextLoader detailTextLoader;
    public PictureDetails pictureDetails;

    void Start()
    {
        createPicturesForCurrentPreselection(PreselectionData.picSpriteDictionary);
    }
            

    public void createPicturesForCurrentPreselection(Dictionary<string, PictureSprite> picSpriteDic)
    {
        float count = 405;

        foreach (var kvp in picSpriteDic)
        {
            if (kvp.Value.active) {
                GameObject NewObj = new GameObject();
                NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                button = NewObj.AddComponent<Button>();
                button.onClick.AddListener(() =>
                {
                 //   Debug.Log("picture for detailview wurde geklickt");
                    detailViewLoader.createDetailView(kvp.Value.picture);
                    detailTextLoader.createDetailText(kvp.Value.sprite, kvp.Value.picture);
                    pictureDetails.hide();
                });
                NewImage.sprite = kvp.Value.sprite; //Set the Sprite of the Image Component on the new GameObject
                NewObj.GetComponent<RectTransform>().SetParent(ParentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
                NewObj.SetActive(true);
                NewImage.preserveAspect = true;
                NewObj.transform.position = new Vector3(110, count, 0);
                NewImage.SetNativeSize();
                count = count - 145;

            }
        }
    }
}



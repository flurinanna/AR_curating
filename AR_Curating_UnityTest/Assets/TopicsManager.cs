using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicsManager : MonoBehaviour
{
    public List<Image> imagesForTopic = new List<Image>(); //List of Sprites added from the Editor to be created as GameObjects at runtime
    Dropdown dropdown;
    public PicturesForTopic picturesForTopic;
    public Button searchButton;
    public InputField inputField;


    void Start()
    {
        dropdown =  GetComponent<Dropdown>();
        searchButton.onClick.AddListener(() =>
        {
         //   Debug.Log("suche: " + inputField.text);
            GetSearchedTopic(inputField.text);
            inputField.text = "";
        });
    }

    public void GetActivatedTopic() {
        picturesForTopic.getPicturesForTopic(dropdown.value);
    }

    public void GetSearchedTopic(string searchString)
    {
        picturesForTopic.getPicturesForTopicByString(dropdown.value, searchString);
    }
}

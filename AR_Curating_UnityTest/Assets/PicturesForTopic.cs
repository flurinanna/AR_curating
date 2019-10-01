using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Windows.Input;
using UnityEngine.Networking;
using System;
using static UnityEngine.JsonUtility;
using System.IO;

public class PicturesForTopic : MonoBehaviour
{

    public Dictionary<string, PictureSprite> SpritesForTopic;
    public JsonLoaderPreselection jsonLoaderPreselection;


    void Start()
    {
        getPicturesForCurrentTopic(0);
    }



    public void getPicturesForTopic(int topicValue)
    {
   //     Debug.Log("getPicturesForTopic wird aufgerufen");
        
        getPicturesForCurrentTopic(topicValue);
    }

    public void getPicturesForTopicByString(int topic, string searchString)
    {
        getPicturesForCurrentTopic(topic);
        StartCoroutine(jsonLoaderPreselection.searchPictureInDb(searchString));      
    }


    private void getPicturesForCurrentTopic(int topicValue)
    {
        switch (topicValue)
        {
            case 0:
                if (jsonLoaderPreselection) {
                    jsonLoaderPreselection.FilterByTopic("");
                }
                break;
            case 1:
                jsonLoaderPreselection.FilterByTopic("19./20. Jahrhundert");
                break;
            case 2:
                jsonLoaderPreselection.FilterByTopic("21. Jahrhundert");
                break;
            case 3:
                jsonLoaderPreselection.FilterByTopic("Farbe");
                break;
            case 4:
                jsonLoaderPreselection.FilterByTopic("Siebdruck");
                break;
            case 5:
                jsonLoaderPreselection.FilterByTopic("Tiere");
                break;
            case 6:
                jsonLoaderPreselection.FilterByTopic("Körper in der Luft");
                break;
            case 7:
                jsonLoaderPreselection.FilterByTopic("Bernhard Luginbühl (1929 - 2011)");
                break;
            case 8:
                jsonLoaderPreselection.FilterByTopic("Verena Loewensberg (1912 - 1986)");
                break;
            case 9:
                jsonLoaderPreselection.FilterByTopic("Pablo Picasso (1881 - 1973)");
                break;
            case 10:
                jsonLoaderPreselection.FilterByTopic("Rembrandt Harmensz. van Rijn (1606 - 1669)");
                break;     
            case 11:
                break;
        }
    }



}






using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LikesManager : MonoBehaviour
{
    public Text likesCounter;
    private string uri = "http://160.85.252.106:8080/api/exhibitions/" + ExhibitionData.exhibition.id;
    bool starAdded = false;


    private void Start()
    {
        likesCounter.text = ExhibitionData.exhibition.Likes.ToString();
    }

    public void addStar()
    {
  //      Debug.Log(ExhibitionData.exhibition.id);
        if(!starAdded)
        {
            starAdded = true;
            ExhibitionData.exhibition.Likes++;
    //        Debug.Log("exhibition and stars of exhibition: " + ExhibitionData.exhibition.id + ExhibitionData.exhibition.Likes);
            likesCounter.text = ExhibitionData.exhibition.Likes.ToString();

            string jsonPayload = JsonConvert.SerializeObject(ExhibitionData.exhibition, Formatting.Indented);

            StartCoroutine(UpdateExhibitionToDB(jsonPayload));
        }
        else
        {
            return;
        }
    }

    private IEnumerator UpdateExhibitionToDB(string jsonPayload)
    {
   //     Debug.Log("payload to save: " + jsonPayload);

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonPayload))
        {
            // Request and wait for the desired page.
            webRequest.method = "Put";
    //        Debug.Log("url: " + webRequest.url);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
    //        Debug.Log("return von PUT data");

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
      //          Debug.Log(webRequest.url + ": Error: " + webRequest.error);
            }
            else
            {
     //           Debug.Log(webRequest.url + ": saving successful");
            }
        }
    }

}


using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class ExhibitionPersister : MonoBehaviour
{

    private string uri = "http://160.85.252.106:8080/api";
    //
    //private string uri = "http://localhost:8443/api"; //TODO: property for Host?

    // Start is called before the first frame update
    void Start()
    {
        //      Debug.Log("start .........");
        uri = uri + "/exhibitions";
    }

    public bool SaveExhibition(string author, string titel, string eMail)
    {

        DateTime time = DateTime.Now;
        var format = "dd.MM.yyyy, HH:mm";
        var timestamp = time.ToString(format);
        //       Debug.Log("timestamp" + timestamp);

        ExhibitionData.exhibition.Author = author;
        ExhibitionData.exhibition.Title = titel;
        ExhibitionData.exhibition.Timestamp = time;
        ExhibitionData.exhibition.Time = timestamp;
        ExhibitionData.exhibition.Email = eMail;

        foreach (var kvp in PicturesOnWallData.exPicDictionary)
        {
            if (PreselectionData.picSpriteDictionary.ContainsKey(kvp.Key))
            {
                PreselectionData.picSpriteDictionary[kvp.Key].active = true;
            }
            if (!ExhibitionData.exhibition.Pictures.Contains(kvp.Value))
            {
                ExhibitionData.exhibition.Pictures.Add(kvp.Value);
            }
        }
        string preJsonPayload = JsonConvert.SerializeObject(ExhibitionData.exhibition, Formatting.Indented);
        Exhibition x = JsonConvert.DeserializeObject<Exhibition>(preJsonPayload);

        foreach (ExhibitionPicture p in x.Pictures)
        {
            p.Picture.thumbnail = "";
        }

        string json = JsonConvert.SerializeObject(x, Formatting.Indented);

        if (SaveExhibitionToDB(json))
        {
            return true;
        }

        ExhibitionData.exhibition.Pictures.Clear();
        return false;
    }

    private bool SaveExhibitionToDB(string jsonPayload)
    {
        //    Debug.Log("payload to save: " + jsonPayload);

        UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonPayload);
        // Request and wait for the desired page.
        webRequest.method = "Post";
        //    Debug.Log("url: " + webRequest.url);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SendWebRequest();
        //     Debug.Log("return von POST data");

        while (!webRequest.isDone)
        {
            //            Debug.Log("Waiting for response...");
        }
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool getExhibitiions()
    {
        //StartCoroutine(
        getExhibitiionsFromDB();
        //);
        return true;
    }

    private IEnumerator getExhibitiionsFromDB()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.

            //     Debug.Log("url: " + webRequest.url);
            yield return webRequest.SendWebRequest();
            //     Debug.Log("return von GET data");

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                //          Debug.Log(webRequest.url + ": Error: " + webRequest.error);
            }
            else
            {
                //         Debug.Log(webRequest.url + ": collecting successful");
                string jsonString = fixJson(webRequest.downloadHandler.text);
                //        Debug.Log("response: " + jsonString);
                Exhibition[] exhibitions = JsonHelper.FromJson<Exhibition>(jsonString);
            }
        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ExhibitionModel : MonoBehaviour
{
    public GameObject prefab;

    public static ExhibitionModel I;

    public List<Exhibition> data = new List<Exhibition>();

    public ExhibitionLoader exLoader;
    UnityEngine.UI.Button button;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        GetSavedExhibitions();

    }

    public void InsertItem(int index, Exhibition data)
    {

        // set custom data
        I.data.Insert(index, data);

        //this.scrollView.totalItemCount = I.data.Count;
    }

    // public void onClick_InsertButton () {

    //     this.insertItem( int.Parse(this.indexInput.text), new CustomData{ name=this.titleInput.text, value=this.valueInput.text, on=true } );
    // }
    private void GetSavedExhibitions()
    {
   //     Debug.Log("Load Exhibitions");
        StartCoroutine(loadExhibitionsFromDb());

    }

    IEnumerator loadExhibitionsFromDb()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("http://160.85.252.106:8080/api/exhibitions");
        yield return webRequest.SendWebRequest();

        string jsonString = "failed";

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
   //         Debug.Log(webRequest.error);
        }
        else
        {
            //get JSON from url
            jsonString = webRequest.downloadHandler.text;
      //      Debug.Log(jsonString);

            Exhibition[] exhibitions = JsonConvert.DeserializeObject<Exhibition[]>(jsonString);

            foreach (Exhibition e in exhibitions)
            {
 //               Debug.Log(e.Author);
                this.InsertItem(0, e);
            }
            Populate();
        }
    }



    void Populate()
    {
        foreach (Exhibition ex in data)
        {
            GameObject NewObj = (GameObject)Instantiate(prefab, transform);
            NewObj.GetComponentInChildren<Text>().text = ex.Title + " von " + ex.Author + " \n" + "Erstellt am " + ex.Time + " Uhr.";
            NewObj.transform.Find("StarCounter").GetComponent<Text>().text = ex.Likes.ToString();

            bool isBase64 = false;
            if (ex.PreviewImg != null) {
                isBase64 = IsBase64String(ex.PreviewImg);
            }

            if (isBase64)
            {
                byte[] b64_bytes = System.Convert.FromBase64String(ex.PreviewImg);
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(b64_bytes);
                Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                NewObj.GetComponent<Image>().sprite = s;
            }
            else
            {
                NewObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(ex.PreviewImg);
            }

            button = NewObj.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                exLoader.LoadExhibition(ex);
            });
        }
    }

    public void SortByLikes()
    {
        DeleteExhibitionPreviewItems();
        data.Sort((x, y) => y.Likes.CompareTo(x.Likes));
        Populate();
    }

    public void SortByDate()
    {
        DeleteExhibitionPreviewItems();
        data.Sort((x, y) => y.Timestamp.CompareTo(x.Timestamp));
        Populate();
    }

    private void DeleteExhibitionPreviewItems()
    {

        var previewItems = GameObject.FindGameObjectsWithTag("exPreview");
   //     Debug.Log(previewItems);
        foreach (var item in previewItems)
        {
    //        Debug.Log(item);
            Destroy(item);
        }
    }

    private bool IsBase64String(string s)
    {
        s = s.Trim();
        return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

    }
}


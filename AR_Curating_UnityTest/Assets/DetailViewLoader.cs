using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DetailViewLoader : MonoBehaviour
{

    public GameObject prefab;
    Image image;
    Sprite sprite;
    public DetailViewManager1 detailViewManager;

    void Start()
    {
        image = GetComponent<Image>();
        image.transform.Translate(new Vector3(0, 0, 0));
        detailViewManager.hide();
    }

    public void createDetailView(Picture picture)
    {
        detailViewManager.show();
  //      Debug.Log("waitForButtonClick wird aufgerufen!");
        StartCoroutine(LoadFromLikeCoroutine(picture));
    }

    private IEnumerator LoadFromLikeCoroutine(Picture picture)
    {
        string b64_string = picture.thumbnail;
        byte[] b64_bytes = System.Convert.FromBase64String(b64_string);

        Texture2D tex = new Texture2D(1, 1);
        bool isLoaded = tex.LoadImage(b64_bytes);

        if (isLoaded)
        {
            Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            image.sprite = s;
            image.SetNativeSize();
            if(tex.height < tex.width)
            {
                image.rectTransform.sizeDelta = new Vector2(1000, (float)tex.height / (float)tex.width * 1000);
            } else
            {
                image.rectTransform.sizeDelta = new Vector2((float)tex.width / (float)tex.height * 1000, 1000);
            }

        }

   //     Debug.Log("Loading ...");

        string url = "http://160.85.252.106:8080/public/preview_" + picture.Bildcode;

        UnityWebRequest wwwLoader = UnityWebRequestTexture.GetTexture(url);

        yield return wwwLoader.SendWebRequest();

        if (wwwLoader.isNetworkError || wwwLoader.isHttpError)
        {
    //        Debug.Log(wwwLoader.error);
        }
        else
        {
    //        Debug.Log("Loaded ");
            Texture2D myTexture = ((DownloadHandlerTexture)wwwLoader.downloadHandler).texture;
            Sprite s = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 0.5f);
            image.sprite = s;

        }
    }
}

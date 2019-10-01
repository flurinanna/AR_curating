using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotGenerator : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public void captureScreenshot()
    {
        StartCoroutine(TakeScreenshot());

    }

    public IEnumerator TakeScreenshot()
    {
        // save currently selected picture
        if (GameObject.Find("pictureholder")) {
            GameObject.Find("pictureholder").GetComponent<PictureGenerator>().saveCurrentPicture();
        }
        // hide UI
        hide();
        yield return frameEnd;
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        // show UI
        show();
        // Verkleinern
        FilterMode mode = FilterMode.Trilinear;
        TextureScaler.scale(screenImage, 550, 410, mode);
        //Convert to jpg
        byte[] imageBytes = screenImage.EncodeToJPG(70);
        string base64Screenshot = System.Convert.ToBase64String(imageBytes);
        // Debug.Log("size of image: " + imageBytes.Length);
        // Debug.Log("base64 ist: " + base64Screenshot.Length);
        SaveScreenshot(base64Screenshot);


    }

    private void SaveScreenshot(string base64Screenshot)
    {
        ExhibitionData.exhibition = new Exhibition();
        ExhibitionData.exhibition.PreviewImg = base64Screenshot;

    }

    private void hide()
    {
        canvasGroup.alpha = 0f; //this makes everything transparent
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }

    private void show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}

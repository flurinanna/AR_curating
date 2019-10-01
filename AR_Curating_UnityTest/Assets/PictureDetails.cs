using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureDetails : MonoBehaviour
{
    public Button schliessen;
    public Button entfernen;
    public CanvasGroup canvasGroup;
    public Text text;
    public JsonLoader jsonLoader;
    public PicturesForTopic pft;

    void Start()
    {
        this.text = GetComponent<Text>();

        schliessen.onClick.AddListener(() =>
        {
            hide();
        });

        hide();
    }

    public void createPictureDetails(Picture picture)
    {
        if (entfernen != null) {
            entfernen.onClick.RemoveAllListeners();
            entfernen.onClick.AddListener(() =>
            {
                PreselectionData.picSpriteDictionary[picture.Bildcode].active = true;
                foreach (Transform child in pft.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                jsonLoader.createPicturesForCurrentPreselection(PreselectionData.picSpriteDictionary);
                hide();
                PicturesOnWallData.exPicDictionary.Remove(picture.Bildcode);
                GameObject toBeDestroyed = GameObject.Find(picture.Bildcode);
                if (toBeDestroyed) {
                    Destroy(toBeDestroyed);
                }
            });
        }
        text.text = "<b>Künstler:</b>" + "\n" + picture.Kuenstler + "\n\n" + 
                    "<b>Titel:</b>" + "\n" + picture.Titel + "\n\n" + 
                    "<b>Datierung:</b>" + "\n" + picture.Datierung + "\n\n" + 
                    "<b>Material/Technik:</b>" + "\n" + picture.Material_Technik + "\n\n" + 
                    "<b>Masse (in cm):</b>" + "\n" + picture.height + " x " + picture.width;
        show();
        
    }

    public void hide()
    {
        canvasGroup.alpha = 0f; //this makes everything transparent
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }

    public void show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}

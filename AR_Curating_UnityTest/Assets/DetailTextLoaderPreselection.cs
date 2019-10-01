using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailTextLoaderPreselection : MonoBehaviour
{
    Text text;
    public DetailViewManager1 detailViewManager;

    void Start()
    {
        text = GetComponent<Text>();
        detailViewManager.hide();
    }

    public void createDetailText(Sprite sprite, Picture picture)
    {
        detailViewManager.show();
        text.text = "<b>Künstler:</b>" + "\n" + picture.Kuenstler + "\n\n" + 
                    "<b>Titel:</b>" + "\n" + picture.Titel + "\n\n" + 
                    "<b>Datierung:</b>" + "\n" + picture.Datierung + "\n\n" + 
                    "<b>Material/Technik:</b>" + "\n" + picture.Material_Technik + "\n\n" + 
                    "<b>Masse (in cm):</b>" + "\n" + picture.height + " x " + picture.width;
    }
}

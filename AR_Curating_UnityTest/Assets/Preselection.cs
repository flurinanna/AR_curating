using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Preselection : MonoBehaviour
{
    public UIManagerScript uIManagerScript;
    GameObject lastClicked;
    UnityEngine.UI.Outline outline;

    public void togglePreselection(string bildcode, PictureSprite value)
    {
        lastClicked = GameObject.Find(bildcode);
        outline = lastClicked.GetComponent<UnityEngine.UI.Outline>();
        
        PictureSprite tempPicSp = PreselectionData.picSpriteDictionary.Values.Where(pic => pic.picture.Bildcode == value.picture.Bildcode).SingleOrDefault();

        if (PicturesOnWallData.exPicDictionary.Keys.Contains(bildcode)) {
            warningPictureOnWall(bildcode);
        }
        else if (tempPicSp == null)
        {
            PreselectionData.picSpriteDictionary.Add(bildcode, value);
       //     Debug.Log("picturelist count value " + PreselectionData.picSpriteDictionary.Values.Count);
       //     Debug.Log("picturelist count value " + PreselectionData.picSpriteDictionary.Keys.Count);
            outline.effectDistance = new Vector2(10, 10);
        }
        else
        {
            PreselectionData.picSpriteDictionary.Remove(bildcode);
            //    Debug.Log("picturelist count value " + PreselectionData.picSpriteDictionary.Values.Count);
            //    Debug.Log("picturelist count value " + PreselectionData.picSpriteDictionary.Keys.Count);
            outline.effectDistance = new Vector2(0, 0);
        }
    }

    private void warningPictureOnWall(string bildcode)
    {
       // Debug.Log("Frage trotzdem entfernen");
        NativeDialog dialog = new NativeDialog("Achtung", "Bild ist bereits platziert! Trotzdem entfernen?", new Action(() =>
        {
            PicturesOnWallData.exPicDictionary.Remove(bildcode);
            PreselectionData.picSpriteDictionary[bildcode].active = true;
            PreselectionData.picSpriteDictionary.Remove(bildcode);
            outline.effectDistance = new Vector2(0, 0);
            outline.effectColor = new Color(0, 177, 0);
        }));
        dialog.init();
    }
}

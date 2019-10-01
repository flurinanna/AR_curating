using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionLoader : MonoBehaviour
{
    public UIManagerScript uIManagerScript;
    GameObject lastClicked;


    public void LoadExhibition(Exhibition ex) {
        PicturesOnWallData.exPicDictionary.Clear();
     //   Debug.Log("zu ladende Ex: " + ex.Title);
        foreach (ExhibitionPicture picEx in ex.Pictures) {
           if(!PicturesOnWallData.exPicDictionary.ContainsKey(picEx.Picture.Bildcode)) {
                PicturesOnWallData.exPicDictionary.Add(picEx.Picture.Bildcode, picEx);
            }
        }
        ExhibitionData.exhibition = ex;
        uIManagerScript.ViewExhibition();
    }

}

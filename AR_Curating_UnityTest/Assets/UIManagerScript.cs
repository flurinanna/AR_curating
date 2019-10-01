using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public void LoadMainMenu()
    {
        // Reset Preselection and PicturesOnWall
        PreselectionData.picSpriteDictionary.Clear();
        PicturesOnWallData.exPicDictionary.Clear();
        foreach (var kvp in AllPicturesData.picSpriteDictionary)
        {
            kvp.Value.active = true;
        }
        SceneManager.LoadScene("WelcomeScreen");
    }

    public void StartExhibition()
    {
        if (PreselectionData.picSpriteDictionary.Count < 1 && PicturesOnWallData.exPicDictionary.Count < 1)
        {
            warningNoPicturesSelected();
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void LoadExhibitions()
    {
        // Reset Preselection and PicturesOnWall
        PreselectionData.picSpriteDictionary.Clear();
        PicturesOnWallData.exPicDictionary.Clear();

        SceneManager.LoadScene("Exhibitions");
    }

    public void ViewExhibition()
    {

        SceneManager.LoadScene("ViewExhibition");
    }

    public void LoadSaveScene()
    {
        SceneManager.LoadScene("SaveScreen");
    }

    public void SaveScene()
    {
        string titel = GameObject.FindGameObjectWithTag("ausstellungstitel").GetComponent<Text>().text;
        string author = GameObject.FindGameObjectWithTag("ausstellungsauthor").GetComponent<Text>().text;
        string eMail = GameObject.FindGameObjectWithTag("email").GetComponent<Text>().text;

        if (titel == "" || author == "") {
            warningEmptyFields();
        } else {
            bool sucess = false;
            sucess = GameObject.Find("ExPersister").GetComponent<ExhibitionPersister>().SaveExhibition(author, titel, eMail);
            if (sucess)
            {
                if(LanguageData.language == 1)
                {
                    NativeMessage message = new NativeMessage("Speichern erfolgreich", "Ihre Ausstellung wurde erfolgreich gespeichert", "Ok");
                    LoadExhibitions();
                }
                else
                {
                    NativeMessage message = new NativeMessage("Successfully saved", "Your exhibition was saved successfully", "Ok");
                    LoadExhibitions();
                }

            } else
            {
                if (LanguageData.language == 1)
                {
                    NativeMessage message = new NativeMessage("Achtung", "Ausstellung konnte nicht gespeichert werden. Bitte versuchen Sie es nocheinmal.", "Ok");
                }
                else
                {
                    NativeMessage message = new NativeMessage("Attention", "Exhibition could not be saved. Please try again.", "Ok");
                }
            }
        }
    }

    public void LoadPreselection()
    {
        GameObject ph = GameObject.Find("pictureholder");
        if (ph != null)
        {
            ph.GetComponent<PictureGenerator>().saveCurrentPicture();
        }
        SceneManager.LoadScene("ChoosePictures");
    }


    public void LoadARView()
    {
        SceneManager.LoadScene("SampleScene");
    }


    private void warningNoPicturesSelected()
    {
        if (LanguageData.language == 1)
        {
            NativeMessage message = new NativeMessage("Achtung", "Sie haben keine Bilder ausgewählt.", "Ok");
        }
        else
        {
            NativeMessage message = new NativeMessage("Attention", "You have not selected any works.", "Ok");
        }

    }


    public void warningPopupVerwerfen()
    {
        if (LanguageData.language == 1)
        {
            NativeDialog dialog = new NativeDialog("Achtung", "Wollen Sie Ihre Ausstellung wirklich verwerfen?", new Action(LoadMainMenu));
            dialog.init();
        }
        else
        {
            NativeDialog dialog = new NativeDialog("Attention", "Do you really want to discard your exhibition?", new Action(LoadMainMenu));
            dialog.init();
        }
    }

    public void warningPopupSpeichern()
    {
        if (PicturesOnWallData.exPicDictionary.Count > 0)
        {
            if (LanguageData.language == 1)
            {
                NativeDialog dialog = new NativeDialog("Achtung", "Wollen Sie Ihre Ausstellung speichern und veröffentlichen? Die Ausstellung kann anschliessend nicht mehr verändert werden.", new Action(LoadSaveScene));
                dialog.init();
            }
            else
            {
                NativeDialog dialog = new NativeDialog("Attention", "Do you want to save and publish your exhibition? The exhibition can not be changed afterwards.", new Action(LoadSaveScene));
                dialog.init();
            }
        }
        else
        {
            if (LanguageData.language == 1)
            {
                NativeMessage message = new NativeMessage("Achtung", "Leere Ausstellungen können nicht gespeichert werden.", "Ok");
            }
            else
            {
                NativeMessage message = new NativeMessage("Attention", "Empty exhibitions can not be saved.", "Ok");
            }
        }
    }

    private void warningEmptyFields() {
        if (LanguageData.language == 1)
        {
            NativeMessage message = new NativeMessage("Achtung", "Titel und Name sind Pflichtfelder.", "Ok");
        }
        else
        {
            NativeMessage message = new NativeMessage("Attention", "Title and name are required fields.", "Ok");
        }
    }

}
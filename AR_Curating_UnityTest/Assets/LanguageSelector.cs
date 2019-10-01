using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{

    public LanguageManager languageManager;
    Text lang;
    public UIManagerScript uIManagerScript;
   
     void Start()
    {
        lang = this.GetComponentInChildren<Text>();
        if (lang.text == "English")
        {
            lang.text = "Deutsch";
            LanguageData.language = 2;
            languageManager.setLanguage(2);
        }
        else if (lang.text == "Deutsch")
        {
            lang.text = "English";
            LanguageData.language = 1;
            languageManager.setLanguage(1);
        }
    }


    public void OnClick()
    {
        lang = this.GetComponentInChildren<Text>();
     //   Debug.Log(lang.text);
        if(lang.text == "English")
        {
            lang.text = "Deutsch";
            LanguageData.language = 2;
            languageManager.setLanguage(2);
        }
        else if (lang.text == "Deutsch")
        {
            lang.text = "English";
            LanguageData.language = 1;
            languageManager.setLanguage(1);
        }
    }
}

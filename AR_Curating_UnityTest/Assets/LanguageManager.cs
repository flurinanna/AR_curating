using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public int currentLanguageIndex = 1;
    string translations;
    string choosenLanguage;

    //languageKey und Deutsch/englische bezeichnung
    Dictionary<string, string> languageDictionary;
    Dictionary<string, Text> textMeshMapper;


    private void Start()
    {
        currentLanguageIndex = LanguageData.language;
        setLanguage(currentLanguageIndex);
     //   Debug.Log("currentLanguageIndex: " + currentLanguageIndex);
    }

    public void setLanguage(int languageIndex)
    {
     //   Debug.Log("setLanguage wird aufgerufen mit index: " + languageIndex);
        currentLanguageIndex = languageIndex;


        languageDictionary = new Dictionary<string, string>();
        textMeshMapper = new Dictionary<string, Text>();

        LanguageKey[] totranslates = GameObject.FindObjectsOfType<LanguageKey>();
        foreach (LanguageKey totranslate in totranslates)
        {
            if (totranslate.languageKey != "")
            {
                Text tm = totranslate.gameObject.GetComponent<Text>();
                if (tm != null)
                {
                    textMeshMapper.Add(totranslate.languageKey, tm);
               //     Debug.Log("added thextmeshmapper languagekey " + totranslate.languageKey);
                }
            }
        }
        translate();
    }


    private void translate()
    {
        StartCoroutine("loadTranslations");
    }

    IEnumerator loadTranslations()
    {

     //   Debug.Log("before UnityWebRequest");
        UnityWebRequest webRequest = UnityWebRequest.Get("http://160.85.252.106:8080/public/translations.tsv");
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
    //        Debug.Log(webRequest.error);
        }
        else
        {
            translations = webRequest.downloadHandler.text;
     //       Debug.Log("translations " + translations);
            parseLoadedTranslations();
            applyTranslations();
        }


    }

    private void parseLoadedTranslations()
    {

       string[] lines = translations.Split('\n');
        foreach(string line in lines)
        {
            string[] wordTranslations = line.Split('\t');
            if(wordTranslations.Length > currentLanguageIndex)
            {
                string word = wordTranslations[currentLanguageIndex];
                if (!languageDictionary.ContainsKey(wordTranslations[0]))
                {
                    languageDictionary.Add(wordTranslations[0], word); 
                }
                else
                {
                }
            }
        }
    }

    private void applyTranslations()
    {
        foreach (string key in languageDictionary.Keys)
        {
            if(textMeshMapper.ContainsKey(key))
            {
                Text tm = textMeshMapper[key];
                tm.text = languageDictionary[key];
            }

        }
    }

}

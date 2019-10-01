using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeDialog
{
    #region PUBLIC_VARIABLES
    string title;
    string message;
    string yesButton;
    string noButton;
    public Action fu;
    #endregion

    #region PUBLIC_FUNCTIONS
    public NativeDialog(string title, string message, Action fu)
    {
        this.title = title;
        this.message = message;
        this.yesButton = "Yes";
        this.noButton = "No";
        this.fu = fu;
    }

    public NativeDialog(string title, string message, string yesButtonText, string noButtonText, Action fu)
    {
        this.title = title;
        this.message = message;
        this.yesButton = yesButtonText;
        this.noButton = noButtonText;
        this.fu = fu;
    }

    public void init()
    {
#if (UNITY_IPHONE && !UNITY_EDITOR)
            IOSDialog dialog = IOSDialog.Create(title, message, yesButton, noButton, fu);
#endif
#if UNITY_EDITOR
        bool option = UnityEditor.EditorUtility.DisplayDialog(title, message, yesButton, noButton);
            switch(option) 
            {
                case true:
                 //   Debug.Log("Ja");
                    fu();
                    break;
                case false:
                 //   Debug.Log("Nein");
                    break;
                default:
                    break;
            }
    #endif
    }
    #endregion
}
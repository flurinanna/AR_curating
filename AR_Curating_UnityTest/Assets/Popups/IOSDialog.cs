using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOSDialog : MonoBehaviour
{
    #region DELEGATE
    public delegate void OnDialogPopupComplete(MessageState state);
    public static event OnDialogPopupComplete onDialogPopupComplete;
    #endregion

    #region DELEGATE_CALLS
    private void RaiseOnOnDialogPopupComplete(MessageState state)
    {
        if (onDialogPopupComplete != null)
        {
            onDialogPopupComplete(state);
        }
    }
    #endregion

    #region PUBLIC_VARIABLES
    public string title;
    public string message;
    public string yes;
    public string no;
    public string urlString;
    public Action fu;
    #endregion

    #region PUBLIC_FUNCTIONS
    // Constructor
    public static IOSDialog Create(string title, string message, Action fu)
    {
        return Create(title, message, "Yes", "No", fu);
    }

    public static IOSDialog Create(string title, string message, string yes, string no, Action fu)
    {
        IOSDialog dialog;
        dialog = new GameObject("IOSDialogPopUp").AddComponent<IOSDialog>();
        dialog.title = title;
        dialog.message = message;
        dialog.yes = yes;
        dialog.no = no;
        dialog.fu = fu;
        dialog.init();
        return dialog;
    }

    public void init()
    {
        IOSNative.showDialog(title, message, yes, no);
    }
    #endregion

    #region IOS_EVENT_LISTENER
    public void OnDialogPopUpCallBack(string buttonIndex)
    {
        int index = System.Convert.ToInt16(buttonIndex);
        switch (index)
        {
            case 0:
                fu();
                RaiseOnOnDialogPopupComplete(MessageState.YES);
                break;
            case 1:
                RaiseOnOnDialogPopupComplete(MessageState.NO);
                break;
        }
        Destroy(gameObject);
    }
    #endregion
}
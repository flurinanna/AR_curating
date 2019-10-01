using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeMessage
{
    #region PUBLIC_FUNCTIONS

    public NativeMessage(string title, string message)
    {
        init(title, message, "Ok");
    }

    public NativeMessage(string title, string message, string ok)
    {
        init(title, message, ok);
    }

    private void init(string title, string message, string ok)
    {
#if (UNITY_IPHONE && !UNITY_EDITOR)
            IOSMessage.Create(title, message, ok);
#endif
#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog(title, message, ok);
#endif
#if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN)
        EditorUtility.DisplayDialogComplex(title, message, ok);
#endif
    }
    #endregion
}
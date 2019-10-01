#define DEBUG_MODE

using UnityEngine;
using System.Collections;

#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSNative
{
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
    [DllImport("__Internal")]
    private static extern void _TAG_ShowDialog(string title, string message, string yes, string no);

    [DllImport("__Internal")]
    private static extern void _TAG_ShowMessage(string title, string message, string ok);

    [DllImport("__Internal")]
    private static extern void _TAG_DismissCurrentAlert();

#endif

    public static void showDialog(string title, string message, string yes, string no)
    {
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
        _TAG_ShowDialog(title, message, yes, no);
#endif
    }

    public static void showMessage(string title, string message, string ok)
    {
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
        _TAG_ShowMessage(title, message, ok);
#endif
    }

    public static void DismissCurrentAlert()
    {
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
        _TAG_DismissCurrentAlert();
#endif
    }
}
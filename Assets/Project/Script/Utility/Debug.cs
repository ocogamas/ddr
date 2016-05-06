using UnityEngine;
using System.Collections;

public class Debug
{
    public static void PrintLog(string color, object message)
    {
        UnityEngine.Debug.Log ("<color=" + color + ">" + message + "</color>");
    }
    public static void Log_yellow(object message) { PrintLog ("#ffff00", message);}
    public static void Log_lime(object message)   { PrintLog ("#00ff00", message);}


}

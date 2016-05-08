using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResponseObjectManager 
{
    public static string GetStringFromDictionary(Dictionary<string, object> dic, string key)
    {
        Dictionary<string, object> elementDic = (Dictionary<string, object>)dic ["gsx$" + key];
        string result = (string)elementDic ["$t"];
        return result;
    }
}

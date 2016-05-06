using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResponseObjectUpdateCheck 
{
    public string encoding;
    public string version;

    public Dictionary<string, object> feed;

    public Dictionary<string, object>[] entry;

    public void SetupEntry()
    {
        foreach (string key in feed.Keys)
        {
            if (key == "entry")
            {
                this.entry = (Dictionary<string, object>[])feed [key];
            }
        }
    }

    public int GetDataCount()
    {
        return this.entry.Length;
    }

    public string GetTitle(int index)
    {
        Dictionary<string, object> dic = entry [index];
        Dictionary<string, object> titleDic = (Dictionary<string, object>)dic ["gsx$title"];
        string title = (string)titleDic ["$t"];
        return title;
    }
}

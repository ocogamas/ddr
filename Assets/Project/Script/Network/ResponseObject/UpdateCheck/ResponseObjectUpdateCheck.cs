using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResponseObjectUpdateCheck 
{
    public string encoding;
    public string version;
    public Dictionary<string, object> feed;


    private Dictionary<string, object>[] entry;

    // feedからentryを抜き出して保持する
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

    // entry内のデータをわかりやすい形式のデータに変換して返却する
    public List<UpdateCheckData> GetDataList()
    {
        List<UpdateCheckData> list = new List<UpdateCheckData> ();

        foreach (Dictionary<string, object> dic in this.entry)
        {
            Dictionary<string, object> idDic = (Dictionary<string, object>)dic ["gsx$id"];
            string id = (string)idDic ["$t"];

            Dictionary<string, object> titleDic = (Dictionary<string, object>)dic ["gsx$title"];
            string title = (string)titleDic ["$t"];

            Dictionary<string, object> urlDic = (Dictionary<string, object>)dic ["gsx$url"];
            string url = (string)urlDic ["$t"];

            Dictionary<string, object> versionDic = (Dictionary<string, object>)dic ["gsx$version"];
            string version = (string)versionDic ["$t"];

            UpdateCheckData data = new UpdateCheckData ();
            data.id      = id;
            data.title   = title;
            data.url     = url;
            data.version = version;
            list.Add (data);

            Debug.Log_lime ("更新確認データ抽出　id=" + id + ", title = " + title + ", version = " + version);
        }
        return list;
    }

}

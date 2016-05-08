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
            string id      = ResponseObjectManager.GetStringFromDictionary (dic, "id");
            string title   = ResponseObjectManager.GetStringFromDictionary (dic, "title");
            string version = ResponseObjectManager.GetStringFromDictionary (dic, "version");

            UpdateCheckData data = new UpdateCheckData ();
            data.id      = id;
            data.title   = title;
            data.version = version;
            list.Add (data);

            Debug.Log_lime ("更新確認データ抽出　id=" + id + ", title = " + title + ", version = " + version);
        }
        return list;
    }

}

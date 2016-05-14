using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResponseObjectMusicInfo 
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
    public MusicInfoDataList GetDataList()
    {
        MusicInfoDataList dataList = new MusicInfoDataList ();
        List<MusicInfoData> list = new List<MusicInfoData> ();

        foreach (Dictionary<string, object> dic in this.entry)
        {
            string musicTitle = ResponseObjectManager.GetStringFromDictionary (dic, "musictitle");
            string spell      = ResponseObjectManager.GetStringFromDictionary (dic, "spell"); 
            string pk         = ResponseObjectManager.GetStringFromDictionary (dic, "pk"); 

            string gsp        = ResponseObjectManager.GetStringFromDictionary (dic, "gsp"); 
            string bsp        = ResponseObjectManager.GetStringFromDictionary (dic, "bsp"); 
            string dsp        = ResponseObjectManager.GetStringFromDictionary (dic, "dsp"); 
            string esp        = ResponseObjectManager.GetStringFromDictionary (dic, "esp"); 
            string csp        = ResponseObjectManager.GetStringFromDictionary (dic, "csp"); 

            string bdp        = ResponseObjectManager.GetStringFromDictionary (dic, "bdp"); 
            string ddp        = ResponseObjectManager.GetStringFromDictionary (dic, "ddp"); 
            string edp        = ResponseObjectManager.GetStringFromDictionary (dic, "edp"); 
            string cdp        = ResponseObjectManager.GetStringFromDictionary (dic, "cdp"); 

            MusicInfoData data = new MusicInfoData ();
            data.musicTitle    = musicTitle;
            data.spell         = spell;
            data.pk            = pk;
            data.gsp           = gsp;
            data.bsp           = bsp;
            data.dsp           = dsp;
            data.esp           = esp;
            data.csp           = csp;
            data.bdp           = bdp;
            data.ddp           = ddp;
            data.edp           = edp;
            data.cdp           = cdp;
            list.Add (data);

            Debug.Log_lime ("楽曲データ抽出　musicTitle=" + musicTitle + ", spell = " + spell + ", pk = " + pk);
        }

        dataList.dataList = list;
        return dataList;
    }

  
}

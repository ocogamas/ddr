using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayTop : DisplayBase
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private SystemLogView systemLogView;
    [SerializeField] private DisplayMusicList displayMusicList;


    private List<UpdateCheckData> updateCheckDataList; // 更新情報
    private List<MusicInfoData> musicInfoDataList;     // 全楽曲情報

    public List<MusicInfoData> MusicInfoDataList { get { return this.musicInfoDataList; } }

    private void Start()
    {
        musicInfoDataList = new List<MusicInfoData> ();

        StartCoroutine (checkUpdate ());
    }
        
	public void OnClickMusicListButton()
	{
		UnityEngine.Debug.Log ("OnClickMusicListButton >");

        if (this.translateCoroutine == null)
        {
            this.translateCoroutine = AnimationUtil.TranslateWithScale (this.transform, this.displayMusicList.transform);
            StartCoroutine (this.translateCoroutine);
        }
	}

    // 更新確認の通信処理
    private IEnumerator checkUpdate()
    {
        string url    = this.networkManager.GetURLWithKey ("od6");
        string result = this.networkManager.Request (url, "更新情報取得");
        ResponseObjectUpdateCheck resultObject = JsonFx.Json.JsonReader.Deserialize<ResponseObjectUpdateCheck> (result);
        resultObject.SetupEntry ();

        this.updateCheckDataList = resultObject.GetDataList ();

        if (this.updateCheckDataList != null)
        {
            downloadMusicListIfNeeded ();
        }
        else
        {
            this.systemLogView.AddText ("更新情報が空っぽでした。再起動してみて。");
        }

        yield return null;
    }

    private void downloadMusicListIfNeeded()
    {
        this.systemLogView.AddText ("必要があれば楽曲リストを取得します");

        foreach (UpdateCheckData data in this.updateCheckDataList)
        {
            // TODO: data.titleのバージョン（data.version）が、
            // 保存してあるバージョンと一致していなければダウンロードする　

            string url    = this.networkManager.GetURLWithKey (data.id);
            string result = this.networkManager.Request (url, "楽曲リスト取得 [" + data.title + "]");
            ResponseObjectMusicInfo resultObject = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicInfo> (result);
            resultObject.SetupEntry ();

            List<MusicInfoData> musicDataList = resultObject.GetDataList ();

            foreach (MusicInfoData musicData in musicDataList)
            {
                this.musicInfoDataList.Add (musicData);
            }
            this.systemLogView.AddText ("[ " + data.title + " ] " + musicDataList.Count + "曲取得しました"); 
        }

    }
}

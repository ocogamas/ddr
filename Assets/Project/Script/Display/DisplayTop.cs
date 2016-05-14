using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DisplayTop : DisplayBase
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private SystemLogView systemLogView;
    [SerializeField] private DisplayMusicList displayMusicList;



    private UpdateCheckDataList updateCheckDataList; // 更新情報
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

    public void OnClickDeleteDataButton()
    {
        bool isSuccess = DataManager.Delete (DataManager.UPDATE_INFO);
        if (isSuccess)
        {
            this.systemLogView.AddText ("データ削除成功");
        }
        else
        {
            this.systemLogView.AddText ("データ削除失敗。ファイルが見つからない。");
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
            downloadMusicListIfNeeded (resultObject);
        }
        else
        {
            this.systemLogView.AddText ("更新情報が空っぽでした。再起動してみて。");
        }

        yield return null;
    }

    private void downloadMusicListIfNeeded(ResponseObjectUpdateCheck resultObject)
    {
        this.systemLogView.AddText ("必要があれば楽曲リストを取得します");

        UpdateCheckDataList loadedUpdateCheckDataList = DataManager.Load<UpdateCheckDataList> (DataManager.UPDATE_INFO);
        if (loadedUpdateCheckDataList == null)
        {
            this.systemLogView.AddText ("更新情報は保存されていません");
        }
        else
        {
            this.systemLogView.AddText ("更新情報を読み込みました");                       
        }


        foreach (UpdateCheckData data in this.updateCheckDataList.dataList)
        {
            bool isSkip = false;
            if (loadedUpdateCheckDataList != null)
            {
                foreach (UpdateCheckData loadedData in loadedUpdateCheckDataList.dataList)
                {
                    if (loadedData.title == data.title)
                    {
                        if (loadedData.version == data.version)
                        {
                            isSkip = true;
                        }
                    }
                }
            }

            // data.titleのバージョン（data.version）が、
            // 保存してあるバージョンと一致していなければダウンロードする　
            if (isSkip)
            {
                systemLogView.AddText ("更新不要。通信をskipしました [" + data.title + "]");
                continue;
            }

            string url    = this.networkManager.GetURLWithKey (data.id);
            string result = this.networkManager.Request (url, "楽曲リスト取得 [" + data.title + "]");
            ResponseObjectMusicInfo musicInfo = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicInfo> (result);
            musicInfo.SetupEntry ();

            List<MusicInfoData> musicDataList = musicInfo.GetDataList ();

            foreach (MusicInfoData musicData in musicDataList)
            {
                this.musicInfoDataList.Add (musicData);
            }
            this.systemLogView.AddText ("[ " + data.title + " ] " + musicDataList.Count + "曲取得しました"); 
        }

        // 更新情報を保存する
        DataManager.Save<UpdateCheckDataList> (DataManager.UPDATE_INFO, updateCheckDataList);
        this.systemLogView.AddText ("更新情報を保存しました");

    }
}

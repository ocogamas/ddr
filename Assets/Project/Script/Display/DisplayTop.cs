using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DisplayTop : DisplayBase
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private SystemLogView systemLogView;
    [SerializeField] private DisplayMusicList displayMusicList;

    [SerializeField] private Button musicListButton;



    private UpdateCheckDataList updateCheckDataList; // 更新情報
    private List<MusicInfoData> musicInfoDataList;     // 全楽曲情報

    public List<MusicInfoData> MusicInfoDataList { get { return this.musicInfoDataList; } }

    private void Start()
    {
        this.musicListButton.interactable = false;
        musicInfoDataList = new List<MusicInfoData> ();

        StartCoroutine (checkUpdate ());
    }
     
    #region ボタン
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
    #endregion

    // 更新確認の通信処理
    private IEnumerator checkUpdate()
    {
        string url    = this.networkManager.GetURLWithKey ("od6");
        string result = this.networkManager.Request (url, "更新情報取得");
        ResponseObjectUpdateCheck resultObject = JsonFx.Json.JsonReader.Deserialize<ResponseObjectUpdateCheck> (result);
        resultObject.SetupEntry ();

        this.updateCheckDataList = resultObject.GetDataList ();

        yield return null;

        if (this.updateCheckDataList != null)
        {
            StartCoroutine(downloadMusicListIfNeeded (resultObject));
        }
        else
        {
            this.systemLogView.AddText ("更新情報が空っぽでした。再起動してみて。");
        }

        yield return null;
    }

    private IEnumerator downloadMusicListIfNeeded(ResponseObjectUpdateCheck resultObject)
    {
        this.systemLogView.AddText ("必要があれば楽曲リストを取得します");
        yield return null;

        UpdateCheckDataList loadedUpdateCheckDataList = DataManager.Load<UpdateCheckDataList> (DataManager.UPDATE_INFO);
        if (loadedUpdateCheckDataList == null)
        {
            this.systemLogView.AddText ("更新情報は保存されていません");
        }
        else
        {
            this.systemLogView.AddText ("更新情報を読み込みました");                       
        }

        yield return null;

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
                MusicInfoDataList dataList = DataManager.Load<MusicInfoDataList> (data.title);
                if (dataList != null)
                {
                    foreach (MusicInfoData musicData in dataList.dataList)
                    {
                        this.musicInfoDataList.Add (musicData);
                    }
                    systemLogView.AddText ("更新不要。通信をskipしました [" + data.title + "]");
                    yield return null;
                    continue;
                }
                else
                {
                    systemLogView.AddText ("データが壊れていたので通信して取得します [" + data.title + "]");
                }

            }

            yield return null;

            string url    = this.networkManager.GetURLWithKey (data.id);
            string result = this.networkManager.Request (url, "楽曲リスト取得 [" + data.title + "]");
            ResponseObjectMusicInfo musicInfo = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicInfo> (result);
            musicInfo.SetupEntry ();

            MusicInfoDataList musicDataList = musicInfo.GetDataList ();

            foreach (MusicInfoData musicData in musicDataList.dataList)
            {
                this.musicInfoDataList.Add (musicData);
            }
            this.systemLogView.AddText ("[ " + data.title + " ] " + musicDataList.dataList.Count + "曲取得しました"); 

            // 楽曲リストを保存する
            DataManager.Save<MusicInfoDataList> (data.title, musicDataList);

            yield return null;
        }

        // 更新情報を保存する
        DataManager.Save<UpdateCheckDataList> (DataManager.UPDATE_INFO, updateCheckDataList);
        this.systemLogView.AddText ("更新情報を保存しました");

        this.musicListButton.interactable = true;

        yield return null;
    }
}

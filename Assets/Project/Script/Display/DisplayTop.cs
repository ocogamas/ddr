using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayTop : DisplayBase
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private SystemLogView systemLogView;
    [SerializeField] private DisplayMusicList displayMusicList;


    private List<UpdateCheckData> updateCheckDataList;

    private void Start()
    {
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

        yield return null;
    }
}

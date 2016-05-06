using UnityEngine;
using System.Collections;

public class DisplayTop : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private void Start()
    {
        string url = "https://spreadsheets.google.com/feeds/list/1Ch0f9mdYj6KjT4DVOmXBCgqr5W8tDfjgr9rEKTeUZwI/od6/public/values?alt=json";
        string result = this.networkManager.Request (url, "更新情報取得");

        ResponseObjectUpdateCheck resultObject = JsonFx.Json.JsonReader.Deserialize<ResponseObjectUpdateCheck> (result);

        resultObject.SetupEntry ();

        int count = resultObject.GetDataCount ();

    }

	public void OnClickMusicListButton()
	{
		UnityEngine.Debug.Log ("OnClickMusicListButton >");
	}
}

using UnityEngine;
using System.Net;
using System.Text;
using System.IO;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviour 
{
	[SerializeField] private SystemLogView systemLogView;

    public const string SPREAD_SHEET_URL_BEFORE = "http://spreadsheets.google.com/feeds/list/1Ch0f9mdYj6KjT4DVOmXBCgqr5W8tDfjgr9rEKTeUZwI/";
    public const string SPREAD_SHEET_URL_AFTER  = "/public/values?alt=json";

	private void Awake()
	{
	}

	private void Start()
	{
	}

    public string GetURLWithKey(string key)
    {
        return SPREAD_SHEET_URL_BEFORE + key + SPREAD_SHEET_URL_AFTER;
    }

    public string Request(string url, string title)
    {
        StartCoroutine(addSystemLog("通信開始：[" + title + "]"));

        WebRequest request = HttpWebRequest.Create (url);
        request.Method = "GET";

        WebResponse response = null;
        try
        {
            response = request.GetResponse();
        }
        catch(Exception e)
        {
            response = null;
            StartCoroutine(addSystemLog("通信エラー：[" + title + "] 正常に通信できませんでした"));
        }

        string text = "";

        if (response != null)
        {
            StartCoroutine(addSystemLog("通信成功：[" + title + "]"));

            Stream stream = response.GetResponseStream ();
            StreamReader streamReader = new StreamReader (stream, Encoding.GetEncoding ("UTF-8"));
            text = streamReader.ReadToEnd ();

            streamReader.Close ();
            stream.Close ();

            Debug.Log_lime ("通信結果 = " + text);
        }
        return text;
    }

    private IEnumerator addSystemLog(string text)
    {
        this.systemLogView.AddText ("<color=#33bbff>" + text + "</color>");
        yield return null;
    }
}

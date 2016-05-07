using UnityEngine;
using System.Net;
using System.Text;
using System.IO;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
	[SerializeField] private SystemLogView systemLogView;


	private void Awake()
	{
	}

	private void Start()
	{
		this.systemLogView.AddText ("通信マネージャ起動");                      
	}

    public string Request(string url, string title)
    {
        addSystemLog("通信開始：[" + title + "]");
        WebRequest request = HttpWebRequest.Create (url);
        request.Method = "GET";

        WebResponse response = null;
        try
        {
            response = request.GetResponse();
        }
        catch
        {
            response = null;
            addSystemLog("通信エラー");
        }

        string text = "";

        if (response != null)
        {
            addSystemLog("通信成功：[" + title + "]");

            Stream stream = response.GetResponseStream ();
            StreamReader streamReader = new StreamReader (stream, Encoding.GetEncoding ("UTF-8"));
            text = streamReader.ReadToEnd ();

            streamReader.Close ();
            stream.Close ();

            Debug.Log_lime ("通信結果 = " + text);
        }
        return text;
    }

    private void addSystemLog(string text)
    {
        this.systemLogView.AddText ("<color=#33bbff>" + text + "</color>");
    }
}

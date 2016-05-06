using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class SystemLogView : MonoBehaviour
{
	const float LINE_SIZE = 14.0f;
	
	[SerializeField] private Text textField;

	private RectTransform textFieldRect;

	private StringBuilder sb = new StringBuilder ();


	private void Awake()
	{
		this.textFieldRect = this.textField.GetComponent<RectTransform> ();
		
		AddText ("アプリを起動しました");
	}

	private void Start()
	{
	}


	public void AddText(string text)
	{
		this.sb.Insert (0, text + "\n");
		this.textField.text = this.sb.ToString ();

		adjustTextFieldSizeWithAddLine ();
		forceAnchorTop ();
	}

	// 一行を追加する事によるテキストフィールドのサイズ調整
	private void adjustTextFieldSizeWithAddLine()
	{
		Vector3 pos = this.textField.transform.localPosition;
		pos.y += LINE_SIZE;
		this.textField.transform.localPosition = pos;

		Vector2 delta = this.textFieldRect.sizeDelta;
		delta.y += LINE_SIZE;
		this.textFieldRect.sizeDelta = delta;
	}

	private void forceAnchorTop()
	{
		Vector2 anchoredPos = this.textFieldRect.anchoredPosition;
		anchoredPos.y = 0;
		this.textFieldRect.anchoredPosition = anchoredPos;
	}

}

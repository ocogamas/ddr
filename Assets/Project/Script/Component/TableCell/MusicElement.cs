using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
public class MusicElement : MonoBehaviour 
{
    [SerializeField] Text musicTextField;
    [SerializeField] Text likeTextField;

    public MusicInfoData musicInfoData;
    public MusicLikeData musicLikeData;

    public System.Action callback = null;

    public void OnClickUpLikeButton()
    {
        musicLikeData.like++;
        if (musicLikeData.like >= 999)
        {
            musicLikeData.like = 999;
        }
        SetLikePoint (musicLikeData.like);

        if (callback != null)
        {
            callback ();
        }
    }
    public void OnClickDownLikeButton()
    {
        musicLikeData.like--;
        if (musicLikeData.like <= -999)
        {
            musicLikeData.like = -999;
        }
        SetLikePoint (musicLikeData.like);

        if (callback != null)
        {
            callback ();
        }
    }

    public void SetMusicTitle(string text)
    {
        this.musicTextField.text = text;
    }

    public void SetLikePoint(int point)
    {
        this.likeTextField.text = point.ToString ("D");
    }
}

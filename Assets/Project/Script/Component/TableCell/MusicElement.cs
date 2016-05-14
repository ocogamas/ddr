using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
public class MusicElement : MonoBehaviour 
{
    [SerializeField] Text musicTextField;
    [SerializeField] Text likeTextField;

    public void OnClickUpLikeButton()
    {
        Debug.Log_blue ("UpLike!");
    }
    public void OnClickDownLikeButton()
    {
        Debug.Log_blue ("DownLike!");
    }

    public void SetMusicTitle(string text)
    {
        this.musicTextField.text = text;
    }
}

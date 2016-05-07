using UnityEngine;
using System.Collections;

public class DisplayMusicList : DisplayBase 
{
    [SerializeField] DisplayTop displayTop;


    public void OnClickBackButton()
    {
        Debug.Log_blue ("OnClickBackButton");
        if (this.translateCoroutine == null)
        {
            this.translateCoroutine = AnimationUtil.TranslateWithScale (this.transform, this.displayTop.transform);
            StartCoroutine (this.translateCoroutine);
        }
    }
}

using UnityEngine;
using System.Collections;

public class DisplayMusicList : DisplayBase 
{
    [SerializeField] DisplayTop displayTop;

    [SerializeField] GameObject content;
    [SerializeField] GameObject cellPrefab;

    private void Awake()
    {
        Debug.Log_blue ("DisplayMusicList : Awake >");
    }
    private void Start()
    {
        Debug.Log_blue ("DisplayMusicList : Start >");

        setupMusicData ();
    }
    private void OnEnable()
    {
        Debug.Log_blue ("DisplayMusicList : OnEnable >");
    }
        
    public void OnClickBackButton()
    {
        Debug.Log_blue ("OnClickBackButton");
        if (this.translateCoroutine == null)
        {
            this.translateCoroutine = AnimationUtil.TranslateWithScale (this.transform, this.displayTop.transform);
            StartCoroutine (this.translateCoroutine);
        }
    }


    private void setupMusicData()
    {
        StartCoroutine (setupMusicDataCoroutine ());
    }

    private IEnumerator setupMusicDataCoroutine()
    {
        foreach (MusicInfoData data in this.displayTop.MusicInfoDataList)
        {
            string pk    = data.pk;
            string title = data.musicTitle;

            GameObject cellObject = Instantiate (cellPrefab);
            cellObject.transform.SetParent (content.transform);
            cellObject.name = "MusicCell_" + pk;
            cellObject.transform.localScale = new Vector3 (1.0f, 1.0f);

            MusicElement musicElement = cellObject.GetComponent<MusicElement> ();
            musicElement.SetMusicTitle (title);

            yield return null;
        }

    }
}

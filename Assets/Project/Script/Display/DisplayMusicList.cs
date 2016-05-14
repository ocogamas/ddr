using UnityEngine;
using System.Collections;

public class DisplayMusicList : DisplayBase 
{
    [SerializeField] private DisplayTop displayTop;

    [SerializeField] private GameObject content;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private SystemLogView systemLogView;

    private MusicLikeDataList musicLikeDataList;

    private void Awake()
    {
    }
    private void Start()
    {
        setupMusicData ();
    }
    private void OnEnable()
    {
        Debug.Log_blue ("DisplayMusicList : OnEnable >");
    }

    private void callbackLikeChanged()
    {
        Debug.Log_lime ("ほぞん！");
        DataManager.Save (DataManager.MUSIC_LIKE_DATA_LIST, this.musicLikeDataList);
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
        this.musicLikeDataList = DataManager.Load<MusicLikeDataList> (DataManager.MUSIC_LIKE_DATA_LIST);
        if (this.musicLikeDataList == null)
        {
            this.systemLogView.AddText ("投票データが保存されていません");
            this.systemLogView.AddText ("投票データを新規作成しました");
            this.musicLikeDataList = new MusicLikeDataList ();
        }
        else
        {
            this.systemLogView.AddText ("投票データをロードしました");
        }
        
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
            musicElement.musicInfoData = data;

            bool isFound = false;
            foreach (MusicLikeData likeData in this.musicLikeDataList.dataList)
            {
                if (data.pk == likeData.pk)
                {
                    isFound = true;
                    musicElement.SetLikePoint (likeData.like);
                    musicElement.musicLikeData = likeData;
                }
            }
            if (isFound == false)
            {
                MusicLikeData newLikeData = new MusicLikeData ();
                newLikeData.pk   = data.pk;
                newLikeData.like = 0;
                this.musicLikeDataList.dataList.Add (newLikeData);
                musicElement.SetLikePoint (0);
                musicElement.musicLikeData = newLikeData;
            }

            musicElement.callback = callbackLikeChanged;

            yield return null;
        }

        // 楽曲投票データを保存
        DataManager.Save<MusicLikeDataList> (DataManager.MUSIC_LIKE_DATA_LIST, this.musicLikeDataList);
    }
}

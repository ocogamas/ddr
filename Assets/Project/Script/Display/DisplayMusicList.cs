using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DisplayMusicList : DisplayBase 
{
    [SerializeField] private DisplayTop displayTop;

    [SerializeField] private GameObject content;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private SystemLogView systemLogView;

    [SerializeField] private Text filterText;

    [SerializeField] private Button higherButton;

    [SerializeField] private Button belowButton;

    [SerializeField] private MusicScrollRect scrollRect;

    private List<MusicElement> musicElementList = new List<MusicElement> ();

    private IEnumerator coroutine=null;

    private MusicLikeDataList musicLikeDataList;

    private float scrollRateY;

    #region MonoBehaviour Methods
    private void Awake()
    {
        this.scrollRect.onValueChanged.AddListener (onScrollValueChanged);
    }
    private void Start()
    {
        this.higherButton.interactable = false;
        this.belowButton.interactable = true;

        setupMusicData ();

    }
    private void OnEnable()
    {
        Debug.Log_blue ("DisplayMusicList : OnEnable >");
    }
    #endregion



    #region Callback Methods

    private void callbackLikeChanged()
    {
        Debug.Log_lime ("ほぞん！");
        DataManager.Save<MusicLikeDataList> (DataManager.MUSIC_LIKE_DATA_LIST, this.musicLikeDataList);
    }



    private void onScrollValueChanged(Vector2 vec)
    {
        this.scrollRateY = 1.0f - vec.y;
    }

    #endregion
      

    #region Button Methods

    public void OnClickBackButton()
    {
        Debug.Log_blue ("OnClickBackButton");
        if (this.translateCoroutine == null)
        {
            this.translateCoroutine = AnimationUtil.TranslateWithScale (this.transform, this.displayTop.transform);
            StartCoroutine (this.translateCoroutine);
        }
    }

    public void OnClickUpFilterButton()
    {
        int value = int.Parse (this.filterText.text);
        value++;
        if (value >= 999)
        {
            value = 999;
        }
        this.filterText.text = value.ToString ();

        addSystemLogWithFilterButton (value.ToString ());

        if (coroutine != null)
        {
            StopCoroutine (coroutine);
        }

        coroutine = setupMusicDataCoroutine ();
        StartCoroutine(coroutine);
    }

    public void OnClickDownFilterButton()
    {
        int value = int.Parse (this.filterText.text);
        value--;
        if (value <= -999)
        {
            value = -999;
        }
        this.filterText.text = value.ToString ();

        addSystemLogWithFilterButton (value.ToString ());

        if (coroutine != null)
        {
            StopCoroutine (coroutine);
        }

        coroutine = setupMusicDataCoroutine ();
        StartCoroutine(coroutine);
    }

    public void OnClickHigherButton()
    {
        this.higherButton.interactable = false;
        this.belowButton.interactable = true;

        addSystemLogWithFilterButton (this.filterText.text);

        if (coroutine != null)
        {
            StopCoroutine (coroutine);
        }

        coroutine = setupMusicDataCoroutine ();
        StartCoroutine(coroutine);
    }

    public void OnClickBelowButton()
    {
        this.higherButton.interactable = true;
        this.belowButton.interactable = false;

        addSystemLogWithFilterButton (this.filterText.text);

        if (coroutine != null)
        {
            StopCoroutine (coroutine);
        }

        coroutine = setupMusicDataCoroutine ();
        StartCoroutine(coroutine);
    }

    #endregion


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

        if (coroutine != null)
        {
            StopCoroutine (coroutine);
        }

        coroutine = setupMusicDataCoroutine ();
        StartCoroutine(coroutine);
    }

    private IEnumerator setupMusicDataCoroutine()
    {
        
        yield return null;

        int likePointMusicCount = 0;

        // 保存データから、お気に入りポイントを割り当てる
        foreach (MusicInfoData data in this.displayTop.MusicInfoDataList)
        {
            string pk = data.pk;
            foreach (MusicLikeData likeData in this.musicLikeDataList.dataList)
            {
                if (pk == likeData.pk)
                {
                    data.likePoint = likeData.like;
                    break;
                }
            }

            if (data.likePoint == int.Parse (this.filterText.text))
            {
                likePointMusicCount++;
            }
        }
        sortMusicData ();

        this.systemLogView.AddText ("<color=cyan>ポイント " + this.filterText.text + " の曲は、" + likePointMusicCount + " 件です。</color>");

        yield return null;

        removeAllCellObject ();

        // １フレで複数個のセルを生成するためのカウンタ
        int createCellCounter = 0;

        // 読み込み進行度を表示するためのカウンタ
        int loadingDisplayCounter = 0;

        foreach (MusicInfoData data in this.displayTop.MusicInfoDataList)
        {
            string pk    = data.pk;
            string title = data.musicTitle;

            // 投票データの確認
            bool isFilterHide = false;
            int filterValue = int.Parse (this.filterText.text);


            // 値以上を表示する場合
            if (this.higherButton.interactable == false)
            {
                if (data.likePoint < filterValue)
                {
                    isFilterHide = true; // フィルタに引っかかったので表示しません
                }
            }
            // 値以下を表示する場合
            else if (this.belowButton.interactable == false)
            {
                if (data.likePoint > filterValue)
                {
                    isFilterHide = true; // フィルタに引っかかったので表示しません
                }
            }

            if (isFilterHide)
            {
                continue;
            }


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

            this.musicElementList.Add (musicElement);

            createCellCounter++;
            if (createCellCounter % 4 == 0)
            {
                yield return null;
            }

            loadingDisplayCounter++;
            if (loadingDisplayCounter % 40 == 0)
            {                
                float percent = loadingDisplayCounter / (float)this.displayTop.MusicInfoDataList.Count;
                this.systemLogView.AddText ("リスト構築中... " + ((int)(100 * percent)).ToString("D") + "％   " + loadingDisplayCounter);
                yield return null;
            }


            while ( ((loadingDisplayCounter - 20) / (float)this.displayTop.MusicInfoDataList.Count) > this.scrollRateY)
            {
                //Debug.Log_blue ("counter = " + loadingDisplayCounter + ", " + (loadingDisplayCounter / (float)this.displayTop.MusicInfoDataList.Count) + " / " + this.scrollRateY);
                yield return null;
            }
        }  
        this.systemLogView.AddText ("リスト構築中... 100％   " + loadingDisplayCounter);
        this.coroutine = null;
        yield return null;
    }

    private void sortMusicData()
    {
        // スペルで並び替え
        this.displayTop.MusicInfoDataList.Sort (
            delegate(MusicInfoData a, MusicInfoData b)
            {
                if (a.likePoint == b.likePoint)
                {
                    if (a.spell == null || a.spell.Length == 0)
                    {
                        if (b.spell == null || b.spell.Length == 0) { return 0; }
                        else { return 1; }
                    }
                    else
                    {
                        if (b.spell == null || b.spell.Length == 0) { return -1; }
                        else { return string.Compare(a.spell, b.spell); }
                    }
                }
                else
                {
                    if (a.likePoint > b.likePoint)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        );            
    }



    private void removeAllCellObject()
    {
        int count = this.musicElementList.Count;
      
        for (int i=count-1; i>=0; i--)
        {
            DestroyImmediate (this.musicElementList[i].gameObject);
        }

        this.musicElementList.Clear ();
    }


    private void addSystemLogWithFilterButton (string numStr)
    {
        if (this.higherButton.interactable == true)
        {
            this.systemLogView.AddText ("<color=#ff9933>ポイント " + numStr + " 以下を表示します</color>");
        }
        else
        {
            this.systemLogView.AddText ("<color=#ff9933>ポイント " + numStr + " 以上を表示します</color>");
        }
    }
}

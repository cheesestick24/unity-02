using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class RankingDisplayManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform rankingListParent;
    public GameObject rankingItemPrefab;

    [Header("Ranking Data")]
    // 現在のランキングデータを保持するリスト
    private List<RankingData> currentRankings = new List<RankingData>();

    // ファイル保存関連
    private const string RANKING_FILE_NAME = "ranking.json"; // ファイル名 (パスはJsonDataSaverで結合)
    private const int MAX_RANKING_COUNT = 10;

    void Awake()
    {
        LoadRankings(); // ゲーム起動時にランキングデータをロード
    }

    void Start()
    {
        DisplayRankings(currentRankings.ToArray()); // ロードしたデータ、または初期データでUI表示
    }

    [ContextMenu("Reset Rankings Data")]
    private void ResetRankingsData()
    {
        currentRankings.Clear();
        SaveRankings();
        DisplayRankings(currentRankings.ToArray());
        Debug.Log("ランキングデータをリセットしました。");
    }

    /// <summary>
    /// ランキングデータをUIに表示します。
    /// </summary>
    /// <param name="rankingsToDisplay">表示するランキングデータの配列</param>
    public void DisplayRankings(RankingData[] rankingsToDisplay)
    {
        // 既存のランキングアイテムを削除
        foreach (Transform child in rankingListParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rankingsToDisplay.Length; i++)
        {
            GameObject itemObject = Instantiate(rankingItemPrefab, rankingListParent);
            itemObject.SetActive(true);

            // ランキングアイテムのTextコンポーネントを取得
            TMP_Text rankText = itemObject.transform.Find("RankText").GetComponent<TMP_Text>();
            TMP_Text timeText = itemObject.transform.Find("TimeText").GetComponent<TMP_Text>();
            TMP_Text scoreText = itemObject.transform.Find("ScoreText").GetComponent<TMP_Text>();

            if (rankText == null || timeText == null || scoreText == null)
            {
                Debug.LogError("RankingItemTemplateのTextコンポーネントが見つかりません。名前を確認してください。");
                Destroy(itemObject);
                continue;
            }

            // ランキングデータをUIに設定
            rankText.text = rankingsToDisplay[i].rank.ToString() + "位";
            timeText.text = FormatGameTime(rankingsToDisplay[i].gameTime);
            scoreText.text = rankingsToDisplay[i].score.ToString();
        }
    }

    /// <summary>
    /// 新しいスコアが追加された場合のランキング更新
    /// </summary>
    public void AddScoreToRanking(float newGameTime, int newScore)
    {
        RankingData newEntry = new RankingData { gameTime = newGameTime, score = newScore, rank = 0 };
        currentRankings.Add(newEntry);

        SortAndTruncateRankings();
        DisplayRankings(currentRankings.ToArray());
        SaveRankings(); // ランキングが更新されたらファイルに保存
    }

    /// <summary>
    /// ランキングデータをファイルからロードします。
    /// </summary>
    private void LoadRankings()
    {
        RankingDataList loadedList = JsonDataSaver.LoadData<RankingDataList>(RANKING_FILE_NAME);

        if (loadedList != null && loadedList.rankings != null)
        {
            currentRankings = loadedList.rankings;
        }
        else
        {
            // ファイルがない場合やロード失敗の場合、新しいリストを初期化
            currentRankings = new List<RankingData>();
        }

        SortAndTruncateRankings(); // ロード後、ソートと絞り込み
    }

    /// <summary>
    /// 現在のランキングデータをファイルに保存します。
    /// </summary>
    private void SaveRankings()
    {
        RankingDataList saveList = new() { rankings = currentRankings };
        JsonDataSaver.SaveData(saveList, RANKING_FILE_NAME);
    }

    /// <summary>
    /// ランキングをソートし、上位MAX_RANKING_COUNT件に絞り込み、順位を更新します。
    /// </summary>
    private void SortAndTruncateRankings()
    {
        // スコア降順、スコアが同じなら時間昇順でソート (短い時間の方が上位)
        currentRankings = currentRankings
            .OrderByDescending(r => r.score)
            .ThenBy(r => r.gameTime)
            .Take(MAX_RANKING_COUNT)
            .ToList();

        // 順位を再割り当て
        for (int i = 0; i < currentRankings.Count; i++)
        {
            currentRankings[i].rank = i + 1;
        }
    }


    /// <summary>
    /// float型のゲーム時間を「分:秒.ミリ秒」形式でフォーマットします。
    /// </summary>
    private string FormatGameTime(float timeInSeconds)
    {
        if (timeInSeconds < 0) timeInSeconds = 0;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        return timeSpan.ToString(@"mm\:ss\.fff");
    }
}

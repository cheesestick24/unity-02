using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI rankingText;
    public int maxRankings = 5;

    // スコアと、それが今回追加されたかどうかを保持する内部クラス
    [System.Serializable] // PlayerPrefsでは使わないが、デバッグでInspector表示したい場合に便利
    public class RankingEntry
    {
        public int score;
        public bool isNewEntry; // 今回追加されたエントリかどうか

        public RankingEntry(int score, bool isNewEntry = false)
        {
            this.score = score;
            this.isNewEntry = isNewEntry;
        }
    }

    private List<RankingEntry> currentHighScores = new List<RankingEntry>();
    private int lastAddedScoreIndex = -1; // 最後に追加されたスコアのインデックス

    void Awake()
    {
        LoadHighScores();
        DisplayRanking();
    }

    public void AddNewScore(int newScore)
    {
        lastAddedScoreIndex = -1; // 新しいスコア追加前にリセット

        // 現在のリストに新しいスコアを追加（最初はisNewEntryをtrueにする）
        currentHighScores.Add(new RankingEntry(newScore, true));

        // スコアを降順にソート
        currentHighScores = currentHighScores.OrderByDescending(entry => entry.score).ToList();

        // maxRankingsを超えたら古い（低い）スコアを削除
        if (currentHighScores.Count > maxRankings)
        {
            currentHighScores.RemoveAt(currentHighScores.Count - 1);
        }

        // 今回追加されたスコアがランキングのどこに入ったか探す
        // 同点の場合、ソート順によっては正確な位置が難しいので、まずは最初のマッチを探す
        for (int i = 0; i < currentHighScores.Count; i++)
        {
            if (currentHighScores[i].score == newScore && currentHighScores[i].isNewEntry)
            {
                lastAddedScoreIndex = i; // インデックスを記憶
                break; // 最初に見つかったものでOK
            }
        }

        // isNewEntry フラグは一時的なものなので、保存前にリセットする
        // （次回の起動時にisNewEntryがtrueにならないようにするため）
        foreach (var entry in currentHighScores)
        {
            entry.isNewEntry = false;
        }

        SaveHighScores();
        DisplayRanking();
    }

    void LoadHighScores()
    {
        currentHighScores.Clear();

        for (int i = 0; i < maxRankings; i++)
        {
            int score = PlayerPrefs.GetInt("HighScore" + i, 0);
            if (PlayerPrefs.HasKey("HighScore" + i) && score >= 0) // 0点以上で存在すれば追加
            {
                currentHighScores.Add(new RankingEntry(score, false)); // 読み込み時はisNewEntryはfalse
            }
        }
        // 読み込んだ後も念のためソート
        currentHighScores = currentHighScores.OrderByDescending(entry => entry.score).ToList();
    }

    void SaveHighScores()
    {
        for (int i = 0; i < maxRankings; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i);
        }

        for (int i = 0; i < currentHighScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, currentHighScores[i].score); // scoreだけを保存
        }
        PlayerPrefs.Save();
    }

    void DisplayRanking()
    {
        if (rankingText == null)
        {
            Debug.LogWarning("RankingManager: rankingTextが紐付けられていません。");
            return;
        }

        System.Text.StringBuilder rankingStringBuilder = new System.Text.StringBuilder();

        rankingStringBuilder.AppendLine("--- ランキング ---");
        // テーブルヘッダー (オプション)
        // rankingStringBuilder.AppendLine("順位\tスコア"); // \tでタブ区切り
        // rankingStringBuilder.AppendLine("------------------");

        if (currentHighScores.Count == 0)
        {
            rankingStringBuilder.AppendLine("まだスコアがありません。");
        }
        else
        {
            for (int i = 0; i < maxRankings; i++) // maxRankingsの数だけ表示
            {
                string rankString = (i + 1).ToString(); // 順位
                string scoreString = "";

                if (i < currentHighScores.Count)
                {
                    scoreString = currentHighScores[i].score.ToString();

                    // 新しく追加されたスコアに外枠（<mark>タグ）を付ける
                    if (i == lastAddedScoreIndex)
                    {
                        // <mark>タグで囲むと、通常はハイライトされますが、TextMeshProで外枠を表現するには
                        // TextMeshProのMaterialを変更するか、Outlineタグを使用します。
                        // 今回は<mark>タグを色付きの背景として利用し、別途Outline設定を推奨します。
                        // TextMeshProで直接外枠を付けるには、Font Asset の Material 設定で Outline を有効にするのが一般的です。
                        // ここでは背景色で強調する<mark>タグの例を示します。
                        // <mark>タグはデフォルトで背景色を変えるので、外枠ではないかもしれませんが、視覚的な強調には使えます。
                        // より厳密に「外枠」を表現するには、TextMeshProのOutlineタグを使います。
                        // <outline>タグはデフォルトでは存在しないので、MaterialのInspectorでOutlineを有効にする必要があります。
                        // <outline color=#FF0000FF thickness=0.2>...</outline> のように使う
                        rankingStringBuilder.Append($"<mark=#00c3ff50>{rankString}位: {scoreString}</mark>\n"); // 黄色っぽい半透明の背景
                    }
                    else
                    {
                        rankingStringBuilder.Append($"{rankString}位: {scoreString}\n");
                    }
                }
                else
                {
                    // ランキングの枠が埋まっていない場合
                    rankingStringBuilder.Append($"{rankString}位: ---\n");
                }
            }
        }
        rankingText.text = rankingStringBuilder.ToString();
    }

    // デバッグ用: ランキングをリセットしたい場合
    public void ResetRanking()
    {
        for (int i = 0; i < maxRankings; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i);
        }
        PlayerPrefs.Save();
        Debug.Log("ランキングをリセットしました！");
        LoadHighScores();
        DisplayRanking();
    }
}

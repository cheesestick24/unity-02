using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン管理のために必要

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI scoreText;
    private GameManager gameManager;
    private RankingManager rankingManager;

    void Start()
    {
        // ゲーム開始時にUIを非表示にする
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // GameManagerインスタンスを取得
        gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません。シーンにGameManagerオブジェクトがあるか確認してください。");
        }

        // RankingManagerインスタンスを取得
        rankingManager = FindAnyObjectByType<RankingManager>();
        if (rankingManager == null)
        {
            Debug.LogError("RankingManagerが見つかりません。シーンにRankingManagerオブジェクトがあるか確認してください。");
        }
    }

    // ゲームオーバーUIを表示する関数
    public void ShowGameOverUI(int brokenBlocks)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (scoreText != null)
        {
            scoreText.text = "壊したブロック数: " + brokenBlocks;
        }
        Time.timeScale = 0f; // ゲーム内の時間を停止

        if (rankingManager != null)
        {
            rankingManager.AddNewScore(brokenBlocks);
        }
    }

    // コンティニューボタンが押されたときに呼ばれる関数
    public void OnContinueButtonClicked()
    {
        // UIManager自身のUIを非表示にする
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // GameManagerにゲームを再開するよう要求する
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
        else
        {
            // GameManagerが見つからない場合は、直接シーンを再ロード
            Time.timeScale = 1f; // 時間を戻す
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    private GameManager gameManager;
    private RankingDisplayManager rankingDisplayManager;

    public TextMeshProUGUI scoreText;

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
        rankingDisplayManager = FindAnyObjectByType<RankingDisplayManager>();
        if (rankingDisplayManager == null)
        {
            Debug.LogError("RankingDisplayManagerが見つかりません。シーンにRankingDisplayManagerオブジェクトがあるか確認してください。");
        }
    }

    void Update()
    {
        // ゲームオーバーUIが表示されている場合のみ、スペースキーの入力をチェック
        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            // スペースキーを押すとコンティニューボタンがクリックされたとみなす
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnContinueButtonClicked();
            }
        }
    }

    // ゲームオーバーUIを表示する関数
    public void ShowGameOverUI()
    {
        // ランキングにスコアを追加
        rankingDisplayManager.AddScoreToRanking(gameManager.GameDuration, GameManager.brokenBlockCount);
        // スコアを表示
        scoreText.text = "Score " + GameManager.brokenBlockCount.ToString();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    /// <summary>
    /// コンティニューボタンが押されたときに呼ばれる関数
    /// </summary>
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

    /// <summary>
    /// 終了ボタンが押されたときに呼ばれる関数
    /// </summary>
    public void OnExitButtonClicked()
    {
        // UIManager自身のUIを非表示にする (必要であれば)
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // ゲームを終了する
#if UNITY_EDITOR
        // Unityエディターで実行中の場合
        EditorApplication.isPlaying = false;
        Debug.Log("エディターでの再生を停止しました。");
#else
        // ビルドされたアプリケーションで実行中の場合
        Application.Quit();
#endif
    }
}

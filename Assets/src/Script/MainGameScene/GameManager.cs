using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject startCountPanel;

    private GameOverUIManager gameOverUIManager;
    private BallController ballController;
    private BlockGenerator blockGenerator;
    private RankingDisplayManager rankingDisplayManager;

    // ゲーム時間
    private float timer = 0f;
    // ブロック生成間隔（秒）
    private float blockGenerationInterval = 10f;

    /// <summary>
    /// 壊したブロックの数
    /// </summary>
    public static int brokenBlockCount = 0;

    private float gameStartTime = 0f;
    private float gameEndTime = 0f;
    public float GameDuration => gameEndTime - gameStartTime;

    /// <summary>
    /// ゲーム開始前のカウントダウンテキスト
    /// </summary>
    public TextMeshProUGUI countdownText;
    /// <summary>
    /// ゲーム開始前のカウントダウン時間（秒）
    /// </summary>
    private float countdownDuration = 3f;
    /// <summary>
    /// ゲーム進行中かどうかのフラグ
    /// </summary>
    private bool isGameActive = false;

    /// <summary>
    /// ゲーム進行フラグを取得します。
    /// </summary>
    /// <returns>isGameActive</returns>
    public bool IsGameActive()
    {
        return isGameActive;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        brokenBlockCount = 0;
        Time.timeScale = 1f;
        timer = 0f;

        StartCoroutine(StartGameCountdown());

        gameOverUIManager = FindAnyObjectByType<GameOverUIManager>();
        if (gameOverUIManager == null)
        {
            Debug.LogError("GameOverUIManagerが見つかりません。シーンにGameOverUIManagerオブジェクトがあるか確認してください。");
        }

        ballController = FindAnyObjectByType<BallController>();
        if (ballController == null)
        {
            Debug.LogError("BallControllerが見つかりません。シーンにBallControllerオブジェクトがあるか確認してください。");
        }
        blockGenerator = FindAnyObjectByType<BlockGenerator>();
        if (blockGenerator == null)
        {
            Debug.LogError("BlockGeneratorが見つかりません。シーンにBlockGeneratorオブジェクトがあるか確認してください。");
        }
        rankingDisplayManager = FindAnyObjectByType<RankingDisplayManager>();
        if (rankingDisplayManager == null)
        {
            Debug.LogError("RankingDisplayManagerが見つかりません。シーンにRankingDisplayManagerオブジェクトがあるか確認してください。");
        }
    }

    /// <summary>
    /// ゲーム開始前のカウントダウンを行います。
    /// </summary>
    IEnumerator StartGameCountdown()
    {
        isGameActive = false; // ゲーム進行フラグをオフにする
        startCountPanel.SetActive(true);

        // カウントダウンテキストを有効にする
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        // カウントダウン
        for (int i = (int)countdownDuration; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSeconds(1f);
        }

        // カウントダウンテキストを無効にする
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // カウントダウンパネルを非表示にする
        startCountPanel.SetActive(false);
        // ゲーム開始時間を記録
        gameStartTime = Time.time;

        isGameActive = true;
        Debug.Log("ゲームスタート！");

        ballController.LaunchBall(); // BallControllerにボールを発射させる
        blockGenerator.GenerateNewBlockRow(); // BlockGeneratorに最初のブロック行を生成させる
    }

    void Update()
    {
        if (!isGameActive) return;

        timer += Time.deltaTime;

        if (timer >= blockGenerationInterval)
        {
            if (blockGenerator != null)
            {
                blockGenerator.GenerateNewBlockRow(true); // BlockGeneratorに新しいブロック行を生成させる
            }
            timer = 0f;
        }

        if (blockGenerator != null)
        {
            blockGenerator.MoveExistingBlocksForward(); // 既存のブロックを前方に移動させる
        }
    }

    /// <summary>
    /// ゲームを終了します。
    /// </summary>
    public void GameOver()
    {
        if (!isGameActive) return;

        isGameActive = false; // ゲーム進行フラグをオフにする
        gameEndTime = Time.time; // ゲーム終了時間を記録

        Debug.Log("ゲーム終了！");

        // ランキングにスコアを追加
        rankingDisplayManager.AddScoreToRanking(GameDuration, brokenBlockCount);

        // gameOverUIManagerにゲームオーバーUIの表示を依頼
        if (gameOverUIManager != null)
        {
            gameOverUIManager.ShowGameOverUI(brokenBlockCount);
        }
        else
        {
            Time.timeScale = 0f; // ゲームを停止
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // シーンを再読み込み
        }
    }

    /// <summary>
    /// ゲームを再スタートします。
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("ゲームを再スタートします。");
        Time.timeScale = 1f;
        brokenBlockCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

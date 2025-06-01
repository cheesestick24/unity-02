using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    // コルーチンを使用するために必要
    public static GameManager Instance { get; private set; }
    // ブロックのプレハブ
    public GameObject blockPrefab;
    // GameOverUIManagerの参照
    private GameOverUIManager uiManager;
    private BallController ballController;
    // ゲーム時間
    private float timer = 0f;
    // ブロックの横の数
    private int blocksPerRow = 15;
    // ブロックの間隔
    private float blockSpacingX = 1.1f;
    // ブロックの生成開始位置
    private float startX = -7.8f;
    private float startY = 0f;
    private float startZ = 4f;

    // ブロック生成間隔（秒）
    private float blockGenerationInterval = 10f;
    // ブロックの落下速度
    public float blockSlideSpeed = 0.1f;
    // 現在のブロックのリスト
    private List<GameObject> currentBlocks = new List<GameObject>();

    /// <summary>
    /// 壊したブロックの数
    /// </summary>
    public static int brokenBlockCount = 0;
    /// <summary>
    /// ゲーム開始前のカウントダウンテキスト
    /// </summary>
    public TMPro.TextMeshProUGUI countdownText;
    /// <summary>
    /// ゲーム開始前のカウントダウン時間（秒）
    /// </summary>
    private float countdownDuration = 3f;
    /// <summary>
    /// ゲーム進行中かどうかのフラグ
    /// </summary>
    private bool isGameActive = false;

    /// <summary>
    /// ゲーム進行フラグを設定します。
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

        uiManager = FindAnyObjectByType<GameOverUIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManagerが見つかりません。シーンにUIManagerオブジェクトがあるか確認してください。");
        }
    }

    IEnumerator StartGameCountdown()
    {
        isGameActive = false; // ゲーム進行フラグをオフにする

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

        isGameActive = true;
        Debug.Log("ゲームスタート！");

        ballController = FindAnyObjectByType<BallController>();
        ballController.LaunchBall();

        GenerateNewBlockRow();
    }

    void Update()
    {
        if (!isGameActive) return;

        timer += Time.deltaTime;

        if (timer >= blockGenerationInterval)
        {
            GenerateNewBlockRow();
            timer = 0f;
        }

        MoveExistingBlocksForward();
    }


    void GenerateNewBlockRow()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("ブロックのプレハブが設定されていません！GameManagerのインスペクターで設定してください。");
            return;
        }

        for (int i = 0; i < blocksPerRow; i++)
        {
            Vector3 spawnPosition = new Vector3(
                startX + (i * blockSpacingX),
                startY,
                startZ
            );
            GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
            Renderer blockRenderer = newBlock.GetComponent<Renderer>();
            if (blockRenderer != null)
            {
                blockRenderer.material.color = new Color(Random.value, Random.value, Random.value, 1f);
            }
            currentBlocks.Add(newBlock);

        }
        Debug.Log("新しいブロックの行が生成されました！");
    }

    void MoveExistingBlocksForward()
    {
        for (int i = currentBlocks.Count - 1; i >= 0; i--)
        {
            GameObject block = currentBlocks[i];
            if (block != null)
            {
                block.transform.position += Vector3.forward * -blockSlideSpeed * Time.deltaTime;
            }
            else
            {
                currentBlocks.RemoveAt(i);
            }
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        brokenBlockCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

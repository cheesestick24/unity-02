using UnityEngine;

public class BlockZBoundaryChecker : MonoBehaviour
{
    public float gameOverZThreshold;
    private GameOverUIManager uiManager;

    void Start()
    {
        // UIManagerインスタンスを取得
        uiManager = FindAnyObjectByType<GameOverUIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManagerが見つかりません。シーンにUIManagerオブジェクトがあるか確認してください。");
        }
        // gameOverZThreshold が未設定の場合のデバッグ用警告
        if (gameOverZThreshold == 0f)
        {
            Debug.LogWarning("gameOverZThresholdが設定されていません。手動で設定するか、GameManagerから渡してください。");
        }
    }

    void Update()
    {
        // ブロックのZ座標が閾値を超えたらゲームオーバー
        if (transform.position.z < gameOverZThreshold)
        {
            Debug.Log("ブロックが手前まで迫ってきた！ゲームオーバー！");

            // UIManagerにゲームオーバーUIの表示を依頼
            if (uiManager != null)
            {
                uiManager.ShowGameOverUI(GameManager.brokenBlockCount);
            }
            else
            {
                Time.timeScale = 0f; // 時間を停止
            }
        }
    }
}

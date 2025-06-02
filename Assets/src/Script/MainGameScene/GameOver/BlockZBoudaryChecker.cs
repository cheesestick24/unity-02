using UnityEngine;

public class BlockZBoundaryChecker : MonoBehaviour
{
    private float gameOverZThreshold = -4.5f; // ゲームオーバーのZ座標閾値
    private GameOverUIManager uiManager;

    void Start()
    {
        // UIManagerインスタンスを取得
        uiManager = FindAnyObjectByType<GameOverUIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManagerが見つかりません。シーンにUIManagerオブジェクトがあるか確認してください。");
        }
    }

    void Update()
    {
        // ブロックのZ座標が閾値を超えたらゲームオーバー
        if (transform.position.z < gameOverZThreshold)
        {
            Debug.Log("ブロックが手前まで迫ってきた！ゲームオーバー！");

            GameManager.Instance.GameOver();
        }
    }
}

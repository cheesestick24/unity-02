using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理のために必要 (UIManagerから呼ばれるが念のため残す)

public class GameOverZone : MonoBehaviour
{
    private GameOverUIManager uiManager;

    void Start()
    {
        // シーン内のUIManagerインスタンスを取得
        uiManager = FindAnyObjectByType<GameOverUIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManagerが見つかりません。シーンにUIManagerオブジェクトがあるか確認してください。");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトが「Ball」タグを持っているか確認
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("ゲームオーバー！ボールがゾーンに落ちました。");

            // UIManagerにゲームオーバーUIの表示を依頼
            if (uiManager != null)
            {
                uiManager.ShowGameOverUI(GameManager.brokenBlockCount);
            }
            else
            {
                Time.timeScale = 0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}

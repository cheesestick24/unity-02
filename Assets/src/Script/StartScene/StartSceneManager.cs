using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("ゲームを終了します");

        // エディターでテストするために、エディターを停止する処理（ビルドでは不要）
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // （オプション）ランキングボタンにこの関数を割り当てる
    public void OnRankingButtonClicked()
    {
        // ランキングシーンがあればロード
        // SceneManager.LoadScene("RankingScene");
        Debug.Log("ランキング画面に遷移");
    }
}

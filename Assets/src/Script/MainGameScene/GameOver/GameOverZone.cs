using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトが「Ball」タグを持っているか確認
        if (other.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.GameOver();
        }
    }
}

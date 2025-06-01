using UnityEngine;

public class BlockController : MonoBehaviour
{
    // ボールがブロックに衝突したときに呼ばれる
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.brokenBlockCount++;
            Debug.Log("Broken Block Count: " + GameManager.brokenBlockCount);
            Destroy(gameObject);
        }
    }
}

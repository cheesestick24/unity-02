using UnityEngine;

public class BlockController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.brokenBlockCount++;
            Destroy(gameObject);
        }
    }
}

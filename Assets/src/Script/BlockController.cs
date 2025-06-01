using UnityEngine;

public class BlockController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BlockController.OnCollisionEnter called with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.brokenBlockCount++;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
{
    Debug.Log("BlockController.OnTriggerEnter called with: " + other.gameObject.name);
    if (other.gameObject.CompareTag("Ball"))
    {
        GameManager.brokenBlockCount++;
        Destroy(gameObject);
    }
}
}

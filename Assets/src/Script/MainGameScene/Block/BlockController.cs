using UnityEngine;

public class BlockController : MonoBehaviour
{
    public AudioClip breakSound;
    public GameObject audioPlayerPrefab;
    public float time;

    private PlayAudioController playAudioController;

    private void Start()
    {
        playAudioController = FindAnyObjectByType<PlayAudioController>();
        if (playAudioController == null)
        {
            Debug.LogWarning("PlayAudioControllerがシーン内に見つかりません。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.brokenBlockCount++;
            playAudioController.PlayOneShotSound(breakSound, time);
            Destroy(gameObject); // ブロック自身はすぐに削除
        }
    }
}

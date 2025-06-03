using UnityEngine;

public class PlayAudioController : MonoBehaviour
{
    // Inspectorで設定するサウンド再生用プレハブ
    public GameObject audioPlayerPrefab;

    // 何らかのイベントで音を鳴らす関数
    public void PlayOneShotSound(AudioClip clipToPlay, float startTime = 0f)
    {
        Vector3 position = Vector3.zero;

        if (audioPlayerPrefab == null || clipToPlay == null)
        {
            Debug.LogWarning("AudioPlayerPrefabまたはAudioClipが設定されていません。");
            return;
        }

        // サウンド再生用プレハブを生成
        GameObject soundInstance = Instantiate(audioPlayerPrefab, position, Quaternion.identity);

        // 生成したオブジェクトのAudioSourceを取得
        AudioSource audioSource = soundInstance.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.clip = clipToPlay; // 再生するクリップを設定
            audioSource.time = startTime; // 指定された開始時間に設定
            audioSource.Play();
            // 音が鳴り終わったら、このサウンド再生オブジェクト自身を削除
            Destroy(soundInstance, clipToPlay.length);
        }
        else
        {
            Debug.LogError("生成されたAudioPlayerPrefabにAudioSourceコンポーネントが見つかりません！");
            Destroy(soundInstance); // 見つからない場合はすぐに削除
        }
    }
}

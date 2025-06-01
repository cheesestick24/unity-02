using UnityEngine;

public class BallController : MonoBehaviour
{
    public float fixedSpeed = 10f;
    private Rigidbody rb;
    private GameManager gameManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません。シーンにGameManagerオブジェクトがあるか確認してください。");
        }

        rb.linearVelocity = Vector3.zero; // 初期速度をゼロに設定
        rb.angularVelocity = Vector3.zero; // 初期角速度をゼロに設定
    }

    // ゲーム開始時やリスタート時にボールを発射する関数
    public void LaunchBall()
    {
        // ボールが停止している可能性を考慮して速度をリセット
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // ランダムなX方向と固定のZ方向の初速を与える
        float randomX = Random.Range(-1f, 1f);
        Vector3 initialDirection = new Vector3(randomX, 0, 1).normalized;

        rb.linearVelocity = initialDirection * fixedSpeed;
        Debug.Log("Ball Launched with speed: " + rb.linearVelocity.magnitude);
    }

    void FixedUpdate()
    {
        if (gameManager != null && !gameManager.IsGameActive())
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return; // 処理を終了
        }

        // 現在のボールの速度の大きさを取得
        float currentSpeed = rb.linearVelocity.magnitude;

        // ボールの速度が固定速度と異なっていたら補正する
        if (currentSpeed != fixedSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * fixedSpeed;
        }
    }
}

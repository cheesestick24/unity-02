using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float moveSpeed = 50f; // パドルの移動速度
    public float xLimit = 12f;     // パドルのX軸移動制限

    void Update()
    {
        // キーボード入力の取得
        // "Horizontal"はA/Dキーまたは左右矢印キーに割り当てられています。
        float horizontalInput = Input.GetAxis("Horizontal");

        // パドルを移動させる
        // Vector3.right は (1, 0, 0) を表し、X軸の正方向（右）を意味します。
        // horizontalInputが正なら右、負なら左に動きます。
        // Time.deltaTime を掛けることで、フレームレートに依存しない滑らかな移動を実現します。
        Vector3 moveDirection = Vector3.right * horizontalInput * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection);

        // パドルのX軸の移動を制限する
        // 現在のパドルの位置を取得
        Vector3 currentPosition = transform.position;
        // X座標を -xLimit から xLimit の範囲に制限
        currentPosition.x = Mathf.Clamp(currentPosition.x, -xLimit, xLimit);
        // 制限された位置をパドルに適用
        transform.position = currentPosition;
    }

    // オプション：ボールとの衝突時の挙動（ボールの速度を調整するなど）
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // 衝突したのがボールの場合
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // パドルの端に当たった場合にボールの横方向の速度を調整
                // 衝突した点のX座標を取得
                float hitPointX = collision.contacts[0].point.x;
                // パドルの中心X座標を取得
                float paddleCenterX = transform.position.x;
                // パドル中心からの相対位置を計算（左端に近ければ負、右端に近ければ正）
                float relativeHitX = hitPointX - paddleCenterX;
                // パドルの半分の幅を計算
                float paddleWidth = transform.localScale.x / 2;

                // ボールの現在の速度の大きさを取得（方向は考慮せず、速さだけ）
                float currentSpeed = ballRb.linearVelocity.magnitude;

                // 新しいボールの速度ベクトルを計算
                Vector3 newVelocity = new Vector3(
                    // パドル中心からの相対位置に基づいてX方向の速度を調整
                    // relativeHitX / paddleWidth で -1.0から1.0の範囲の値が得られる
                    // これに現在の速さの一部を掛け合わせる（0.5fは調整係数）
                    relativeHitX / paddleWidth * currentSpeed * 0.5f,
                    ballRb.linearVelocity.y, // Y方向の速度はそのまま
                    ballRb.linearVelocity.z  // Z方向の速度はそのまま
                ).normalized * currentSpeed; // 方向を正規化して、元の速さを維持する

                // ボールが垂直に張り付かないように、Z方向の最低速度を確保
                // Z方向の速度がほとんどない場合（absが0.1f以下）に調整
                if (Mathf.Abs(newVelocity.z) < 0.1f)
                {
                    // 元のZ速度が正なら0.1f、負なら-0.1fを設定し、最低限のZ方向移動を保証
                    newVelocity.z = newVelocity.z > 0 ? 0.1f : -0.1f;
                }

                // 計算した新しい速度をボールに適用
                ballRb.linearVelocity = newVelocity;
            }
        }
    }
}

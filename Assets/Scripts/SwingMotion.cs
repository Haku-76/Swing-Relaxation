using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public GameObject pivotObject; // 回転の中心となるオブジェクト
    public bool isSwinging = false; // スイングがアクティブかどうかのフラグ
    public float swingangle = 45.0f; // スイングの角度
    public float frequency = 1.0f; // スイングの頻度

    private float radius; // 中心からの距離
    private float angle = 0f; // 現在の角度
    private GameObject dot; // スイングするドットオブジェクト

    void Start()
    {
        radius = this.GetComponent<RandomDotGenerator>().RandomDotsDistance; // ランダムドットジェネレータから距離を取得
        dot = GameObject.FindWithTag("Dots"); // タグ'Dots'を持つオブジェクトを検索
        if (dot == null)
        {
            Debug.LogError("No GameObject with tag 'Dots' found!"); // オブジェクトが見つからない場合のエラー
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーでスイングのオン/オフ
        {
            isSwinging = !isSwinging;
        }

        if (isSwinging && dot != null) // スイングがアクティブで、ドットオブジェクトが存在する場合
        {
            angle = swingangle * Mathf.Sin(Time.time * frequency); // 角度を計算

            Vector3 swingDirection = Quaternion.Euler(0, angle, 0) * pivotObject.transform.forward; // スイング方向を計算
            Vector3 newPosition = pivotObject.transform.position + swingDirection * radius; // 新しい位置を計算

            dot.transform.position = newPosition; // ドットの位置を更新
            dot.transform.LookAt(pivotObject.transform); // ドットが中心オブジェクトを向くようにする
        }
    }
}

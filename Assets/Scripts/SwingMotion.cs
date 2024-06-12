using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public enum SWING_TYPE
    {
        SIDE = 0,
        UPDOWN = 1
    }

    public SWING_TYPE type; //スイングする方向を指定
    public bool isSwinging = false; // スイングがアクティブかどうかのフラグ
    public float swingangle = 45.0f; // スイングの角度
    public float frequency = 0.2f; // スイングの頻度（秒間何往復するか）

    private float angle = 0f; // 現在の角度
    private GameObject dot; // スイングするドットオブジェクト
    private float passedTime; //スイング開始時にドットの位置が急に変わらないようにするために経過時間で回転

    void Start()
    {
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
            passedTime += Time.deltaTime;
            angle = swingangle * Mathf.Sin(passedTime * frequency * 2 * Mathf.PI); // 角度を計算
            if (type == SWING_TYPE.SIDE)
            {
                dot.transform.localEulerAngles = new Vector3(0, angle, 0);
            }
            else if (type == SWING_TYPE.UPDOWN)
            {   
                dot.transform.localEulerAngles = new Vector3(angle, 0, 90);
            }
        }
    }
}

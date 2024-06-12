using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public enum SWING_TYPE
    {
        SIDE = 0,
        UPDOWN = 1,
        FORWARDBACK = 2
    }

    public SWING_TYPE type; //スイングする方向を指定
    public bool isSwinging = false; // スイングがアクティブかどうかのフラグ
    public float swingAngle = 45.0f; // スイングの角度(SIDE,UPDOWN用)
    public float swingRange = 5f; // 前後移動の範囲(FORWARDBACK用)
    public float frequency = 0.2f; // スイングの頻度（秒間何往復するか）
    public RandomDotGenerator generator; //RandomDot生成用

    private float angle = 0f; // 現在の角度
    private GameObject dot; // スイングするドットオブジェクト
    private float passedTime; //スイング開始時にドットの位置が急に変わらないようにするために経過時間で回転
    private Vector3 defaultPosition; //前後移動用に保存しておくドットの基準位置
    void Start()
    {
        //ドット生成用コンポーネントが見つからないなら探す
        if (generator == null)
        {
            generator = GetComponent<RandomDotGenerator>();
            if (generator == null)
            {
                Debug.LogError("No RandomDotGenerator Component Found!");
            }
        }

        //ドットの生成
        if (type == SWING_TYPE.FORWARDBACK)//前後なら円筒型にする
        {
            generator.RandomDotsAngle = 180;
            generator.CreateRandomDot(false);
        }
        else
        {
            generator.CreateRandomDot();
        }

        //ランダムドット群の取得
        dot = GameObject.FindWithTag("Dots"); // タグ'Dots'を持つオブジェクトを検索
        if (dot == null)
        {
            Debug.LogError("No GameObject with tag 'Dots' found!"); // オブジェクトが見つからない場合のエラー
        }
        defaultPosition = dot.transform.position;
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
            angle = swingAngle * Mathf.Sin(passedTime * frequency * 2 * Mathf.PI); // 角度を計算
            if (type == SWING_TYPE.SIDE)
            {
                dot.transform.localEulerAngles = new Vector3(0, angle, 0);
            }
            else if (type == SWING_TYPE.UPDOWN)
            {
                dot.transform.localEulerAngles = new Vector3(angle, 0, 90);
            }
            else if (type == SWING_TYPE.FORWARDBACK)
            {
                dot.transform.localEulerAngles = new Vector3(90, 0, 0);
                var offset = swingRange * Mathf.Sin(passedTime * frequency * 2 * Mathf.PI); // 角度を計算
                dot.transform.position = defaultPosition + new Vector3(0, 0, offset);   
            }
        }
    }
}

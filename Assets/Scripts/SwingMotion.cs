using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public enum SWING_TYPE
    {
        SIDE = 0,
        UPDOWN = 1,
        FORWARDBACK = 2
    }

    public enum MOVE_TYPE
    {
        SWING = 0,
        RANDOM = 1
    }

    public SWING_TYPE direction; //スイングする方向を指定
    public MOVE_TYPE moveType; //スイングするかランダムに動かすか指定
    public bool isMoving = false; // スイングがアクティブかどうかのフラグ
    public float swingAngle = 45.0f; // スイングの角度(SIDE,UPDOWN用)
    public float swingRange = 5f; // 前後移動の範囲(FORWARDBACK用)
    public float frequency = 0.2f; // スイングの頻度（秒間何往復するか）
    public RandomDotGenerator generator; //RandomDot生成用

    [Header("ランダムに動かす際のパラメータ")]
    [Tooltip("ドットをランダムに動かす際に方向を変える頻度")]
    public int changeDirectionFrequency = 10;
    [Tooltip("最大で秒間にどれだけ動くか")]
    public float translateRange = 1;
    [Tooltip("最大で秒間にどれだけ回るか（度）")]
    public float rotateRange = 10;

    private float angle = 0f; // 現在の角度
    private GameObject dot; // スイングするドットオブジェクト
    private float passedTime; // スイング開始時にドットの位置が急に変わらないようにするために経過時間で回転
    private Vector3 defaultPosition; // 前後移動用に保存しておくドットの基準位置
    private int movedCount = 0; // ランダムドットの方向変えるためのカウンタ
    private List<float> translateValues = new List<float>(); // 各ドットの平行移動幅を格納
    private List<float> rotateValues = new List<float>(); // 各ドットの回転量を格納
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
        
        if (direction == SWING_TYPE.FORWARDBACK)//前後なら円筒型にする
        {
            generator.RandomDotsAngle = 180;
            dot = generator.CreateRandomDot(false);
        }
        else
        {
            dot = generator.CreateRandomDot();
        }

        defaultPosition = dot.transform.position;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return)) // スイングのオン/オフ
        {
            isMoving = !isMoving;
        }

        if (Input.GetKeyDown(KeyCode.R)) // ドットの再生成
        {
            if (direction == SWING_TYPE.FORWARDBACK)//前後なら円筒型にする
            {
                generator.RandomDotsAngle = 180;
                dot = generator.CreateRandomDot(false);
            }
            else
            {
                dot = generator.CreateRandomDot();
            }
            if (dot == null)
            {
                Debug.LogError("No GameObject with tag 'Dots' found!"); // オブジェクトが見つからない場合のエラー
            }
        }

        //ランダムドット全体を回転させておく
        if (direction == SWING_TYPE.SIDE)
        {
            dot.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == SWING_TYPE.UPDOWN)
        {
            dot.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (direction == SWING_TYPE.FORWARDBACK)
        {
            dot.transform.localEulerAngles = new Vector3(90, 0, 0);
        }


        if (isMoving && dot != null) // スイングがアクティブで、ドットオブジェクトが存在する場合
        {
            if (moveType == MOVE_TYPE.SWING)
            {
                passedTime += Time.deltaTime;
                angle = swingAngle * Mathf.Sin(passedTime * frequency * 2 * Mathf.PI); // 角度を計算
                if (direction == SWING_TYPE.SIDE)
                {
                    dot.transform.localEulerAngles = new Vector3(0, angle, 0);
                }
                else if (direction == SWING_TYPE.UPDOWN)
                {
                    dot.transform.localEulerAngles = new Vector3(angle, 0, 90);
                }
                else if (direction == SWING_TYPE.FORWARDBACK)
                {
                    dot.transform.localEulerAngles = new Vector3(90, 0, 0);
                    var offset = swingRange * Mathf.Sin(passedTime * frequency * 2 * Mathf.PI); // 角度を計算
                    dot.transform.position = defaultPosition + new Vector3(0, 0, offset);
                }
            }
            if (moveType == MOVE_TYPE.RANDOM)
            {
                if (direction == SWING_TYPE.SIDE || movedCount >= 0)
                {
                    // 動かす方向の決定
                    if (translateValues.Count == 0 || movedCount >= changeDirectionFrequency)
                    {
                        movedCount = 0;
                        translateValues.Clear();
                        rotateValues.Clear();

                        for (int i = 0; i < generator.RandomDotsAmount; i++)
                        {
                            var val = Random.Range(-translateRange, translateRange);
                            var rot = Random.Range(-rotateRange, rotateRange);
                            translateValues.Add(val);
                            rotateValues.Add(rot);
                        }
                    }

                    //全ドットを動かす
                    int j = 0;
                    foreach (Transform child in dot.transform)
                    {
                        var grandChild = child.GetChild(0);
                        //上下に動かす
                        {
                            var maxHeight = generator.RandomDotsMaxHeight;
                            var nextY = grandChild.transform.localPosition.y + Time.deltaTime * translateValues[j];
                            if (nextY > maxHeight)
                            {
                                translateValues[j] *= -1;
                                nextY = maxHeight * 2 - nextY;
                            }
                            else if (nextY < -maxHeight)
                            {
                                translateValues[j] *= -1;
                                nextY = -maxHeight * 2 - nextY;
                            }
                            grandChild.localPosition = new Vector3(0, nextY, generator.RandomDotsDistance);
                        }
                        //左右に回す
                        {
                            var maxY = generator.RandomDotsAngle;
                            var nextY = child.localEulerAngles.y + Time.deltaTime * rotateValues[j];
                            if (nextY > 180) nextY -= 360;
                            if (nextY > maxY)
                            {
                                rotateValues[j] *= -1;
                                nextY = maxY * 2 - nextY;
                            }
                            else if (nextY < -maxY)
                            {
                                rotateValues[j] *= -1;
                                nextY = -maxY * 2 - nextY;
                            }
                            child.localEulerAngles = new Vector3(0, nextY, 0);
                        }

                        //サイズ調整
                        {
                            var distance = Vector3.Distance(grandChild.transform.position, generator.viewCamera.transform.position); // カメラからの距離に基づいたサイズ調整
                            float adjustedSize = generator.RandomDotsSize * distance / generator.RandomDotsDistance;
                            grandChild.transform.localScale = new Vector3(adjustedSize, adjustedSize, adjustedSize);
                        }

                        j++;
                    }
                }
                movedCount++;
            }
        }
    }
}

using Meta.WitAi;
using UnityEngine;

public class RandomDotGenerator : MonoBehaviour
{
    public GameObject viewCamera; // ドットを向けるカメラ
    public int seed = 1; // ランダム生成のためのシード値
    public float RandomDotsDistance = 10; // ドットの距離
    [Range(1f, 180)]
    public float RandomDotsAngle = 90; // ドットが配置される範囲の角度
    [Range(0.1f, 40f)]
    public float RandomDotsMaxHeight = 20; //ランダムドットの生成範囲（左右に振る場合の上下）の指定
    public float RandomDotsDensity = 10; // ドットの密度
    [Range(0.01f, 1f)]
    public float RandomDotsSize = 0.1f; // ドットの基本サイズ
    public int RandomDotsAmount; // 生成されるドットの数

    private GameObject parent;
    void Awake()
    {
        //カメラ割り当ててないなら探してみる
        if (viewCamera == null)
        {
            var cam = GameObject.FindGameObjectWithTag("MainCamera");
            if (cam != null)
            {
                viewCamera = cam;
            }
            else
            {
                Debug.LogError("No Camera Assigned/Found!");
            }
        }
    }

    /// <summary>
    /// ランダムドットを生成
    /// </summary>
    /// <param name="adjustScale">ランダムドットのサイズが遠いものほど大きくなるようにするか</param>
    /// <returns>ランダムドットの親オブジェクト</returns>
    public GameObject CreateRandomDot(bool adjustScale = true)
    {
        if (parent != null) Destroy(parent);
        Random.InitState(seed); // ランダムジェネレータの初期化
        float areaSize = 2 * Mathf.PI * RandomDotsDistance * RandomDotsMaxHeight * RandomDotsAngle / 180f; // ドットが配置される領域（面積）の計算
        int numberOfDots = (int)(areaSize * RandomDotsDensity); // ドットの数の計算
        RandomDotsAmount = numberOfDots;
        float baseSize = RandomDotsSize;
        parent = new GameObject("RandomDot"); // ドットの親オブジェクトを作成
        parent.tag = "Dots"; // タグを設定
        parent.transform.position = viewCamera.transform.position;

        for (int i = 0; i < numberOfDots; i++)
        {
            // 球体ドットを作成
            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            Renderer dotRenderer = dot.GetComponent<Renderer>();
            dotRenderer.material = new Material(Shader.Find("Unlit/Color")); // マテリアル設定
            dotRenderer.material.color = Color.white;

            // 球体の高度調整
            float y = Random.Range(-RandomDotsMaxHeight, RandomDotsMaxHeight); // y位置をランダムに設定
            Vector3 randomPosition = parent.transform.position + new Vector3(0, y, RandomDotsDistance);
            dot.transform.position = randomPosition;

            //球体の方向調整
            var centerObject = new GameObject("Center");
            centerObject.transform.SetParent(parent.transform);
            centerObject.transform.localPosition = Vector3.zero;
            dot.transform.SetParent(centerObject.transform); // 親オブジェクトに設定
            float theta = Random.Range(-RandomDotsAngle, RandomDotsAngle);
            centerObject.transform.localEulerAngles = new Vector3(0, theta, 0);

            // 球体のサイズ調整
            float distance = RandomDotsDistance;
            if (adjustScale) distance = Vector3.Distance(randomPosition, viewCamera.transform.position); // カメラからの距離に基づいたサイズ調整
            float adjustedSize = baseSize * distance / RandomDotsDistance;
            dot.transform.localScale = new Vector3(adjustedSize, adjustedSize, adjustedSize);

        }
        return parent;
    }
}

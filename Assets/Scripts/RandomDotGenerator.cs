using UnityEngine;

public class RandomDotGenerator : MonoBehaviour
{
    public GameObject viewCamera; // ドットを向けるカメラ
    public int seed = 1; // ランダム生成のためのシード値
    public float RandomDotsDistance = 10; // ドットの距離
    [Range(0, 180)]
    public float RandomDotsAngle = 45; // ドットが配置される範囲の角度
    public float RandomDotsDensity = 10; // ドットの密度
    public float RandomDotsSize = 0.1f; // ドットの基本サイズ
    public float RandomDotsMaxHeight = 10;
    public int RandomDotsAmount; // 生成されるドットの数

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

        CreateRandomDot(); // ドットを生成する
    }

    public void CreateRandomDot()
    {
        Random.InitState(seed); // ランダムジェネレータの初期化
        float areaSize = 2 * Mathf.PI * RandomDotsDistance * RandomDotsMaxHeight * RandomDotsAngle / 180f; // ドットが配置される領域（面積）の計算
        int numberOfDots = (int)(areaSize * RandomDotsDensity); // ドットの数の計算
        RandomDotsAmount = numberOfDots;
        float baseSize = RandomDotsSize;
        GameObject parent = new GameObject("RandomDot"); // ドットの親オブジェクトを作成
        parent.tag = "Dots"; // タグを設定
        parent.transform.position = viewCamera.transform.position;

        for (int i = 0; i < numberOfDots; i++)
        {
            float theta = Random.Range(90 - RandomDotsAngle, 90 + RandomDotsAngle) / 180 * Mathf.PI; // ドットのカメラからの方向を決定
            float x = RandomDotsDistance * Mathf.Cos(theta); // x位置を求める
            float y = Random.Range(-RandomDotsMaxHeight, RandomDotsMaxHeight); // y位置をランダムに設定
            float z = RandomDotsDistance * Mathf.Sin(theta); // z位置を求める
            Vector3 randomPosition = parent.transform.position + new Vector3(x, y, z);

            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere); // 球体ドットを作成
            dot.transform.position = randomPosition;

            float distance = Vector3.Distance(randomPosition, viewCamera.transform.position); // カメラからの距離に基づいたサイズ調整
            float adjustedSize = baseSize * distance / RandomDotsDistance;
            dot.transform.localScale = new Vector3(adjustedSize, adjustedSize, adjustedSize);
            Renderer dotRenderer = dot.GetComponent<Renderer>();
            dotRenderer.material = new Material(Shader.Find("Unlit/Color")); // マテリアル設定
            if (i < numberOfDots / 2)
                dotRenderer.material.color = Color.white; // ドットの色を半分は白に
            else
                dotRenderer.material.color = Color.black; // 半分は黒に
            dot.transform.SetParent(parent.transform); // 親オブジェクトに設定
        }
    }
}

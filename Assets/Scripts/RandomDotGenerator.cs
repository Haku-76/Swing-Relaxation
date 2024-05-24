using UnityEngine;

public class RandomDotGenerator : MonoBehaviour
{
    public GameObject viewCamera; // ドットを向けるカメラ
    public int seed = 1; // ランダム生成のためのシード値
    public float RandomDotsDistance = 10; // ドットの距離
    public float RandomDotsAngle = 45; // ドットが配置される範囲の角度
    public float RandomDotsDensity = 10; // ドットの密度
    public float RandomDotsSize = 0.1f; // ドットの基本サイズ
    public int RandomDotsAmount; // 生成されるドットの数

    void Awake()
    {
        CreateRandomDot(); // ドットを生成する
    }

    public void CreateRandomDot()
    {
        Random.InitState(seed); // ランダムジェネレータの初期化
        float areaSize = 2 * (RandomDotsDistance * Mathf.Tan(RandomDotsAngle / 180.0f * Mathf.PI)); // ドットが配置される領域の計算
        int numberOfDots = (int)(areaSize * areaSize * RandomDotsDensity); // ドットの数の計算
        RandomDotsAmount = numberOfDots;
        float baseSize = RandomDotsSize;
        GameObject parent = new GameObject("RandomDot"); // ドットの親オブジェクトを作成
        parent.transform.position = new Vector3(0, 0, RandomDotsDistance);
        parent.tag = "Dots"; // タグを設定

        for (int i = 0; i < numberOfDots; i++)
        {
            float x = Random.Range(-areaSize / 2.0f, areaSize / 2.0f); // x位置をランダムに設定
            float y = Random.Range(-areaSize / 2.0f, areaSize / 2.0f); // y位置をランダムに設定
            Vector3 randomPosition = new Vector3(x, y, RandomDotsDistance);

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

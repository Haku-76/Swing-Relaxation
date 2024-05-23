using UnityEngine;

public class RandomDotGenerator : MonoBehaviour
{
    public GameObject viewCamera;
    public Material blackMaterial;
    public int seed = 1;
    public float RandomDotsDistance = 10;
    public float RandomDotsAngle = 45;
    public float RandomDotsDensity = 100;
    public float RandomDotsSize = 0.1f;
    public int RandomDotsAmount;

    void Awake()
    {
        CreateRandomDot();
    }

    public void CreateRandomDot()
    {
        Random.InitState(seed);
        float areaSize = 2 * (RandomDotsDistance * Mathf.Tan(RandomDotsAngle / 180.0f * Mathf.PI));
        int numberOfDots = (int)(areaSize * areaSize * RandomDotsDensity);
        RandomDotsAmount = numberOfDots;
        float baseSize = RandomDotsSize;
        GameObject parent = new GameObject("RandomDot");
        parent.tag = "Dots";

        for (int i = 0; i < numberOfDots; i++)
        {
            float x = Random.Range(-areaSize / 2.0f, areaSize / 2.0f);
            float y = Random.Range(-areaSize / 2.0f, areaSize / 2.0f);
            Vector3 randomPosition = new Vector3(x, y, RandomDotsDistance);

            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dot.transform.position = randomPosition;

            float distance = Vector3.Distance(randomPosition, viewCamera.transform.position);
            float adjustedSize = baseSize * distance / RandomDotsDistance;
            dot.transform.localScale = new Vector3(adjustedSize, adjustedSize, adjustedSize);
            dot.GetComponent<Renderer>().material = blackMaterial;
            dot.transform.SetParent(parent.transform);
        }
    }

}

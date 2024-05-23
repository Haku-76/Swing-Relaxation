using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public GameObject pivotObject; // 中心物体
    public float distance = 5.0f; // 单摆的绳长
    public float frequency = 1.0f; // 频率
    public float amplitude = 1.0f; // 振幅
    public Vector3 direction = Vector3.right; // 摆动方向

    private bool isSwinging = false;
    private float angle = 0f;
    private GameObject dot;

    void Start()
    {
        dot = GameObject.FindWithTag("Dots");
        if (dot == null)
        {
            Debug.LogError("No GameObject with tag 'Dots' found!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSwinging = !isSwinging;
            angle = 0f;
        }

        if (isSwinging && dot != null)
        {
            angle += Time.deltaTime * frequency * 2 * Mathf.PI;

            float offsetX = Mathf.Sin(angle) * amplitude;
            float offsetY = Mathf.Cos(angle) * amplitude;
            Vector3 offsetPosition = pivotObject.transform.position + direction.normalized * distance + new Vector3(offsetX, -offsetY, 0);
            dot.transform.position = offsetPosition;
        }
    }
}

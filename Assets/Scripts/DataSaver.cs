using UnityEngine;
using System.IO;

public class DataSaver : MonoBehaviour
{
    public string subject;
    public string condition;
    public string trial;

    public string filename;

    [Space(10)]
    public GameObject Camera;

    [Space(10)]
    public float startTime;
    public float runTime;
    public int frameCounter;

    [Space(10)]
    public Vector3 CameraDirection;
    public Vector3 CameraPosition;
    public Vector3 CameraVelocity;

    private Vector3 previousCameraPosition;
    private float previousTime;

    void Start()
    {
        startTime = Time.time;
        frameCounter = 0;

        filename = subject + "_" + condition + "_" + trial;

        previousCameraPosition = Camera.transform.position;
        previousTime = Time.time;

        SaveHeader();
    }

    private void Update()
    {
        CameraDirection = Camera.transform.localEulerAngles;
        CameraPosition = Camera.transform.position;

        float currentTime = Time.time;
        float deltaTime = currentTime - previousTime;
        if (deltaTime > 0)
        {
            CameraVelocity = (CameraPosition - previousCameraPosition) / deltaTime;
        }

        previousCameraPosition = CameraPosition;
        previousTime = currentTime;

        frameCounter += 1;
        runTime = Time.time - startTime;

        SaveData(filename);
    }

    private void SaveHeader()
    {
        string path = Application.dataPath + "/Data/" + filename + ".csv";
        if (!File.Exists(path))
        {
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine("FrameCounter,RunTime," +
                         "CameraPositionX,CameraPositionY,CameraPositionZ," +
                         "CameraDirectionX,CameraDirectionY,CameraDirectionZ," +
                         "CameraVelocityX,CameraVelocityY,CameraVelocityZ"
                         );
            sw.Close();
        }
    }

    private void SaveData(string fileName)
    {
        string filePath = Application.dataPath + "/Data/" + fileName + ".csv";
        string dataLine = string.Join(",", new string[]
        {frameCounter.ToString(), runTime.ToString(),
        CameraPosition.x.ToString(), CameraPosition.y.ToString(),CameraPosition.z.ToString(),
        CameraDirection.x.ToString(), CameraDirection.y.ToString(),CameraDirection.z.ToString(),
        CameraVelocity.x.ToString(), CameraVelocity.y.ToString(), CameraVelocity.z.ToString()
        });

        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            sw.WriteLine(dataLine);
        }
    }
}

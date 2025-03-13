using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManagement : MonoBehaviour
{
    int maxFrameRate = 99;
    public float targetFrameRate = 60.0f;
    float currentFrameRate;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxFrameRate;
        currentFrameRate = Time.realtimeSinceStartup;
        StartCoroutine(WaitForNextFrame());
    }

    IEnumerator WaitForNextFrame()
    {
        while(true) {
            yield return new WaitForEndOfFrame();
            currentFrameRate = 1.0f / Time.deltaTime;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameRate - t - 0.01f;
            if (sleepTime > 0) {
                System.Threading.Thread.Sleep((int)(sleepTime * 1000));
            }
            while (t < currentFrameRate) {
                t = Time.realtimeSinceStartup;
            }
        }
    } 
}

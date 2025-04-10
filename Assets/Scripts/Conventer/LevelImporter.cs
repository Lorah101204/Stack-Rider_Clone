using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelImporter : MonoBehaviour
{
    [Header("Level ID")]
    public int levelId = 1;

    [Header("Prefab")]
    public GameObject backgroundCamPrefab;
    public GameObject playerPrefab;
    public GameObject startPlatformPrefab;
    public GameObject endPlatformPrefab;
    public GameObject roadPrefab;
    public GameObject stackBallPrefab;
    public GameObject coinPrefab;
    public GameObject wallPrefab;

    void Start()
    {
        LoadLevel(levelId);
    }

    public void LoadLevel(int id) 
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"LevelData/Level_{id}");
        if (jsonFile == null) 
        {
            Debug.LogError($"Level data for level {id} not found!");
            return;
        }

        LevelData levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);

        GameObject levelParent = new GameObject($"Level_{id}");

        if (backgroundCamPrefab != null) 
        {
            Instantiate(backgroundCamPrefab, levelData.backgroundCam.ToVector3(), Quaternion.identity, levelParent.transform);
        }

        if (playerPrefab != null) 
        {
            Instantiate(playerPrefab, levelData.player.ToVector3(), Quaternion.identity, levelParent.transform);
        }

        if (startPlatformPrefab != null) 
        {
            Instantiate(startPlatformPrefab, levelData.startPlatform.ToVector3(), Quaternion.identity, levelParent.transform);
        }

        if (endPlatformPrefab != null) 
        {
            Instantiate(endPlatformPrefab, levelData.endPlatform.ToVector3(), Quaternion.identity, levelParent.transform);
        }

        foreach (var pos in levelData.allRoads) 
        {
            Instantiate(roadPrefab, pos.ToVector3(), Quaternion.identity, levelParent.transform);
        }

        foreach (var ball in levelData.allBalls)
        {
            GameObject b = Instantiate(stackBallPrefab, ball.position.ToVector3(), Quaternion.identity, levelParent.transform);
            StackBall sb = b.GetComponent<StackBall>();
            if (sb != null) 
            {
                sb.amount = ball.amount;
            }
        }
        foreach (var wall in levelData.allWalls) 
        {
            GameObject w = Instantiate(wallPrefab, wall.position.ToVector3(), Quaternion.identity, levelParent.transform);
            NamedPrefab obstacle = w.GetComponent<NamedPrefab>();
            if (obstacle != null) 
            {
                obstacle.type = wall.type;
            }
        }
        foreach (var coin in levelData.allCoins) 
        {
            Instantiate(coinPrefab, coin.ToVector3(), Quaternion.identity, levelParent.transform);
        }

        Debug.Log($"Level {id} loaded successfully!");
    }
}

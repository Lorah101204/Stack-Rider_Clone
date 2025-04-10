/*using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class LevelExport : MonoBehaviour
{
    //[MenuItem("Tools/Export Level")]
    public static void ExportLevel() 
    {
        LevelData levelData = new LevelData();
        levelData.levelId = 1;

        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        levelData.backgroundCam = Vector3Data.FromVector3(cam.transform.position);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        levelData.player = Vector3Data.FromVector3(player.transform.position);

        GameObject startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
        levelData.startPlatform = Vector3Data.FromVector3(startPlatform.transform.position);

        GameObject endPlatform = GameObject.FindGameObjectWithTag("EndPlatform");
        levelData.endPlatform = Vector3Data.FromVector3(endPlatform.transform.position);

        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        levelData.allRoads = new List<Vector3Data>();
        foreach(var r in roads) 
        {
            levelData.allRoads.Add(Vector3Data.FromVector3(r.transform.position));
        }

        GameObject[] balls = GameObject.FindGameObjectsWithTag("StackBall");
        levelData.allBalls = new List<StackBallData>();
        foreach(var ball in balls)
        {
            StackBall ballData = ball.GetComponent<StackBall>();
            int amount = ballData != null ? ballData.amount : 1;
            levelData.allBalls.Add(new StackBallData 
            { 
                position = Vector3Data.FromVector3(ball.transform.position), 
                amount = amount 
            });
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Obstacles");
        levelData.allWalls = new List<ObstacleData>();
        foreach(var wall in walls) 
        {
            levelData.allWalls.Add(new ObstacleData 
            { 
                type = wall.name.Replace("(Clone)", "").Trim(),
                position = Vector3Data.FromVector3(wall.transform.position)
            });
        }

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coins");
        levelData.allCoins = new List<Vector3Data>();
        foreach(var coin in coins) 
        {
            levelData.allCoins.Add(Vector3Data.FromVector3(coin.transform.position));
        }

        string json = JsonUtility.ToJson(levelData, true);

        string folderPath = "Assets/Resources/LevelData";
        if (!Directory.Exists(folderPath)) 
        {
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, $"Level_{levelData.levelId}.json");

        File.WriteAllText(filePath, json);
        AssetDatabase.Refresh();

        Debug.Log($"Level data exported to {filePath}");
    }
}*/

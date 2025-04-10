using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int levelId;

    public Vector3Data backgroundCam;
    public Vector3Data player;
    public Vector3Data startPlatform;
    public Vector3Data endPlatform;

    public List<Vector3Data> allRoads;
    public List<StackBallData> allBalls;
    public List<ObstacleData> allWalls;
    public List<Vector3Data> allCoins;
}

[Serializable]
public class StackBallData
{
    public Vector3Data position;
    public int amount;
}

[Serializable]
public class ObstacleData
{
    public string type;
    public Vector3Data position;
}

[Serializable]
public class Vector3Data
{
    public float x, y, z;

    public Vector3 ToVector3() => new Vector3(x, y, z);
    public static Vector3Data FromVector3(Vector3 v) => new Vector3Data { x = v.x, y = v.y, z = v.z };
}

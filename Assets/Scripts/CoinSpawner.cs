using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrelab;
    public int[] coinCounts = {10, 20, 30};
    public float spacing = 0.5f;
    public string axis = "X";

    void SpawnCoinss() {
        int coinCount = coinCounts[Random.Range(0,coinCounts.Length)];

        Vector3 spawnPosition = transform.position;
        Quaternion coinRotation = Quaternion.identity;
        if (axis.ToUpper() == "Z") {
            coinRotation = Quaternion.Euler(0, 90, 0);
        }
        for (int i = 0; i < coinCount; i++) {
            GameObject coin = Instantiate(coinPrelab, spawnPosition, coinRotation);

            if (axis.ToUpper() == "X") {
                spawnPosition.x += spacing; 
            } else {
                spawnPosition.z += spacing;
            }
        }   
    }

}

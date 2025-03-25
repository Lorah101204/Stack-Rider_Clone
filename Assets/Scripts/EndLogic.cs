using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLogic : MonoBehaviour
{
    public Transform targetPosition; 
    public float moveSpeed = 3f;     

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            StartCoroutine(MoveToPosition(other.transform, targetPosition.position));
        }
    }

    IEnumerator MoveToPosition(Transform player, Vector3 destination)
    {
        float elapsedTime = 0f;
        float duration = 1.5f;

        Vector3 startingPos = player.position;
        while (elapsedTime < duration) 
        {
            player.position = Vector3.Lerp(startingPos, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        player.position = destination; 
    }
}


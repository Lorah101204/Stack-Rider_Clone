using System.Collections;
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
        float duration = 1f;

        Vector3 startingPos = player.position;
        while (elapsedTime < duration) 
        {
            Vector3 newPosition = Vector3.Lerp(startingPos, destination, elapsedTime / duration);   
            player.position = new Vector3(newPosition.x, startingPos.y, newPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        player.position = destination; 
    }
}


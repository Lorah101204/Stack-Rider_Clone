using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParticle : MonoBehaviour
{
    public ParticleSystem endParticle;

    void Start() 
    {
        endParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            endParticle.Play();
        }
    }
}

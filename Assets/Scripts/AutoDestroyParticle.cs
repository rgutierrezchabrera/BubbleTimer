using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    void Start()
    {
        // Automatically destroy this GameObject after the particle system finishes
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}

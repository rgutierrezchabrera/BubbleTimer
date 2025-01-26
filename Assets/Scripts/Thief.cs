using UnityEngine;

public class Thief: MonoBehaviour
{
    private Animator animator; 
    private void Start()
    {
    animator = transform.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the SafeZone
        if (other.CompareTag("foundSafe"))
        {
            Debug.Log("Entered the Safe Zone!");

            // Trigger the "kneelingDown" animation on the child's Animator
            animator.SetTrigger("foundSafe");
        }
    }
}

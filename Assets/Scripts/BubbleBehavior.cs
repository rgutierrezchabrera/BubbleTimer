using UnityEngine;
using System.Collections;

public class BubbleBehavior : MonoBehaviour
{
    public GameObject popEffect;    // Prefab for the pop effect

    void Start()
    {
        StartCoroutine(BubbleGrowthSequence());
    }

    private IEnumerator BubbleGrowthSequence()
    {
        // Define the sequence of target sizes
        float[] sizes = new float[] { 0.04f, 0.06f, 0.08f, 0.10f, 0.12f, 0.14f, 0.16f };

        // Loop through each target size
        foreach (float targetSize in sizes)
        {
            // Grow from 0.02 to target size
            yield return StartCoroutine(GrowBubble(targetSize));

            // Shrink back to 0.02 if the target size is less than 0.16
            if (targetSize < 0.16f)
            {
                yield return StartCoroutine(ShrinkBubble());
            }
        }

        // Once we finish growing to 0.16, explode (without shrinking back to 0.02)
        Explode();
    }

    private IEnumerator GrowBubble(float targetScale)
    {
        float currentScale = 0.02f; // Start at 0.02
        float growDuration = 3f;  // Set duration to grow to target size (3 seconds)
        float elapsedTime = 0f;

        // Grow the bubble smoothly to the target size
        while (elapsedTime < growDuration)
        {
            elapsedTime += Time.deltaTime;
            currentScale = Mathf.Lerp(0.02f, targetScale, elapsedTime / growDuration);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            yield return null; // Wait for the next frame
        }

        // Ensure the scale is exactly at the target size at the end
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        Debug.Log("Reached target size: " + targetScale);
    }

    private IEnumerator ShrinkBubble()
    {
        float currentScale = transform.localScale.x;
        float shrinkDuration = 2f;  // Set duration to shrink back to 0.02 (2 seconds)
        float elapsedTime = 0f;

        // Shrink the bubble smoothly back to 0.02
        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            currentScale = Mathf.Lerp(transform.localScale.x, 0.02f, elapsedTime / shrinkDuration);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            yield return null; // Wait for the next frame
        }

        // Ensure the scale is exactly at 0.02 at the end
        transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        Debug.Log("Bubble shrunk back to 0.02");
    }

    private void Explode()
    {
        // Instantiate the pop effect at the bubble's position
        if (popEffect != null)
        {
            Instantiate(popEffect, transform.position, Quaternion.identity);
        }

        // Destroy the bubble object after it explodes
        Destroy(gameObject);
        Debug.Log("Bubble exploded!");
    }
}

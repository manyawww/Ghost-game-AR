using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunlala : MonoBehaviour
{
    // Assign these via the Inspector:
    // The "Image target" GameObject (which is deactivated in Start).
    public GameObject imageTarget; 
    // "Sun plane" GameObject; assign directly or find as a child.
    public GameObject sunPlane; 
    // AudioSource to play when activated.
    public AudioSource audioSource; 

    // This flag ensures activation happens only once per tracking session.
    private bool activated = false;
    // This flag tracks the previous state of the image target (i.e., whether it was tracked).
    private bool wasTracked = false;

    void Start()
    {
          // Initially, deactivate the Image target.
        if (imageTarget != null)
        {
            imageTarget.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Image target is not assigned in Start!");
        }
    }

    void OnEnable()
    {
        
    }

    void Update()
    {
      

        // Check if the Image target is currently being tracked (active in the hierarchy)
        if (imageTarget != null)
        {
            bool isTracked = imageTarget.activeInHierarchy;
            
            // If the target just came back into view after being lost,
            // reset the activation flag.
            if (isTracked && !wasTracked)
            {
                activated = false;
            }
            wasTracked = isTracked;
        }

        // Only allow tap activation when the Image target is being tracked.
        if (!activated && 
           (Input.GetKeyDown(KeyCode.Space)))
        {
            ActivateTarget();
        }
    }

    public void ActivateTarget()
    {
        // Ensure the Image target is active.
        if (imageTarget != null)
        {
            imageTarget.SetActive(true);

            // Activate the Sun plane.
            if (sunPlane != null)
            {
                sunPlane.SetActive(true);
            }
            else
            {
                // Alternatively, find "Sun plane" as a child of the Image target.
                Transform child = imageTarget.transform.Find("Sun plane");
                if (child != null)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Sun plane not found as a child of Image target!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Image target is not assigned!");
        }

        // Play the audio if the AudioSource is assigned.
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio Source is not assigned!");
        }

        // Set the flag so that activation won't occur again until reset.
        activated = true;
    }

    public void SettingImageTargetOn()
    {
        if (imageTarget != null)
        {
            imageTarget.SetActive(false);
        }
    }
}

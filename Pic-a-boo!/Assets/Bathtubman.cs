using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Bathtubman : MonoBehaviour
{
    // Assign these via the Inspector:
    // The "Duck Target Test" GameObject (which is deactivated in Start).
    public GameObject duckTargetTest; 
    // "Bathtub Man Plane" GameObject; assign directly or find as a child.
    public GameObject bathtubManPlane; 
    // AudioSource2 to play when activated.
    public AudioSource audioSource2; 

    public GameObject scanAnimation;

    public Button scanButton;

    public Animator scanAnim;


    // This flag ensures activation happens only once per tracking session.
    private bool activated = false;
    // This flag tracks the previous state of the Duck Target Test (i.e., whether it was tracked).
    private bool wasTracked = false;

    void Start()
    {
          // Initially, deactivate the Duck Target Test.
        if (duckTargetTest != null)
        {
            duckTargetTest.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Duck Target Test is not assigned in Start!");
        }

        if (scanAnimation != null)
        {
            scanAnimation.SetActive(false);
        }

        // If you want the button listener to be set at Start:
        if (scanButton != null)
        {
            scanButton.onClick.AddListener(PlayAnimation);
        }
    }



    void Update()
    {
        // Check if the Duck Target Test is currently being tracked (active in the hierarchy)
        if (duckTargetTest != null)
        {
            bool isTracked = duckTargetTest.activeInHierarchy;
            
            // If the target just came back into view after being lost,
            // reset the activation flag.
            if (isTracked && !wasTracked)
            {
                activated = false;
            }
            wasTracked = isTracked;
        }

        // Only allow tap activation when the Duck Target Test is being tracked.
        if (!activated && 
           (Input.GetKeyDown(KeyCode.Space)))
        {
            ActivateTarget();
        }
    }

    public void ActivateTarget()
    {
        // Ensure the Duck Target Test is active.
        if (duckTargetTest != null)
        {
            duckTargetTest.SetActive(true);

            // Activate the Bathtub Man Plane.
            if (bathtubManPlane != null)
            {
                bathtubManPlane.SetActive(true);
            }
            else
            {
                // Alternatively, find "Bathtub Man Plane" as a child of the Duck Target Test.
                Transform child = duckTargetTest.transform.Find("Bathtub Man Plane");
                if (child != null)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Bathtub Man Plane not found as a child of Image target!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Duck Target Test is not assigned!");
        }

        // Play the audio if the AudioSource2 is assigned.
        if (audioSource2 != null)
        {
            audioSource2.Play();
        }
        else
        {
            Debug.LogWarning("Audio Source2 is not assigned!");
        }

        // Set the flag so that activation won't occur again until reset.
        activated = true;
    }

    public void PlayAnimation()
    {
        if (scanAnim != null)
        {
            // If using Animator, set a Trigger named "Play" 
            scanAnim.SetTrigger("Play"); 
        } 
        else
        {
            Debug.LogWarning("scanAnim is not assigned!");
        }

        if (scanAnimation != null)
        {
            // Optionally activate the GameObject if you want its child visuals to show
            scanAnimation.SetActive(true);
        }
    }

    public void SettingImageTargetOn()
    {
        if (duckTargetTest != null)
        {
            duckTargetTest.SetActive(false);
        }
    }
    
}
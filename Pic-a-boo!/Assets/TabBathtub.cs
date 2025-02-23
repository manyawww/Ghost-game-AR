using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TabBathtub : MonoBehaviour
{
    // Assign these via the Inspector:
    public GameObject duckTargetTab; 
    public GameObject bathtubManPlaneTab; 
    public AudioSource audioSourceTab; 
    public GameObject scanAnimation;
    public Button scanButton;
    public Animator scanAnim;
    public GameObject captions;
    
    // Sprite to use when target is activated (e.g., grayed out)
    public Sprite activatedSprite;
    
    // New: The camera button and its animator.
    public Button cameraButton;
    public Animator cameraAnim;
    // Delay after clicking the camera button before deactivating the AR marker.
    public float cameraDelay = 1f;
    
    // This will store the scan button's original/default sprite.
    private Sprite defaultSprite;

    private bool activated = false;
    private bool wasTracked = false;
    private ColorBlock defaultButtonColors;
    private ColorBlock disabledButtonColors;
    
    // Flag to track if audio was triggered for the current activation.
    private bool audioTriggered = false;

    void Start()
    {
        // Deactivate the Duck Target initially.
        if (duckTargetTab != null)
            duckTargetTab.SetActive(false);
        else
            Debug.LogWarning("Duck Target Tab is not assigned in Start!");

        // Keep scan animation inactive.
        if (scanAnimation != null)
            scanAnimation.SetActive(false);

        // Deactivate captions at the start.
        if (captions != null)
            captions.SetActive(false);
        else
            Debug.LogWarning("Captions is not assigned in Start!");

        // Hook the scan button's event and store its default colors and sprite.
        if (scanButton != null)
        {
            scanButton.onClick.AddListener(PlayAnimation);
            defaultButtonColors = scanButton.colors;
            defaultSprite = scanButton.GetComponent<Image>().sprite;

            // Setup disabled (gray) colors.
            disabledButtonColors = defaultButtonColors;
            disabledButtonColors.normalColor = Color.gray;
            disabledButtonColors.highlightedColor = Color.gray;
            disabledButtonColors.pressedColor = Color.gray;
            disabledButtonColors.disabledColor = Color.gray;
        }
        else
        {
            Debug.LogWarning("Scan Button is not assigned!");
        }
        
        // Initialize the camera button: hide it and hook its click event.
        if (cameraButton != null)
        {
            cameraButton.gameObject.SetActive(false);
            cameraButton.onClick.AddListener(OnCameraButtonClicked);
        }
        else
        {
            Debug.LogWarning("Camera Button is not assigned!");
        }
        
        // IMPORTANT: Ensure the camera animation object is disabled at the start.
        if (cameraAnim != null)
        {
            cameraAnim.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Camera Animator is not assigned!");
        }
    }

    void Update()
    {
        // Check whether the duckTargetTab is currently tracked.
        bool isTracked = duckTargetTab != null && duckTargetTab.activeInHierarchy;

        // If the target just came back into view after being lost, you could reset activation here.
        if (isTracked && !wasTracked)
        {
            // Optionally reset activation if needed.
            // activated = false;
        }
        wasTracked = isTracked;
        
        // If the target is not tracked, ensure captions are turned off and revert the scan button.
        if (!isTracked && activated)
        {
            activated = false;
            if (captions != null && captions.activeSelf)
                captions.SetActive(false);
            if (scanButton != null && defaultSprite != null)
            {
                scanButton.interactable = true;
                scanButton.colors = defaultButtonColors;
                scanButton.GetComponent<Image>().sprite = defaultSprite;
            }
        }

        // Update the scan button's appearance based on activation state.
        if (scanButton != null)
        {
            if (activated)
            {
                scanButton.interactable = false;
                scanButton.colors = disabledButtonColors;
                // Set the activated sprite if assigned.
                if (activatedSprite != null)
                    scanButton.GetComponent<Image>().sprite = activatedSprite;
            }
            else
            {
                scanButton.interactable = true;
                scanButton.colors = defaultButtonColors;
                // Revert to the default sprite.
                if (defaultSprite != null)
                    scanButton.GetComponent<Image>().sprite = defaultSprite;
            }
        }
        
        // --- Camera Button Logic ---
        // The camera button should appear only after audio has been triggered and finished.
        if (audioTriggered && (audioSourceTab == null || !audioSourceTab.isPlaying))
        {
            if (!cameraButton.gameObject.activeSelf)
            {
                cameraButton.gameObject.SetActive(true);
                Debug.Log("Camera Button activated (audio finished).");
            }
        }
        else
        {
            if (cameraButton.gameObject.activeSelf)
            {
                cameraButton.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called when the scan button is tapped.
    /// Plays the scan animation and, after a delay, attempts to activate the target.
    /// </summary>
    public void PlayAnimation()
    {
        StartCoroutine(ScanThenActivate());
    }

    private IEnumerator ScanThenActivate()
    {
        // Trigger the Animator if assigned.
        if (scanAnim != null)
        {
            scanAnim.SetTrigger("Play");
        }
        else
        {
            Debug.LogWarning("scanAnim is not assigned!");
        }

        // Show the scan animation.
        if (scanAnimation != null)
            scanAnimation.SetActive(true);

        // Wait for 3 seconds (duration of the scanning effect).
        yield return new WaitForSeconds(3f);

        // Attempt to activate the target.
        if (duckTargetTab != null && !activated)
            ActivateTarget();

        // Hide the scan animation.
        if (scanAnimation != null)
            scanAnimation.SetActive(false);
    }

    /// <summary>
    /// Activates the Duck Target and its related objects.
    /// Then, only if the Bathtub Man Plane Tab is active, plays audio, shows captions, and marks the target as activated.
    /// Also, sets a flag so that the camera button will appear after the audio finishes.
    /// </summary>
    public void ActivateTarget()
    {
        // Activate the duck target.
        if (duckTargetTab != null)
        {
            duckTargetTab.SetActive(true);

            // Attempt to activate Bathtub Man Plane Tab.
            if (bathtubManPlaneTab != null)
            {
                bathtubManPlaneTab.SetActive(true);
            }
            else
            {
                // Look for a child named "Bathtub Man Plane Tab" if not directly assigned.
                Transform child = duckTargetTab.transform.Find("Bathtub Man Plane Tab");
                if (child != null)
                {
                    child.gameObject.SetActive(true);
                    bathtubManPlaneTab = child.gameObject; // Optionally store this reference.
                }
                else
                {
                    Debug.LogWarning("Bathtub Man Plane Tab not found as a child of duckTargetTab!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Duck Target Tab is not assigned!");
        }

        // Only proceed with audio, captions, and scan button deactivation if Bathtub Man Plane Tab is active.
        if (bathtubManPlaneTab != null && bathtubManPlaneTab.activeInHierarchy)
        {
            if (audioSourceTab != null && audioSourceTab.clip != null)
            {
                audioSourceTab.loop = false;
                audioSourceTab.Play();
                if (captions != null)
                    captions.SetActive(true);
                StartCoroutine(WaitForAudioEnd());
                // Mark that audio was triggered.
                audioTriggered = true;
            }
            else
            {
                Debug.LogWarning("Audio Source Tab is not assigned or has no clip!");
            }
            activated = true;
        }
        else
        {
            Debug.Log("Bathtub Man Plane Tab is not active; audio will not play and the scan button remains active.");
        }
    }

    /// <summary>
    /// Waits until the audio stops playing, then deactivates captions.
    /// </summary>
    private IEnumerator WaitForAudioEnd()
    {
        while (audioSourceTab != null && audioSourceTab.isPlaying)
            yield return null;
        if (captions != null)
            captions.SetActive(false);
        // Audio finished: the camera button logic in Update() will now show the camera button.
    }

    /// <summary>
    /// Called when the camera button is clicked.
    /// Plays the camera button animation and, after a delay, deactivates the AR marker.
    /// Also resets the audio trigger state so the process can repeat.
    /// </summary>
    private void OnCameraButtonClicked()
    {
        // Hide the camera button immediately.
        cameraButton.gameObject.SetActive(false);
        // Activate the camera animation GameObject before triggering animation.
        if (cameraAnim != null)
        {
            cameraAnim.gameObject.SetActive(true);
            cameraAnim.SetTrigger("Play");
        }
        else
        {
            Debug.LogWarning("Camera Animator is not assigned!");
        }
        // Start coroutine to deactivate the AR marker after a delay.
        StartCoroutine(DeactivateARMarkerAfterDelay());
    }

    private IEnumerator DeactivateARMarkerAfterDelay()
    {
        yield return new WaitForSeconds(cameraDelay);
        if (duckTargetTab != null)
        {
            duckTargetTab.SetActive(false);
            Debug.Log("Duck Target Tab deactivated.");
        }
        else
        {
            Debug.LogWarning("Duck Target Tab is not assigned!");
        }
        // Reset the audio trigger for the next cycle.
        audioTriggered = false;
    }

    /// <summary>
    /// Example method to deactivate the target if needed.
    /// </summary>
    public void SettingImageTargetOn()
    {
        if (duckTargetTab != null)
            duckTargetTab.SetActive(false);
    }
}

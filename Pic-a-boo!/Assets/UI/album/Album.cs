using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Album : MonoBehaviour
{
    // Assign these via the Inspector.
    // Order the images as desired (e.g., album0, album1, album2, …, album5)
    public List<GameObject> albumImages;

    // The camera button that triggers the next image.
    public Button cameraButton;

    // Delay before switching images (in seconds).
    public float delay = 2f;

    // Track the current active image index.
    private int currentIndex = 0;

    void Start()
    {
        if (albumImages == null || albumImages.Count == 0)
        {
            Debug.LogError("No album images assigned!");
            return;
        }

        // Activate only the first image; hide the rest.
        for (int i = 0; i < albumImages.Count; i++)
        {
            albumImages[i].SetActive(i == 0);
        }

        // Hook up the camera button's onClick event.
        if (cameraButton != null)
        {
            cameraButton.onClick.AddListener(OnCameraButtonClicked);
        }
        else
        {
            Debug.LogWarning("Camera Button is not assigned!");
        }
    }

    void OnCameraButtonClicked()
    {
        // Start the coroutine to switch the album image after a delay.
        StartCoroutine(SwitchAlbumImage());
    }

    IEnumerator SwitchAlbumImage()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(delay);

        // Deactivate the current image.
        albumImages[currentIndex].SetActive(false);

        // Move to the next image if there is one.
        if (currentIndex < albumImages.Count - 1)
        {
            currentIndex++;
        }
        else
        {
            Debug.Log("Reached the end of the album.");
            // Optionally, reset to 0 to loop:
            // currentIndex = 0;
        }

        // Activate the new current image.
        albumImages[currentIndex].SetActive(true);
    }
}
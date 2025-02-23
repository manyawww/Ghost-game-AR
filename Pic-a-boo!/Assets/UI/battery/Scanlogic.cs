using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scanlogic : MonoBehaviour
{
    // Assign these via the Inspector.
    // Add your images in the desired order (e.g., "scan15", "scan14", "scan13", …)
    public List<GameObject> scanNumber;

    // The scan button to trigger image change.
    public Button scanButton;

    // This index tracks the currently active image.
    private int currentIndex;

    void Start()
    {
        // Check if any images are assigned.
        if (scanNumber == null || scanNumber.Count == 0)
        {
            Debug.LogError("No images assigned!");
            return;
        }

        // Start with the 15th image (i.e. the last image in the list) showing.
        currentIndex = scanNumber.Count - 1;

        // Hide all images except the one at currentIndex.
        for (int i = 0; i < scanNumber.Count; i++)
        {
            scanNumber[i].SetActive(i == currentIndex);
        }

        // Hook the scan button's OnClick event.
        if (scanButton != null)
        {
            scanButton.onClick.AddListener(ShowPreviousImage);
        }
        else
        {
            Debug.LogWarning("Scan Button is not assigned!");
        }
    }

    // This method is called when the scan button is tapped.
    public void ShowPreviousImage()
    {
        // Only change if we haven't reached the first image.
        if (currentIndex > 0)
        {
            // Hide the current image.
            scanNumber[currentIndex].SetActive(false);

            // Move to the previous image.
            currentIndex--;

            // Show the new current image.
            scanNumber[currentIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Already at the first image.");
        }
    }
}

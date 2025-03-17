using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CaptureZone : MonoBehaviour
{
    public float captureTime = 5f; // Time required to capture the point
    private float currentCaptureTime = 0f;
    private bool isCapturing = false;
    private bool isCaptured = false;
    private PlayerController capturingPlayer; // Reference to the player capturing
    public Slider captureProgressBar; // UI element to show progress

    void Start()
    {
        if (captureProgressBar)
            captureProgressBar.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCaptured) return;

        if (other.CompareTag("Player"))
        {
            capturingPlayer = other.GetComponent<PlayerController>();
            if (capturingPlayer != null)
            {
                StartCoroutine(CaptureProgress());
                if (captureProgressBar)
                    captureProgressBar.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && capturingPlayer != null && other.gameObject == capturingPlayer.gameObject)
        {
            StopCapturing();
        }
    }

    IEnumerator CaptureProgress()
    {
        isCapturing = true;
        currentCaptureTime = 0f;

        while (currentCaptureTime < captureTime && isCapturing)
        {
            currentCaptureTime += Time.deltaTime;
            if (captureProgressBar)
                captureProgressBar.value = currentCaptureTime / captureTime;

            yield return null;
        }

        if (currentCaptureTime >= captureTime)
        {
            CapturePoint();
        }
    }

    void CapturePoint()
    {
        isCaptured = true;
        isCapturing = false;
        if (captureProgressBar)
            captureProgressBar.gameObject.SetActive(false);

        GameManager.Instance.RoundWon(capturingPlayer.teamID); // Notify the GameManager that the round is won
    }

    void StopCapturing()
    {
        isCapturing = false;
        capturingPlayer = null;
        currentCaptureTime = 0f;
        if (captureProgressBar)
            captureProgressBar.gameObject.SetActive(false);
    }
}

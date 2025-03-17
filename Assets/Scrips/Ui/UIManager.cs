using UnityEngine;
using TMPro;           // Import the TextMeshPro namespace
using UnityEngine.UI;  // Still needed for UI elements like Slider

public class UIManager : MonoBehaviour
{
    [Header("Display Elements")]
    // Use TextMeshProUGUI instead of Text for TMP support.
    public TextMeshProUGUI weaponDisplay;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI roundTimerText;

    // Slider for showing objective status (0-100).
    public Slider objectiveStatusSlider;

    /// <summary>
    /// Updates the weapon display with the given weapon name.
    /// </summary>
    public void UpdateWeaponDisplay(string weaponName)
    {
        if (weaponDisplay != null)
            weaponDisplay.text = "Weapon: " + weaponName;
    }

    /// <summary>
    /// Updates the ammo display with the current and maximum ammo.
    /// </summary>
    public void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        if (ammoDisplay != null)
            ammoDisplay.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }

    /// <summary>
    /// Updates the health display with the current health value.
    /// </summary>
    public void UpdateHealthDisplay(int health)
    {
        if (healthDisplay != null)
            healthDisplay.text = "Health: " + health;
    }

    /// <summary>
    /// Updates the round timer display with the remaining time.
    /// </summary>
    public void UpdateRoundTimer(float timeRemaining)
    {
        if (roundTimerText != null)
            roundTimerText.text = "Time: " + timeRemaining.ToString("F0");
    }

    /// <summary>
    /// Updates the objective status slider.
    /// Expects a value from 0 to 100.
    /// </summary>
    public void UpdateObjectiveStatus(float status)
    {
        if (objectiveStatusSlider != null)
            objectiveStatusSlider.value = Mathf.Clamp(status, 0, 100);
    }
}

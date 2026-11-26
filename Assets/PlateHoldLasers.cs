using UnityEngine;

public class PlateHoldLasers : MonoBehaviour
{
    public PressurePlate plate;
    public LaserToggle[] lasers;

    bool lastPressed;

    void Update()
    {
        bool pressed = plate.IsPressed;
        if (pressed == lastPressed) return;
        lastPressed = pressed;
        foreach (var l in lasers) l.SetLocked(pressed);   // off while pressed, back on when released
    }
}
using UnityEngine;

public class PlateLaserController : MonoBehaviour
{
    public PressurePlate plateA;
    public PressurePlate plateB;
    public LaserToggle[] lasers;
    public Collider2D levelArea;   // Level3_Area

    bool solved;
    float ignoreUntil;

    void OnEnable()  { PlayerRespawn.OnPlayerDied += OnDied; }
    void OnDisable() { PlayerRespawn.OnPlayerDied -= OnDied; }

    void Update()
    {
        if (solved || Time.time < ignoreUntil) return;

        if (plateA.IsPressed && plateB.IsPressed)
        {
            solved = true;   // latched: stays solved even if they leave the plates
            foreach (var l in lasers) l.SetLocked(true);
        }
    }

    void OnDied(Vector2 playerPos)
    {
        if (levelArea != null && !levelArea.OverlapPoint(playerPos)) return;

        solved = false;
        ignoreUntil = Time.time + 0.3f;   // lets the plates register the reset first
        foreach (var l in lasers) l.SetLocked(false);
    }
}
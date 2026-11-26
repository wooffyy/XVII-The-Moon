using UnityEngine;

public class PlateLatchDoor : MonoBehaviour
{
    public PressurePlate plate;
    public GameObject door;          // the Lvl4Exit tilemap
    public Collider2D levelArea;     // Level4_Area

    bool opened;
    float ignoreUntil;

    void Start() { SetDoor(false); }

    void OnEnable()  { PlayerRespawn.OnPlayerDied += OnDied; }
    void OnDisable() { PlayerRespawn.OnPlayerDied -= OnDied; }

    void Update()
    {
        if (opened || Time.time < ignoreUntil) return;

        if (plate.IsPressed)
        {
            opened = true;   // latched: stays open even after the plate is released
            SetDoor(true);
        }
    }

    void OnDied(Vector2 playerPos)
    {
        if (levelArea != null && !levelArea.OverlapPoint(playerPos)) return;

        opened = false;
        ignoreUntil = Time.time + 0.3f;   // lets the plates register the reset first
        SetDoor(false);
    }

    void SetDoor(bool open)
    {
        var col = door.GetComponent<Collider2D>();
        var rend = door.GetComponent<Renderer>();
        if (col) col.enabled = !open;
        if (rend) rend.enabled = !open;
    }
}
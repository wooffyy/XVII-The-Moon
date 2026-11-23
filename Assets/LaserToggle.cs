using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserToggle : MonoBehaviour
{
    public bool defaultOn = false;
    public bool toggleOnFlip = true;
    public Collider2D levelArea;

    bool isOn;
    bool locked;
    Collider2D col;
    TilemapRenderer tilemapRenderer;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(defaultOn);
    }

    void OnEnable()
    {
        PlayerController.OnSpaceFlip += Toggle;
        PlayerRespawn.OnPlayerDied += ResetState;
    }

    void OnDisable()
    {
        PlayerController.OnSpaceFlip -= Toggle;
        PlayerRespawn.OnPlayerDied -= ResetState;
    }

    bool PlayerInMyLevel(Vector2 playerPos)
    {
        return levelArea == null || levelArea.OverlapPoint(playerPos);
    }

    void Toggle(Vector2 playerPos)
    {
        if (!toggleOnFlip || !PlayerInMyLevel(playerPos)) return;
        SetState(!isOn);
    }

    void ResetState(Vector2 playerPos)
    {
        if (!PlayerInMyLevel(playerPos)) return;
        SetState(defaultOn);
    }

    public void SetLocked(bool value)
    {
        locked = value;
        SetState(isOn);
    }

    void SetState(bool on)
    {
        isOn = on;
        bool active = on && !locked;
        if (col) col.enabled = active;
        if (tilemapRenderer) tilemapRenderer.enabled = active;
        if (spriteRenderer) spriteRenderer.enabled = active;
    }
}
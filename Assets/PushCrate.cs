using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushCrate : MonoBehaviour
{
    public Collider2D levelArea;          // the area trigger for this crate's level
    public bool startFlipped = false;     // tick for level 4
    public bool flipsWithPlayer = true;   // reacts to Space flips in its own level

    Rigidbody2D rb;
    Vector3 startPos;
    float baseGravity;
    float gravityDir = 1f;
    float lastBeamTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        baseGravity = rb.gravityScale;
        SetGravityDir(startFlipped ? -1f : 1f);
    }

    void OnEnable()
    {
        PlayerController.OnSpaceFlip += OnPlayerFlip;
        PlayerRespawn.OnPlayerDied += OnPlayerDied;
    }

    void OnDisable()
    {
        PlayerController.OnSpaceFlip -= OnPlayerFlip;
        PlayerRespawn.OnPlayerDied -= OnPlayerDied;
    }

    bool InMyLevel(Vector2 pos)
    {
        return levelArea == null || levelArea.OverlapPoint(pos);
    }

    void SetGravityDir(float dir)
    {
        gravityDir = dir;
        rb.gravityScale = baseGravity * gravityDir;
    }

    void Flip()
    {
        SetGravityDir(-gravityDir);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.WakeUp();
    }

    void OnPlayerFlip(Vector2 playerPos)
    {
        Debug.Log($"Crate got flip. flipsWithPlayer={flipsWithPlayer}, levelArea={levelArea}, inArea={InMyLevel(playerPos)}, pos={playerPos}", this);
        if (!flipsWithPlayer || !InMyLevel(playerPos)) return;
        Flip();
    }

    public void BeamFlip()
    {
        if (Time.time - lastBeamTime < 0.3f) return;   // stops double triggers
        lastBeamTime = Time.time;
        Flip();
    }

    void OnPlayerDied(Vector2 playerPos)
    {
        if (!InMyLevel(playerPos)) return;

        SetGravityDir(startFlipped ? -1f : 1f);
        rb.linearVelocity = Vector2.zero;
        rb.position = startPos;
        transform.position = startPos;
    }
}
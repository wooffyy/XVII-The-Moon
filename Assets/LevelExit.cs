using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Where to send the player")]
    public Transform nextLevelStart;      // the yellow square at the start of level 2

    [Header("Camera")]
    public Transform cameraTarget;        // empty object centered on level 2
    public float cameraMoveTime = 1f;

    bool used;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        var player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;

        used = true;

        // Teleport
        var rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.position = nextLevelStart.position;
        player.transform.position = nextLevelStart.position;

        // Move camera
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        Transform cam = Camera.main.transform;
        Vector3 start = cam.position;
        Vector3 end = new Vector3(cameraTarget.position.x, cameraTarget.position.y, start.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, cameraMoveTime);
            cam.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        cam.position = end;
    }
}
using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    public float flashTime = 0.5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.6f);

    Rigidbody2D rb;
    PlayerController controller;
    Texture2D tex;
    float flashAlpha;
    bool dying;
    public static event System.Action<Vector2> OnPlayerDied;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        respawnPoint = transform.position;   // level 1 start = where you placed the player

        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
    }

    public void SetRespawn(Vector3 point)
    {
        respawnPoint = point;
    }

    public void Die()
    {
        if (!dying) StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        dying = true;

        rb.linearVelocity = Vector2.zero;
        rb.position = respawnPoint;
        transform.position = respawnPoint;
        controller.ResetGravity();
        OnPlayerDied?.Invoke(transform.position);

        float t = 0f;
        while (t < flashTime)
        {
            t += Time.deltaTime;
            flashAlpha = Mathf.Lerp(flashColor.a, 0f, t / flashTime);
            yield return null;
        }

        flashAlpha = 0f;
        dying = false;
    }

    void OnGUI()
    {
        if (flashAlpha <= 0f) return;
        Color old = GUI.color;
        GUI.color = new Color(flashColor.r, flashColor.g, flashColor.b, flashAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
        GUI.color = old;
    }
}
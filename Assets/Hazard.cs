using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var respawn = other.GetComponentInParent<PlayerRespawn>();
        if (respawn != null) respawn.Die();
    }
}
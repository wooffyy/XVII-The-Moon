using UnityEngine;

public class GravityBeam : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player != null) { player.BeamFlip(); return; }

        var crate = other.GetComponentInParent<PushCrate>();
        if (crate != null) crate.BeamFlip();
    }
}
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    int count;
    public bool IsPressed => count > 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsActivator(other)) count++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsActivator(other)) count = Mathf.Max(0, count - 1);
    }

    bool IsActivator(Collider2D other)
    {
        return other.GetComponentInParent<PlayerController>() != null
            || other.GetComponentInParent<PushCrate>() != null;
    }
}
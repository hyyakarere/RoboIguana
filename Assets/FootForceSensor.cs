using UnityEngine;

public class FootForceSensor : MonoBehaviour
{
    [Header("Output (read only)")]
    public Vector3 groundReactionForce;
    public float verticalForce;
    public float totalForceMagnitude;

    void OnCollisionStay(Collision collision)
    {
        // Only measure ground layer if needed
        // if (!collision.gameObject.CompareTag("Ground")) return;

        Vector3 impulse = collision.impulse;

        // Convert impulse to force
        groundReactionForce = impulse / Time.fixedDeltaTime;

        verticalForce = groundReactionForce.y;
        totalForceMagnitude = groundReactionForce.magnitude;
    }

    void OnCollisionExit(Collision collision)
    {
        groundReactionForce = Vector3.zero;
        verticalForce = 0f;
        totalForceMagnitude = 0f;
    }
}
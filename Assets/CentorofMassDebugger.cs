using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(ArticulationBody))]
public class CenterOfMassDebugger : MonoBehaviour
{
    public float sphereRadius = 0.01f;
    public bool printToConsole = false;

    [Header("Read-Only Runtime Data")]
    [SerializeField] private Vector3 localCenterOfMass;
    [SerializeField] private Vector3 worldCenterOfMass;
    [SerializeField] private Vector3 inertiaTensor;

    ArticulationBody ab;

    void Awake()
    {
        ab = GetComponent<ArticulationBody>();
    }

    void Update()
    {
        if (ab == null)
            ab = GetComponent<ArticulationBody>();

        if (ab == null) return;

        // 更新 Inspector 显示的数据
        localCenterOfMass = ab.centerOfMass;
        worldCenterOfMass = ab.worldCenterOfMass;
        inertiaTensor = ab.inertiaTensor;

        if (printToConsole)
        {
            Debug.Log($"{name} | Local COM: {localCenterOfMass} | " +
                      $"World COM: {worldCenterOfMass} | " +
                      $"Inertia: {inertiaTensor}");
        }
    }

    void OnDrawGizmos()
    {
        if (ab == null)
            ab = GetComponent<ArticulationBody>();

        if (ab == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ab.worldCenterOfMass, sphereRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, ab.worldCenterOfMass);
    }
}
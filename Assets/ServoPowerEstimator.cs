using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ArticulationBody))]
public class ServoPowerEstimator : MonoBehaviour
{
    [Header("Motor parameters (output referred)")]
    public float Kt = 0.83f;       // Nm/A
    public float Ke = 0.98f;       // V·s/rad
    public float R = 1.138f;       // Ohm
    public float tauC = 0.1f;      // Coulomb friction Nm
    public float viscousB = 0.02f; // Nm/(rad/s)

    [Header("Computed values (read only)")]
    public float driveTorqueNm;      // Estimated PD torque
    public float solverTorqueNm;     // Unity drive torque
    public float omegaRadPerSec;
    public float currentA;
    public float voltageV;
    public float stiffness;
    public float damping;

    public float mechanicalPowerW;
    public float resistiveLossW;
    public float electricalPowerW;
    public float electricalPowerConsumed;

    public float jointAngleRad;
    public float targetAngleRad;

    private ArticulationBody joint;

    // REQUIRED for GetDriveForces
    private List<float> driveForces = new List<float>(1);

    void Awake()
    {
        joint = GetComponent<ArticulationBody>();
    }

    void FixedUpdate()
    {
        ArticulationDrive drive = joint.xDrive;

        // -------------------------
        // Joint state (radians)
        // -------------------------
        jointAngleRad = joint.jointPosition[0];
        omegaRadPerSec = joint.jointVelocity[0];

        // -------------------------
        // Convert target (degrees → radians)
        // -------------------------
        targetAngleRad = drive.target * Mathf.Deg2Rad;
        float targetVelRad = drive.targetVelocity * Mathf.Deg2Rad;

        // -------------------------
        // PD torque
        // τ = Kp*error + Kd*vel_error
        // -------------------------
        float posError = targetAngleRad - jointAngleRad;
        float velError = targetVelRad - omegaRadPerSec;

        stiffness = drive.stiffness;
        damping = drive.damping;

        driveTorqueNm =
            drive.stiffness * posError +
            drive.damping * velError;

        // Apply force limit
        driveTorqueNm = Mathf.Clamp(
            driveTorqueNm,
            -drive.forceLimit,
            drive.forceLimit);

        // -------------------------
        // Friction model
        // -------------------------
        float frictionTorque =
            tauC * Mathf.Sign(omegaRadPerSec) +
            viscousB * omegaRadPerSec;

        float totalMotorTorque = driveTorqueNm - frictionTorque;

        // -------------------------
        // Electrical model
        // τ = Kt * I
        // V = IR + Ke ω
        // -------------------------
        currentA = totalMotorTorque / Kt;
        voltageV = currentA * R + Ke * omegaRadPerSec;

        mechanicalPowerW = totalMotorTorque * omegaRadPerSec;
        resistiveLossW = currentA * currentA * R;
        electricalPowerW = voltageV * currentA;

        // No regeneration
        electricalPowerConsumed = Mathf.Max(0f, electricalPowerW);
    }

    // Read real drive torque after physics step
    void LateUpdate()
    {
        driveForces.Clear();
        joint.GetDriveForces(driveForces);

        if (driveForces.Count > 0)
            solverTorqueNm = driveForces[0];
        else
            solverTorqueNm = 0f;
    }
}
using UnityEngine;

public class JointAngleDebug : MonoBehaviour
{
    [Header("Assign the LINK objects that have ArticulationBody")]
    public ArticulationBody hipFLyaw;
    public ArticulationBody hipFRyaw;
    public ArticulationBody hipRLyaw;
    public ArticulationBody hipRRyaw;

    public ArticulationBody hipFL;
    public ArticulationBody hipFR;
    public ArticulationBody hipRL;
    public ArticulationBody hipRR;

    public ArticulationBody SEAFL;
    public ArticulationBody SEAFR;
    public ArticulationBody SEARL;
    public ArticulationBody SEARR;

    public ArticulationBody kneeFL;
    public ArticulationBody kneeFR;
    public ArticulationBody kneeRL;
    public ArticulationBody kneeRR;

    [Tooltip("Print every N physics steps")]
    public int printEvery = 10;

    int _counter;

    void FixedUpdate()
    {
        _counter++;
        if (_counter % Mathf.Max(1, printEvery) != 0) return;

        Debug.Log(
            $"HIPyaw deg: FL {Deg(hipFLyaw):F2}, FR {Deg(hipFRyaw):F2}, RL {Deg(hipRLyaw):F2}, RR {Deg(hipRRyaw):F2} | " +
            $"HIP deg: FL {Deg(hipFL):F2}, FR {Deg(hipFR):F2}, RL {Deg(hipRL):F2}, RR {Deg(hipRR):F2} | " +
            $"SEA deg: FL {Deg(SEAFL):F2}, FR {Deg(SEAFR):F2}, RL {Deg(SEARL):F2}, RR {Deg(SEARR):F2} | " +
            $"KNEE deg: FL {Deg(kneeFL):F2}, FR {Deg(kneeFR):F2}, RL {Deg(kneeRL):F2}, RR {Deg(kneeRR):F2}"
        );
    }

    static float Deg(ArticulationBody ab)
    {
        if (ab == null) return float.NaN;
        if (ab.jointPosition.dofCount < 1) return float.NaN;

        // jointPosition is radians
        return ab.jointPosition[0] * Mathf.Rad2Deg;
    }
}

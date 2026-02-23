using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class RecordPosition : MonoBehaviour
{
    public Transform legBase;
    public Transform footTip;

    private StreamWriter writer;
    private bool isRecording = false;
    private Coroutine delayedStartRoutine;

    void Start()
    {
        string path = Application.dataPath + "/positionrecord.csv";
        writer = new StreamWriter(path);
        writer.WriteLine("time,x_rel,y_rel,z_rel");

        Toggle[] toggles = FindObjectsByType<Toggle>(FindObjectsSortMode.None);

        foreach (Toggle t in toggles)
        {
            if (t.gameObject.name == "KeyboardToggle")
            {
                t.onValueChanged.AddListener(OnToggleChanged);
                Debug.Log("Recording linked to KeyboardToggle");
            }
        }
    }

    void OnToggleChanged(bool isOn)
    {
        Debug.Log("Toggle state = " + isOn);

        if (isOn)
        {
            // Start 2-second delayed recording
            delayedStartRoutine = StartCoroutine(StartRecordingAfterDelay(2f));
        }
        else
        {
            // Stop recording immediately
            if (delayedStartRoutine != null)
                StopCoroutine(delayedStartRoutine);

            isRecording = false;
            Debug.Log("Recording stopped");
        }
    }

    IEnumerator StartRecordingAfterDelay(float delay)
    {
        Debug.Log("Waiting 2 seconds before recording...");
        yield return new WaitForSeconds(delay);

        isRecording = true;
        Debug.Log("Recording started");
    }

    void FixedUpdate()
    {
        if (!isRecording)
            return;

        if (legBase == null || footTip == null)
        {
            Debug.LogWarning("Transforms not assigned!");
            return;
        }

        Vector3 pRel = legBase.InverseTransformPoint(footTip.position);
        writer.WriteLine($"{Time.time},{pRel.x},{pRel.y},{pRel.z}");
        writer.Flush();
    }

    void OnDestroy()
    {
        writer?.Close();
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MissionDeliveryWaypoint : MonoBehaviour
{
    private MissionManager manager;
    private bool missionCompleted = false;

    void Start()
    {
        manager = FindObjectOfType<MissionManager>();
        if (manager == null)
        {
            Debug.LogError("MissionManager не найден в сцене!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null || missionCompleted) return;

        if (other.CompareTag("Truck"))
        {
            if (manager.CurrentState == MissionManager.MissionState.Active)
            {
                missionCompleted = true; 
                manager.CompleteMission();
            }
        }
    }
    
    private void OnEnable()
    {
        missionCompleted = false;
    }
}
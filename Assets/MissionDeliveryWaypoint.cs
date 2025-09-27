using UnityEngine;

// ������� ������� Collider, ������������ ��� �������
[RequireComponent(typeof(Collider))]
public class MissionDeliveryWaypoint : MonoBehaviour
{
    private MissionManager manager;

    void Start()
    {
        manager = FindObjectOfType<MissionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ��� ������ � ����� "Truck" ��� "Car"
        if (other.CompareTag("Truck") || other.CompareTag("Car"))
        {
            // ����������, ��� ������ �������
            if (manager.CurrentState == MissionManager.MissionState.Active)
            {
                manager.CompleteMission();
            }
        }
    }
}